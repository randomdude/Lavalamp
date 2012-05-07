using System.ServiceModel;

namespace ruleService
{
    [ServiceContract]
    public interface IRuleService
    {
        [OperationContract]
        string GetData(int value);
    }
}
