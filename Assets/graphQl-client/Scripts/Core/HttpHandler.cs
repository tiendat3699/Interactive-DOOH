using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GraphQlClient.EventCallbacks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace GraphQlClient.Core
{
	public class HttpHandler
	{
		public static async Task<UnityWebRequest> PostAsync(string url, string details, string authToken = null){
            string jsonData = JsonConvert.SerializeObject(new{query = details});
            byte[] postData = Encoding.UTF8.GetBytes(jsonData);
            var uri = new Uri(url);
            UnityWebRequest request = new UnityWebRequest(uri, "POST")
            {
	            uploadHandler = new UploadHandlerRaw(postData),
	            downloadHandler = new DownloadHandlerBuffer(),
	            disposeCertificateHandlerOnDispose = true,
	            disposeDownloadHandlerOnDispose = true,
	            disposeUploadHandlerOnDispose = true
            };
            request.SetRequestHeader("Content-Type", "application/json");
            if (!String.IsNullOrEmpty(authToken)) 
                request.SetRequestHeader("Authorization", "Bearer " + authToken);
            
            OnRequestBegin  requestBegin = new OnRequestBegin();
            requestBegin.FireEvent();
            
            try{
	            await request.SendWebRequest();
                while (!request.isDone)
                {
	                await Task.Yield();
                }
            }
            catch(Exception e){
                Debug.Log(e);
                OnRequestEnded requestFailed = new OnRequestEnded(e);
                requestFailed.FireEvent();
            }
			Debug.Log(request.downloadHandler.text);
            
            OnRequestEnded requestSucceeded = new OnRequestEnded(request.downloadHandler.text);
            requestSucceeded.FireEvent();
            return request;
        }


		public static async Task<UnityWebRequest> GetAsync(string url, string authToken = null){
            UnityWebRequest request = UnityWebRequest.Get(url);
            if (!String.IsNullOrEmpty(authToken)) 
                request.SetRequestHeader("Authorization", "Bearer " + authToken);
            OnRequestBegin  requestBegin = new OnRequestBegin();
            requestBegin.FireEvent();
            try{
                await request.SendWebRequest();
            }
            catch(Exception e){
                Debug.Log("Testing exceptions");
                OnRequestEnded requestEnded = new OnRequestEnded(e);
                requestEnded.FireEvent();
            }
            Debug.Log(request.downloadHandler.text);
            OnRequestEnded requestSucceeded = new OnRequestEnded(request.downloadHandler.text);
            requestSucceeded.FireEvent();
            return request;
        }
        
        #region Websocket

        //Use this to subscribe to a graphql endpoint
		public static async Task<ClientWebSocket> WebsocketConnect(string subscriptionUrl, string details, string authToken = null, string socketId = "1", string protocol = "graphql-transport-ws"){
			string subUrl = subscriptionUrl.Replace("http", "ws");
			string id = socketId;
			ClientWebSocket cws = new ClientWebSocket();
			cws.Options.AddSubProtocol(protocol);
			Uri uri = new Uri(subUrl);
			try{
				Debug.Log("Websocket is connecting");
				await cws.ConnectAsync(uri, CancellationToken.None);
				await WebsocketInit(cws, id, authToken);
				await WebsocketSubscribe(cws, id, details);
			}
			catch (Exception e){
				Debug.Log("Error: " + e.Message);
			}

			return cws;
		}
		

		static async Task WebsocketInit(ClientWebSocket cws, string id, string authToken){
			string jsonData = JsonConvert.SerializeObject(new
			{
				id,
				type = "connection_init",
				payload = new
				{
					authorization = "Bearer "+ authToken
				}
				
			}, Formatting.None);
			Debug.Log("Websocket is starting");
			ArraySegment<byte> b = new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonData));
			await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
			Debug.Log("Websocket is updating");
			WebSocketUpdate(cws);
		}
		
		static async Task WebsocketSubscribe(ClientWebSocket cws, string id, string details){
			string jsonData = JsonConvert.SerializeObject(new
			{
				id,
				type = "subscribe",
				payload = new
				{
					query = details
				}
			}, Formatting.None);
			ArraySegment<byte> b = new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonData));
			await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
		}
		
		//Call GetWsReturn to wait for a message from a websocket. GetWsReturn has to be called for each message
		static async void WebSocketUpdate(ClientWebSocket cws){
			while (true)
			{
				if (!(cws.State == WebSocketState.Connecting || cws.State == WebSocketState.Open))
				{
					Debug.Log("websocket was closed, stop the loop");
					break;
				}
				
				ArraySegment<byte> buf = WebSocket.CreateClientBuffer(1024, 1024);
				if (buf.Array == null)
				{
					throw new WebSocketException("Buffer array is null!");
				}
				WebSocketReceiveResult r;
				var jsonBuild = new StringBuilder();
				do{
					r = await cws.ReceiveAsync(buf, CancellationToken.None);
					jsonBuild.Append(Encoding.UTF8.GetString(buf.Array, buf.Offset, r.Count));
				} while (!r.EndOfMessage);
				var jsonResult = jsonBuild.ToString();
				if (String.IsNullOrEmpty(jsonResult)) return;
				JObject jsonObj = new JObject();
				try{
					jsonObj = JObject.Parse(jsonResult);
				}
				catch (JsonReaderException e){
					throw new ApplicationException(e.Message);
				}

				string subType = (string) jsonObj["type"];
				switch (subType){
					case "connection_ack":
					{
						Debug.Log("init_success, the handshake is complete");
						OnSubscriptionHandshakeComplete subscriptionHandshakeComplete =
							new OnSubscriptionHandshakeComplete();
						subscriptionHandshakeComplete.FireEvent();
						continue;
					}
					case "error":
					{
						throw new ApplicationException("The handshake failed. Error: " + jsonResult);
					}
					case "connection_error":
					{
						throw new ApplicationException("The handshake failed. Error: " + jsonResult);
					}
					case "next":
					{
						OnSubscriptionDataReceived subscriptionDataReceived = new OnSubscriptionDataReceived(jsonResult);
						subscriptionDataReceived.FireEvent();
						continue;
					}
					case "ka":
					{
						continue;
					}
					case "subscription_fail":
					{
						throw new ApplicationException("The subscription data failed");
					}
					case "ping":
					{
						await cws.SendAsync(
							new ArraySegment<byte>(Encoding.UTF8.GetBytes($@"{{""type"":""pong""}}")),
							WebSocketMessageType.Text,
							true,
							CancellationToken.None
						);
						continue;
					}
				
				}
				
				break;
			}

			
		}

		public static async Task WebsocketDisconnect(ClientWebSocket cws, string socketId = "1"){
			string jsonData = $"{{\"type\":\"stop\",\"id\":\"{socketId}\"}}";
			ArraySegment<byte> b = new ArraySegment<byte>(Encoding.ASCII.GetBytes(jsonData));
			await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
			await cws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
			OnSubscriptionCanceled subscriptionCanceled = new OnSubscriptionCanceled();
			subscriptionCanceled.FireEvent();
		}
		
		#endregion

		#region Utility

		public static string FormatJson(string json)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

		#endregion
	}
}
