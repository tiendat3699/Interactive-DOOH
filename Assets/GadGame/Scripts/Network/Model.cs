using System;

namespace GadGame.Network
{
    struct User
    {
        public string id;
    }
    
    struct LoginDetails
    {
        public string accessToken;
        public string refreshToken;
        public User user;
    }

    struct Participant
    {
        public string id;
        public int totalScore;
        public User player;
        public int myRanke;
    }

    struct GameSession
    {
        public string id;
        public DateTime startAt;
        public DateTime endAt;
        public int score;
        public Participant participant;
    }

    struct Guest
    {
        public string id;
        public string firstName;
        public string lastName;
        public string phone;
        public string email;
    }
}