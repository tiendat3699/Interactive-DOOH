using GadGame.Manager;
using GadGame.State;
using GadGame.State.GameState;
using Pools.Runtime;
using TMPro;
using UnityEngine;

namespace GadGame.MiniGame
{
    public class MiniGameController : StateRunner<MiniGameController>
    {
        public int GameTime;
        
        [SerializeField] private Transform _basket;
        [SerializeField] private TextMeshProUGUI _time;
        [SerializeField] private float _lerp;
        [SerializeField] private float _spawnTime;
        [SerializeField, Range(0,1)] private float _bombChange;
        [SerializeField] private Rect _spawnArea;
        
        [SerializeField] private Pool<Item>[] _itemPools;
        [SerializeField] private Pool<Bomb>[] _bombPools;

        private GameManager _gameManager;
        private Camera _camera;
        private float _spawnTimer;
        
        private void Start()
        {
            _gameManager = GameManager.Instance;   
            _gameManager.OnPause += Pause;
            _gameManager.OnResume += Resume;
            _camera = Camera.main;
            SetState<PlayingGameState>();
            _time.text = GameTime.ToString();
        }

        public void SpawnRandomItem()
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnTime)
            {
                _spawnTimer = 0;
                var bombChance = Random.value;
                if (bombChance <= _bombChange)
                {
                    var random = Random.Range(0, _bombPools.Length);
                    var bomb = _bombPools[random].Get();
                    bomb.transform.position = _spawnArea.RandomPointInside();
                }
                else
                {
                    var random = Random.Range(0, _itemPools.Length);
                    var item = _itemPools[random].Get();
                    item.transform.position = _spawnArea.RandomPointInside();
                }
            }
        }
        
        public void PlayerControl()
        {
            if (Input.GetMouseButton(0))
            {
                var mousePos = Input.mousePosition;
                var pos = _camera.ScreenToWorldPoint(mousePos);
                var currentPosition = _basket.position;
                pos.y = currentPosition.y;
                pos.z = 0;
                _basket.position = Vector3.Lerp(currentPosition, pos, _lerp * Time.deltaTime);
            }

        }

        public void SetTextTime(float time)
        {
            _time.text = Mathf.Floor(time).ToString();
        }

        private void OnDestroy()
        {
            _gameManager.OnPause -= Pause;
            _gameManager.OnResume -= Resume;
        }

        private void Pause()
        {
            SetState<PauseGameState>();
        }

        private void Resume()
        {
            SetState<ResumeGameState>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0f, 0.3f);
            Gizmos.DrawCube(_spawnArea.position, _spawnArea.size);
        }
    }
}