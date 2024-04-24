using UnityEngine;

namespace GadGame.Network
{
    public enum Genders
    {
        Male,
        Female,
    }
    
    [System.Serializable]
    public struct ReceiverData
    {
        public bool PassBy;
        public bool OnVision;
        public bool Engage;
        public bool Ready;
        public float Gender;
        public int AgeMin;
        public int AgeMax;
        public Vector2 PosPoint;
    }
}