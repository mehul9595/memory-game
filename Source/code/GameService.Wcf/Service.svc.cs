using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using ColorGame.DataLayer.Model;

namespace GameService.Wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }

        // Add more operations here and mark them with [OperationContract]

        [OperationContract]
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        [OperationContract]
        public void Save(UserScore userScore)
        {
            if (userScore == null)
            {
                throw new ArgumentNullException("userScore", "User score cannot be null");
            }

            UserScore.Save(userScore);

        }

        [OperationContract]
        public UserScore[] GetTopRankersWithCurrentUser(UserScore userScore)
        {
            var results = UserScore.GetTopRankersWithCurrentUser(userScore);
            return results;

        }
    }
}
