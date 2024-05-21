using System.Collections.Generic;
using GadGame.Singleton;
using GraphQlClient.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace GadGame.Network
{
    public struct CreateQueryOptions
    {
        public string Name;
        public GraphApi.Query.Type QueryType;
        public string Root;
        public string ReturnType;
        public string Projection;
        public object QueryArgs;
    }

    public class P4PGraphqlManager : PersistentSingleton<P4PGraphqlManager>
    {
        [SerializeField] private GraphApi _p4pGraph;
        [SerializeField] private string _machineMac;
        [SerializeField] private string _promotionId;

        [ShowInInspector] private string _userID;

        protected override void Awake()
        {
            base.Awake();
            LoginMachine();
            // CreateGuest();
        }

        private GraphApi.Query CreateQuery(CreateQueryOptions options)
        {
            GraphApi.Query query = new GraphApi.Query
            {
                fields = new List<GraphApi.Field>(),
                isComplete = false,
                name = options.Name,
                query = options.Projection
                    .Replace(System.Environment.NewLine, "\n"),
                queryOptions = new List<string>(),
                queryString = options.Root,
                returnType = options.ReturnType,
                type = options.QueryType
            };

            if (options.QueryArgs != null)
            {
                query.SetArgs(options.QueryArgs);
            }
            
            return query;
        }

        private async void LoginMachine()
        {
            GraphApi.Query query = _p4pGraph.GetQueryByName("LoginMachine", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    macAddress = _machineMac,
                    password = "Abc@123"
                }
            });
            UnityWebRequest request = await _p4pGraph.Post(query);
            
        }

        public async void CreateGuest()
        {
            GraphApi.Query query = _p4pGraph.GetQueryByName("CreateGuest", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    password = "Abc@123"
                }
            });
            UnityWebRequest request = await _p4pGraph.Post(query);
            Debug.Log("login machine " + request.result);
            string json = HttpHandler.FormatJson(request.downloadHandler.text);
            JObject obj = JObject.Parse(json);
            Debug.Log(obj["data"]);
        }

        public async void JoinPromotion()
        {
            GraphApi.Query query = _p4pGraph.GetQueryByName("JoinPromotion", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    promotionId = _promotionId
                }
            });
            UnityWebRequest request = await _p4pGraph.Post(query);
            GetData(request.downloadHandler.text);
        }

        public async void SubmitGameSession()
        {
            GraphApi.Query query = _p4pGraph.GetQueryByName("SubmitGameSession", GraphApi.Query.Type.Mutation);
            query.SetArgs(new
            {
                input = new
                {
                    playerID = _userID,
                    promotionId = _promotionId
                }
            });
            UnityWebRequest request = await _p4pGraph.Post(query);
            GetData(request.downloadHandler.text);
        }

        private void GetData(string data)
        {
            string json = HttpHandler.FormatJson(data);
            Debug.Log(JsonConvert.DeserializeObject(json));
        }
    }
}