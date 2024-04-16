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
    public class UdpSocket : PersistentSingleton<UdpSocket>
    {
        [SerializeField] private string _ip = "127.0.0.1";
        [SerializeField] private int _receivePort = 8000;
        [SerializeField] private int _sendPort = 8001;
        [SerializeField] private ReceiverData _dataReceived;

        private bool _receiving = true;
        public ReceiverData DataReceived => _dataReceived;

        private Thread _receiveThread;
        private UdpClient _client;
        private IPEndPoint _remoteEndPoint;


        private void Start()
        {
            // Create remote endpoint
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _sendPort);

            // Create local client
            _client = new UdpClient(_receivePort);

            // local endpoint define (where messages are received)
            // Create a new thread for reception of incoming messages
            _receiveThread = new Thread(new ThreadStart(ReceiveData))
            {
                IsBackground = true
            };
            _receiveThread.Start();
            _receiving = true;
            // Initialize (seen in comments window)
            Debug.Log("UDP Comms Initialised");
        }

        private void ReceiveData()
        {
            while (_receiving)
            {
                try
                {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = _client.Receive(ref anyIP);
                    string jsonData = Encoding.UTF8.GetString(data);
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

        public void SendDataToPython(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                _client.Send(data, data.Length, _remoteEndPoint);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }

        void OnDestroy()
        {
            _receiving = false;
            if (_receiveThread != null)
                _receiveThread.Abort();

            _client.Close();
        }
    }
}