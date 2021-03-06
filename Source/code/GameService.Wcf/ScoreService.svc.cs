﻿using System;
using ColorGame.DataLayer.Model;

namespace GameService.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ScoreService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ScoreService.svc or ScoreService.svc.cs at the Solution Explorer and start debugging.
    public class ScoreService : IScoreService
    {
        public void Save(UserScore userScore)
        {
            if (userScore == null)
            {
                throw new ArgumentNullException("userScore", "User score cannot be null");
            }

            UserScore.Save(userScore);
        }

        public UserScore[] GetTopRankersWithCurrentUser(UserScore userScore)
        {
            UserScore[] results = UserScore.GetTopRankersWithCurrentUser(userScore);
            return results;
        }
    }
}