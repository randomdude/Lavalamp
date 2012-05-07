using System.Collections.Generic;
using System.ServiceModel;
using ruleEngine.ruleItems;

namespace lavalamp
{
    [ServiceContract]
    public interface IRule
    {
        [OperationContract]
        void start();
        
        [OperationContract]
        void stop();
        
        [OperationContract]
        void saveToDisk(string file);

        [OperationContract]
        IEnumerable<ruleItemBase> getRuleItems();
    }
}
