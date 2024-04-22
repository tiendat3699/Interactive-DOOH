using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GadGame.Manager;
using GadGame.Network;
using GadGame.State;
using GadGame.State.GameState;
using Pools.Runtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GadGame.MiniGame
{
    public class MiniGameController : StateRunner<MiniGameController>
    {
        [Header("Stats")]
        public int GameTime;
        [SerializeField] private Basket _basket;
        [SerializeField] private float _lerp;
        [SerializeField] private float _spawnTime;
        [SerializeField, Range(0,1)] private float _bombChange;
        [SerializeField] private Rect _spawnArea;
        [SerializeField, MinMaxSlider(0, 2, true)] private Vector2 _gravityScaleRange;

        [Header("UI")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _resultScore;
        [SerializeField] private CanvasGroup _result;

        [Header("Tutorial")]
        [SerializeField] private GameObject _tutorialWrapper;
        [SerializeField] private Transform _thisYou;
        [SerializeField] private Transform _right;
        [SerializeField] private Transform _left;
        [SerializeField] private Transform _end;
        
        [Header("Pool")]
        [SerializeField] private Pool<Item>[] _itemPools;
        [SerializeField] private Pool<Bomb>[] _bombPools;

        private GameManager _gameManager;
        private Camera _camera;
        private float _spawnTimer;
        private Vector3 _preFramePosition;
        
        private void Start()
        {
            _gameManager = GameManager.Instance;   
            _gameManager.OnPause += Pause;
            _gameManager.OnResume += Resume;
            _gameManager.OnScoreUpdate += OnScoreUpdate;
            _camera = Camera.main;
            _time.text = GameTime.ToString();
            SoundManager.Instance.PlayMusic(MusicDefine.BG);
            SetState<TutorialGameState>();
        }

        private void LateUpdate()
        {
            _preFramePosition = _basket.Position;
        }

        public void SpawnRandomItem()
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnTime)
            {
                _spawnTimer = 0;
                var gravity = Random.Range(_gravityScaleRange.x, _gravityScaleRange.y);
                var bombChance = Random.value;
                if (bombChance <= _bombChange)
                {
                    var random = Random.Range(0, _bombPools.Length);
                    var bomb = _bombPools[random].Get();
                    bomb.Init(gravity);
                    bomb.transform.position = _spawnArea.RandomPointInside();
                }
                else
                {
                    var random = Random.Range(0, _itemPools.Length);
                    var item = _itemPools[random].Get();
                    item.Init(gravity);
                    item.transform.position = _spawnArea.RandomPointInside();
                }
            }
        }
        
        public void PlayerControl()
        {
            //640x480;
            var inputData = UdpSocket.Instance.DataReceived.PosPoint;
            var inputNormalize = new Vector2((inputData.x - 213.33f)/ 213.33f, inputData.y / 480);
            // var inputNormalize = new Vector2(inputData.x/ 200, inputData.y / 480);
            var input = new Vector2
            {
                x = Mathf.Lerp(0, _canvas.pixelRect.width, inputNormalize.x),
                y = -Mathf.Lerp(0, _canvas.pixelRect.height, inputNormalize.y)
            };
            if (input != Vector2.zero)
            {
                var mousePos = input;
                var pos = _camera.ScreenToWorldPoint(mousePos);
                var currentPosition = _basket.Position;
                pos.x *= -1;
                pos.y = currentPosition.y;
                pos.z = 0;
                currentPosition= Vector3.Lerp(currentPosition, pos, _lerp * Time.deltaTime);
                currentPosition.x = Mathf.Clamp(currentPosition.x, -2.25f, 2.25f);
                var dirMove = (_preFramePosition - currentPosition).normalized;
                _basket.transform.DORotate(new Vector3(0, 0, 10 * dirMove.x), 0.2f);
                _basket.Position = currentPosition;
            }
        }

        public async void ShowTutorial()
        {
            _tutorialWrapper.SetActive(true);
            _thisYou.localScale = Vector3.zero;
            _right.localScale = Vector3.zero;
            _left.localScale = Vector3.zero;
            _end.localScale = Vector3.zero;
            await _thisYou.DOScale(Vector3.one, 0.2f);
            await UniTask.Delay(2000);
            await _right.DOScale(Vector3.one, 0.2f).OnStart(()=> _thisYou.DOScale(Vector3.zero, 0.2f));
            await UniTask.WaitUntil(() => _basket.Position.x >= 1.5f);
            await _left.DOScale(Vector3.one, 0.2f).OnStart(()=> _right.DOScale(Vector3.zero, 0.2f));
            await UniTask.WaitUntil(() => _basket.Position.x <= -1.5f);
            await _left.DOScale(Vector3.zero, 0.2f);
            await _end.DOScale(Vector3.one, 0.2f);
            await UniTask.Delay(2000);
            await _end.DOScale(Vector3.zero, 0.2f);
            _tutorialWrapper.SetActive(false);
            SetState<PlayingGameState>();
        }

        public async void ShowResult()
        {
            await _result.DOFade(1, 0.3f);
            await _resultScore.DOText(_gameManager.Score.ToString(), 1f, scrambleMode: ScrambleMode.Numerals);
        }

        public void SetActive(bool value)
        {
            _basket.Active = value;
        }

        public void SetTextTime(float time)
        {
            _time.text = Mathf.Floor(time).ToString();
        }

        private void OnDestroy()
        {
            _gameManager.OnPause -= Pause;
            _gameManager.OnResume -= Resume;
            SoundManager.Instance.StopMusic();
        }
    
        [Button, HideInEditorMode]
        private void Pause()
        {
            SetState<PauseGameState>();
        }

        [Button, HideInEditorMode]
        private void Resume()
        {
            SetState<ResumeGameState>();
        }
        
        private void OnScoreUpdate(int score)
        {
            _score.text = score.ToString();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0f, 0.3f);
            Gizmos.DrawCube(_spawnArea.position, _spawnArea.size);
        }
    }
}