using System;

namespace GadGame.Network
{
    public struct User
    {
        public string id;
    }
    
    public struct LoginDetails
    {
        public string accessToken;
        public string refreshToken;
        public User user;
    }

    public struct Participant
    {
        public string id;
        public int totalScore;
        public User player;
        public int myRanke;
    }

    public struct GameSession
    {
        public string id;
        public DateTime startAt;
        public DateTime endAt;
        public int score;
        public Participant participant;
    }

    public struct Guest
    {
        public string id;
        public string firstName;
        public string lastName;
        public string phone;
        public string email;
    }
}