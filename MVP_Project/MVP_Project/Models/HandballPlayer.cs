using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP_Project.Models
{
    public class HandballPlayer
    {
        public string PlayerName { get; set; }
        public string NickName { get; set; }
        public int Number { get; set; }
        public string TeamName { get; set; }
        public string position { get; set; }
        public int InitialRatingPoints { get; set; }
        public int GoalMade { get; set; }
        public int GoalRecieved { get; set; }
        public int factorGoalMade { get; set; }
        public int factorGoalRecieved { get; set; }
        public int Rating_Points { get; set; }

    }
}
