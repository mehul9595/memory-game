using ColorGame.SL.GameScoreService;

namespace ColorGame.SL.Helpers
{
    /// <summary>
    ///     Singleton Score service proxy
    /// </summary>
    public class ServiceHelperProxy
    {
        private static ScoreServiceClient _scoreServiceClient;

        public static ScoreServiceClient GameScoreServiceClient
        {
            get { return _scoreServiceClient ?? (_scoreServiceClient = new ScoreServiceClient()); }
        }
    }
}