using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GadGame.Singleton;
using GraphQlClient.Core;
using GraphQlClient.EventCallbacks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace GadGame.Network
{

    class DataReceive
    {
        public JObject[] errors = null;
        public JObject data = null;
    }
    
    public class P4PGraphqlManager : PersistentSingleton<P4PGraphqlManager>
    {
        [SerializeField] private GraphApi _p4pGraph;
        [SerializeField] private string _machineMac;
        [SerializeField] private string _promotionId;

        private DateTime _startTime;
        [ShowInInspector, HideInEditorMode] private string _userId;

        public Action<OnSubscriptionDataReceived> OnGuestUpdatedSubscription;

        protected override async void Awake()
        {
            base.Awake();
            await LoginMachine();
            await CreateGuest();
            await JoinPromotion();
            await UniTask.Delay(5000);
            await SubmitGameSession(1000);
            GetQrLink();
        }

        private void OnEnable()
        {
            OnSubscriptionDataReceived.RegisterListener(OnGuestUpdated);
        }

        private void OnDisable()
        {
            OnSubscriptionDataReceived.UnregisterListener(OnGuestUpdated);
        }

        private void OnGuestUpdated(OnSubscriptionDataReceived dataReceived)
        {
            OnGuestUpdatedSubscription?.Invoke(dataReceived);
        }

        private DataReceive GetData(string data)
        {
            var json = HttpHandler.FormatJson(data);
            return JsonConvert.DeserializeObject<DataReceive>(json);
        }

        private async Task<bool> LoginMachine()
        {
            var query = _p4pGraph.GetQueryByName("LoginAsGameMachine", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    macAddress = _machineMac,
                    password = "Abc@123"
                }
            });
            var request = await _p4pGraph.Post(query);
            Debug.Log("LoginAsGameMachine " + request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                var receive = GetData(request.downloadHandler.text);
                if (receive.errors != null)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> CreateGuest()
        {
            var query = _p4pGraph.GetQueryByName("CreateGuest", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    password = "Abc@123"
                }
            });
            var request = await _p4pGraph.Post(query);
            Debug.Log("CreateGuest " + request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                var receive = GetData(request.downloadHandler.text);
                if (receive.data != null)
                {
                    var guest = receive.data["createGuest"];
                    _p4pGraph.SetAuthToken(guest?["accessToken"]?.ToString());
                    _userId = guest?["user"]?["id"]?.ToString();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> JoinPromotion()
        {
            var query = _p4pGraph.GetQueryByName("JoinPromotion", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    promotionId = _promotionId
                }
            });
            var request = await _p4pGraph.Post(query);
            Debug.Log("JoinPromotion " + request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                var receive = GetData(request.downloadHandler.text);
                if (receive.errors == null)
                {
                    _startTime = DateTime.Now;
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> SubmitGameSession(int gameScore)
        {
            var endTime = DateTime.Now.AddSeconds(-1);
            var query = _p4pGraph.GetQueryByName("SubmitGameSession", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    playerId = _userId,
                    promotionId = _promotionId,
                    startAt = _startTime,
                    endAt = endTime,
                    score = gameScore,
                }
            });
            var request = await _p4pGraph.Post(query);
            Debug.Log("Submit Game Session " + request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                var receive = GetData(request.downloadHandler.text);
                if (receive.errors == null)
                {
                    return true;
                }
            }

            return false;
        }

        public async void GetQrLink()
        {
            await GuestUpdatedSubscription();
        }

        private async Task GuestUpdatedSubscription()
        {
            var query = _p4pGraph.GetQueryByName("GuestUpdatedSubscription", GraphApi.Query.Type.Subscription);
            query.SetArgs(new
            {
                input = new
                {
                    guestId = _userId,
                }
            });
            await _p4pGraph.Subscribe(query);
        }
    }
}