using System;
using System.Drawing;
using System.Reflection;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    public class ruleItemInfo
    {
        public ruleItemType itemType;
        public Type ruleItemBaseType;
        public string pythonFileName;
        public string pythonCategory;

        public ruleItemInfo(Type type)
        {
            itemType = ruleItemType.RuleItem;
            ruleItemBaseType = type;
        }

        public ruleItemInfo()
        {
        }

        /// <summary>
        /// Gets the Items image for drag and drop.
        /// </summary>
        /// <returns>an bitmap rendering of the rule item</returns>
        public Image getItemImage()
        {
            ConstructorInfo constr = ruleItemBaseType.GetConstructor(new Type[0]);
            ruleItemBase newRuleItem = (ruleItemBase)constr.Invoke(new object[0] { });
            newRuleItem.initPins();
            ctlRuleItemWidget widget = new ctlRuleItemWidget(newRuleItem, @this => {});
            Size sz = newRuleItem.preferredSize();
            widget.SetBounds(0, 0, sz.Width, sz.Height);
            Bitmap image = new Bitmap(sz.Width,sz.Height);
          
            widget.DrawToBitmap(image, widget.ClientRectangle);
            return image;
        }
    }

    public enum ruleItemType
    {
        RuleItem, scriptFile
    }
}
