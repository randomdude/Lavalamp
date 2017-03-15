using System.Collections.Generic;

using ruleEngine;
using ruleEngine.ruleItems;

namespace lavalamp
{
    using System.Drawing;
    using System.Linq;

    using ServiceStack.ServiceClient.Web;
    using ServiceStack.ServiceHost;

    interface ILavalampInfoBase
    {
        ruleState? state { get; set; }
        string name { get; set; }
    }
    
    [RestService("/rule","POST, PUT, DELETE, GET")]
    [RestService("/rule/{name}")]
    public class lavalampRuleInfo : ILavalampInfoBase, IRule
    {
        public ruleState? state { get; set; }
        public string name { get; set; }

        public string newName { get; set; }

        public string details { get; set; }

        /// <summary>
        /// if fetchRuleItems is true this list will be or has been filled.
        /// </summary>
        
        public List<lavalampRuleItemInfo> ruleItems { get; set; }

        public void start()
        {
            JsonServiceClient client = new JsonServiceClient();
            state = ruleState.running;
            client.Put<lavalampRuleInfo>("/rule", this);
        }

        public void stop()
        {
            JsonServiceClient client = new JsonServiceClient();
            state = ruleState.stopped;
            client.Put<lavalampRuleInfo>("/rule", this);
        }

        public void saveToDisk(string name)
        {
            JsonServiceClient client = new JsonServiceClient();
            state = ruleState.running;
            client.Post<lavalampRuleInfo>("/rule", this);
        }

        public IEnumerable<IRuleItem> getRuleItems()
        {
            if(this.ruleItems == null)
                this.ruleItems = new List<lavalampRuleItemInfo>();
            return this.ruleItems.Select(r => r as IRuleItem);
        }

        public void changeName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void advanceDelta()
        {
            throw new System.NotImplementedException();
        }

        public int preferredHeight
        {
            get;
            set;
        }

        public int preferredWidth
        {
            get; set;
        }
    }


    public class lavalampRuleItemInfo : ILavalampInfoBase, IRuleItem
    {
        public lavalampRuleItemInfo()
        {
            position = new int[2];
        }
        public ruleState? state { get; set; }
        public string name { get; set; }
        public string _category { get; set; }
        public string caption { get; set; }

        public string background { get; set; }

        public IFormOptions opts { get; set; }
        public int[] position { get; set; } 
        public string guid { get; set; }

        public string category()
        {
            return this._category ?? "";
        }

        public string ruleName()
        {
            return name ?? "(unknown rule)";
        }

        string IRuleItem.caption()
        {
            return caption ?? this.ruleName();
        }

        public IFormOptions setupOptions()
        {
            return opts;
        }

        public Point location
        {
            get
            {
                return new Point(position[0],position[1]);
            }
            set
            {
                position = new int[2];
                position[0] = value.X;
                position[1] = value.Y;
            }
        }

        public Dictionary<string, pin> pinInfo
        {
            get; set; 
        }
    }


}
