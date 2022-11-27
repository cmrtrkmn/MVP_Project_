using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP_Project.Models
{
    public class BasketballPlayer
    {
        public string PlayerName { get; set; }
        public string NickName { get; set; }
        public int Number { get; set; }
        public string TeamName { get; set; }
        public string position { get; set; }
        public int ScoredPoint { get; set; }
        public int Rebound { get; set; }
        public int Assist { get; set; }
        public int katSayiScoredPoint { get; set; }
        public int katSayiRebound { get; set; }
        public int katSayiAssist { get; set; }
        public int Rating_Points { get; set; }
    }
}
