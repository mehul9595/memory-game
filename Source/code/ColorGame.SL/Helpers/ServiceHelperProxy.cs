using ColorGame.SL.GameScoreService;

namespace ColorGame.SL.Helpers
{
    public class ServiceHelperProxy
    {
        private static ScoreServiceClient _scoreServiceClient;

        public static ScoreServiceClient GameScoreServiceClient
        {
            get { return _scoreServiceClient ?? (_scoreServiceClient = new ScoreServiceClient()); }
        }
    }
}