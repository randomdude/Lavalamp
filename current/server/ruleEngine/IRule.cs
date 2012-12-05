

namespace ruleEngine
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ruleContract))]
    public interface IRule
    {
        ruleState? state { get; set; }
        string name { get; set; }
        string details { get; set; }
        void start();
        
        void stop();
        
        void saveToDisk(string file);

        IEnumerable<IRuleItem> getRuleItems();

        void changeName(string name);

        void advanceDelta();

        int preferredHeight { get; set; }

        int preferredWidth { get; set; }
    }

    [ContractClassFor(typeof(IRule))]
    internal abstract class ruleContract : IRule
    {
        public ruleState? state
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public string name
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public string details
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public void start()
        {
            throw new System.NotImplementedException();
        }

        public void stop()
        {
            throw new System.NotImplementedException();
        }

        public void saveToDisk(string file)
        {
            throw new System.NotImplementedException();
        }

        [ContractInvariantMethod]
        public IEnumerable<IRuleItem> getRuleItems()
        {
            Contract.Requires(Contract.Result<IEnumerable<IRuleItem>>() != null);
            throw new System.NotImplementedException();
        }

        public void changeName(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(this.name == name || this.name == Contract.OldValue(this.name));
            throw new System.NotImplementedException();
        }

        public void advanceDelta()
        {
            throw new System.NotImplementedException();
        }

        public int preferredHeight
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public int preferredWidth
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
