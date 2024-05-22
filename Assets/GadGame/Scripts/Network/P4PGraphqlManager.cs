using System;
using System.Net.WebSockets;
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
using ZXing;
using ZXing.QrCode;

namespace GadGame.Network
{

    struct DataReceive
    {
        public JObject[] errors;
        public JObject data;
    }

    public class P4PGraphqlManager : PersistentSingleton<P4PGraphqlManager>
    {
        [SerializeField] private GraphApi _graphApi;
        [SerializeField] private string _machineMac;
        [SerializeField] private string _promotionId;

        private DateTime _startTime;
        private string _userId;
        private string _userAccessToken;
        private string _machineAccessToken;

        public Action<Guest> OnGuestUpdatedSubscription;

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
            var Jobj = JObject.Parse(dataReceived.data);
            Debug.Log(dataReceived.data);
            var data = Jobj["payload"]!["data"]!["guestUpdatedSubscription"]!.ToObject<Guest>();
            OnGuestUpdatedSubscription?.Invoke(data);
        }

        private DataReceive GetData(string data)
        {
            var json = HttpHandler.FormatJson(data);
            return JsonConvert.DeserializeObject<DataReceive>(json);
        }

        public async Task<bool> LoginMachine()
        {
            var query = _graphApi.GetQueryByName("LoginAsGameMachine", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    macAddress = _machineMac,
                    password = "Abc@123"
                }
            });
            var request = await _graphApi.Post(query);
            if (request.result == UnityWebRequest.Result.Success)
            {
                var receive = GetData(request.downloadHandler.text);
                if (receive.data != null)
                {
                    var loginDetail = receive.data["loginAsGameMachine"]!.ToObject<LoginDetails>();
                    _machineAccessToken = loginDetail.accessToken;
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> CreateGuest()
        {
            var query = _graphApi.GetQueryByName("CreateGuest", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    password = "Abc@123"
                }
            });
            var request = await _graphApi.Post(query);
            if (request.result == UnityWebRequest.Result.Success)
            {
                var receive = GetData(request.downloadHandler.text);
                if (receive.data != null)
                {
                    var loginDetails = receive.data["createGuest"]!.ToObject<LoginDetails>();
                    _userId = loginDetails.user.id;
                    _userAccessToken = loginDetails.accessToken;
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> JoinPromotion()
        {
            var query = _graphApi.GetQueryByName("JoinPromotion", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    promotionId = _promotionId
                }
            });
            
            _graphApi.SetAuthToken(_userAccessToken);
            var request = await _graphApi.Post(query);
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
            var query = _graphApi.GetQueryByName("SubmitGameSession", GraphApi.Query.Type.Mutation);
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
            
            _graphApi.SetAuthToken(_machineAccessToken);
            var request = await _graphApi.Post(query);
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

        public async Task<Texture2D> GetQrLink()
        {
            return await GuestUpdatedSubscription();
        }

        private async Task<Texture2D> GuestUpdatedSubscription()
        {
            var query = _graphApi.GetQueryByName("GuestUpdatedSubscription", GraphApi.Query.Type.Subscription);
            query.SetArgs(new
            {
                guestId = _userId,
            });
            _graphApi.SetAuthToken(_machineAccessToken);
            var socket = await _graphApi.Subscribe(query);
            if (socket.State == WebSocketState.Open)
            {
                var link = $"https://play4promo.online/brands/{_promotionId}/scan-qr?token={_userAccessToken}";
                Debug.Log(link);
                return EncodeTextToQrCode(link);
            }

            return null;
        }
        
        private Color32 [] Encode(string textForEncoding, int width, int height)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }
        

        private Texture2D EncodeTextToQrCode(string input, int width = 256, int height = 256)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                Debug.Log("You should write something");
                return null;
            }
            
            var texture = new Texture2D(width, height);
            Color32 [] convertPixelToTexture = Encode(input, texture.width, texture.height);
            texture.SetPixels32(convertPixelToTexture);
            texture.Apply();

            return texture;
        }
    }
}