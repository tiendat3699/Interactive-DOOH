using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GadGame.Singleton;
using Newtonsoft.Json;
using UnityEngine;

namespace GadGame.Network
{
    public class Receiver : PersistentSingleton<Receiver>
    {
        [SerializeField] private int _port = 3000;
        [SerializeField] private ReceiverData _dataReceived;

        private bool _receiving = true;
        public ReceiverData DataReceived => _dataReceived;

        private Thread _receiveThread;
        private TcpListener _listener;


        private void Start()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                _receiving = true;
                Debug.Log("Listening for data from Python...");
                // Start a new thread to handle incoming data
                Thread receiveThread = new Thread(GetReceiveData)
                {
                    IsBackground = true
                };
                receiveThread.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("Error starting listener: " + e.Message);
            }
        }

        private void GetReceiveData()
        {
            while (_receiving)
            {
                try
                {
                    var client = _listener.AcceptTcpClient();
                    var stream = client.GetStream();
                    var buffer = new byte[1024];
                    var bytesRead = stream.Read(buffer, 0, buffer.Length);
                    var jsonData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    _dataReceived = JsonConvert.DeserializeObject<ReceiverData>(jsonData, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }
            }
        }

        void OnDestroy()
        {
            _receiving = false;
            if (_listener != null)
            {
                _listener.Stop();
            }
        }
    }
}