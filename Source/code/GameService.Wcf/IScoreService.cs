using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ColorGame.DataLayer.Model;

namespace GameService.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IScoreService" in both code and config file together.
    [ServiceContract]
    public interface IScoreService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        void Save(UserScore userScore);

        [OperationContract]
        UserScore[] GetTopRankersWithCurrentUser(UserScore userScore);
    }
}
