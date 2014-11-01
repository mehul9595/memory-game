using System.ServiceModel;
using ColorGame.DataLayer.Model;

namespace GameService.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IScoreService" in both code and config file together.

    /// <summary>
    ///     This is the service contract which maintains User Score
    /// </summary>
    [ServiceContract]
    public interface IScoreService
    {
        /// <summary>
        ///     Saves user's score.
        /// </summary>
        /// <param name="userScore">The score information.</param>
        [OperationContract]
        void Save(UserScore userScore);

        /// <summary>
        ///     Gets the top rankers with current user ranking position.
        /// </summary>
        /// <param name="userScore">The current score information.</param>
        /// <returns>An array of UserScore rankings.</returns>
        [OperationContract]
        UserScore[] GetTopRankersWithCurrentUser(UserScore userScore);
    }
}