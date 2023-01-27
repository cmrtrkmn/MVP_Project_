using MVP_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP_Project.Controllers
{
    interface ISport
    {
        int Calculator(string position,int scoredPoint,int rebound,int assist);
    }
    class Basketball : ISport
    {
        public int Calculator(string position, int scoredPoint, int rebound, int assist)
        {
            int katSayiScoredPoint = 0;
            int katSayiRebound = 0;
            int katSayiAssist = 0;
            int rating_Points = 0;

            if (position == "G")
            {
                katSayiScoredPoint = 2;
                katSayiRebound = 3;
                katSayiAssist = 1;
            }
            if (position == "F")
            {
                katSayiScoredPoint = 2;
                katSayiRebound = 2;
                katSayiAssist = 2;
            }
            if (position == "C")
            {
                katSayiScoredPoint = 2;
                katSayiRebound = 1;
                katSayiAssist = 3;
            }
            rating_Points = scoredPoint * katSayiScoredPoint + rebound * katSayiRebound + assist * katSayiAssist;
            return rating_Points;
        }

    }
    class Handball : ISport
    {
        public int Calculator(string position, int initialRatingPoints, int goalMade, int goalRecieved)
        {
            int factorGoalMade = 0;
            int factorGoalRecieved = 0;
            int rating_Points = 0;

            if (position == "G")
            {
                factorGoalMade = 5;
                factorGoalRecieved = -2;
            }
            if (position == "F")
            {
                factorGoalMade = 1;
                factorGoalRecieved = -1;
            }


            rating_Points = initialRatingPoints + goalMade * factorGoalMade + goalRecieved * factorGoalRecieved;
            return rating_Points;
        }

    }
}
