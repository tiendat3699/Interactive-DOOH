using GadGame.Manager;
using GadGame.SO;
using GadGame.State;
using GadGame.State.MainFlowState;
using GadGame.Network;
using UnityEngine;
using System.Net.NetworkInformation;
using GadGame.Event.Type;

namespace GadGame
{
    public class MainFlow : StateRunner<MainFlow>
    {
        public SceneFlowConfig SceneFlowConfig;
        public VoidEvent ScanSuccess;
        public BoolEvent PlayPassByAnim;
        public BoolEvent PlayVideo;
        public FloatEvent ReadyCountDown;

        protected override async void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            string _macAddress = GetMacAddressString();
            Debug.Log(_macAddress);
            await P4PGraphqlManager.Instance.LoginMachine(_macAddress);
        }

        private  async void Start()
        {
            await LoadSceneManager.Instance.LoadSceneWithTransitionAsync(SceneFlowConfig.PassByScene.ScenePath);
            SetState<IdleState>();
        }
        
        private string GetMacAddressString()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // Filter out loopback and virtual interfaces
                if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Wireless80211)
                    continue;

                PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
                byte[] bytes = physicalAddress.GetAddressBytes();
                string macAddress = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    macAddress += bytes[i].ToString("X2");
                }
                return macAddress;
            }
            return "";
        }
    }
}