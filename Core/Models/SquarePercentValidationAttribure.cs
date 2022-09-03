using System.ComponentModel.DataAnnotations;
using Models;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
sealed public class OwnerDataValidationAttribure : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is OwnerData)
        {
            var oData = (OwnerData)value;
            return oData.PercentOfTheWholeHouse > 0 || oData.SquareOfPart > 0 && oData.SquareOfPart <= oData.FlatSquare;
        }
        else return true;
    }
}