namespace ruleService
{
    public class ruleService : IRuleService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
    }
}
