namespace ruleEngine.ruleItems
{
    using System;

    public class BaseOptions : IFormOptions
    {
        public virtual string displayName
        { 
            get
            {
                return "Options...";
            } 
        }

        public virtual string typedName 
        { 
            get
            {
                return "Base";
            } 
        }
        

        public void setChanged()
        {
            if (this.optionsChanged != null)
                this.optionsChanged.Invoke(this,null);
        }

        public event EventHandler optionsChanged;
    }
}