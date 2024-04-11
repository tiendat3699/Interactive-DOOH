
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
        public bool Viewed;
        public bool Engage;
        public bool Ready;
        public Genders Gender;
        public int AgeMin;
        public int AgeMax;
    }
}