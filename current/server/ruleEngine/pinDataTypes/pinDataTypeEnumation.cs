namespace ruleEngine.pinDataTypes
{
    using System.ComponentModel.DataAnnotations;

    public enum pinDataTypeEnumeration
    {
        [Display(Name = "Numeric")]
        pinDataNumber,
        [Display(Name = "Text")]
        pinDataString,
        [Display(Name = "Boolean")]
        pinDataBool,
        pinDataObject,
    }

}