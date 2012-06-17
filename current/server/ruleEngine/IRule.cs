

namespace ruleEngine
{
    using System.Collections.Generic;
    using ruleEngine.ruleItems;

    public interface IRule
    {
        ruleState? state { get; set; }
        string name { get; set; }
        string details { get; set; }
        void start();
        
        void stop();
        
        void saveToDisk(string file);

        IEnumerable<ruleItemBase> getRuleItems();

        void changeName(string name);

        void advanceDelta();

        int preferredHeight { get; set; }

        int preferredWidth { get; set; }
    }
}
