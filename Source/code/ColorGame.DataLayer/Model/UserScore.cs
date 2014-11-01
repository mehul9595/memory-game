using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace ColorGame.DataLayer.Model
{
    /// <summary>
    ///     This class represents data structure to store Users Score
    /// </summary>
    public class UserScore
    {
        #region Properties

        public int Rank { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }

        #endregion

        #region Method

        /// <summary>
        ///     This method is use to save the score of current user to database
        /// </summary>
        /// <param name="userScore">Current User Score</param>
        public static void Save(UserScore userScore)
        {
            MySqlConnection connection = null;
            try
            {
                connection =
                    new MySqlConnection(DatabaseHelper.ConnectionString);

                string sqlquery = "Insert INTO UserScore values(@id,@name,@email,@score)";
                var cmd = new MySqlCommand(sqlquery, connection);

                cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@name", userScore.Name);
                cmd.Parameters.AddWithValue("@email", userScore.Email);
                cmd.Parameters.AddWithValue("@score", userScore.Score);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        /// <summary>
        ///     This method gets the UserScores with their Ranking
        /// </summary>
        /// <param name="currentUserScore">Current User Score</param>
        /// <returns></returns>
        public static UserScore[] GetTopRankersWithCurrentUser(UserScore currentUserScore)
        {
            MySqlConnection connection = null;
            try
            {
                connection =
                    new MySqlConnection(DatabaseHelper.ConnectionString);

                string sqlquery = "select  * from UserScore";
                var cmd = new MySqlCommand(sqlquery, connection);
                connection.Open();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                var userScores = new List<UserScore>();
                while (dataReader.HasRows && dataReader.Read())
                {
                    var userScore = new UserScore();
                    userScore.Name = dataReader[1].ToString();
                    userScore.Email = dataReader[2].ToString();
                    userScore.Score = int.Parse(dataReader[3].ToString());
                    userScores.Add(userScore);
                }

                // if there are more than 5 records in DB we want to add the current user to the list so that we get the top 5 list sorted based on score/rank.
                if (userScores.Count >= 5)
                    userScores.Add(new UserScore
                    {
                        Email = currentUserScore.Email,
                        Name = currentUserScore.Name,
                        Score = currentUserScore.Score
                    });
                userScores = userScores.OrderByDescending(r => r.Score).ToList();
                int rankCounter = 1;
                userScores.ForEach(r => r.Rank = rankCounter++);

                List<UserScore> filteredRankInfo = userScores.Take(5).ToList();

                /*Check if current user exists in the filtered list of top 5 rankers, and if user exists multiple times [ since we added it to unsorted list ] , 
                 * remove it from list. If user doesnot exist to the list [ that means he is not in top 5 ] , add to the list .
                 */
                int currentUserInListCount =
                    filteredRankInfo.Count(r => r.Name == currentUserScore.Name && r.Score == currentUserScore.Score &&
                                                r.Email == currentUserScore.Email);
                if (currentUserInListCount == 0)
                {
                    UserScore rankInfoData =
                        userScores.FirstOrDefault(r => r.Name == currentUserScore.Name && r.Score == currentUserScore.Score &&
                                                       r.Email == currentUserScore.Email);
                    if (rankInfoData != null)
                        filteredRankInfo.Add(rankInfoData);
                }
                if (currentUserInListCount == 2)
                {
                    UserScore rankInfoData =
                        userScores.FirstOrDefault(r => r.Name == currentUserScore.Name && r.Score == currentUserScore.Score &&
                                                       r.Email == currentUserScore.Email);
                    if (rankInfoData != null)
                        filteredRankInfo.Remove(rankInfoData);
                }
                return filteredRankInfo.ToArray();
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        #endregion
    }
}