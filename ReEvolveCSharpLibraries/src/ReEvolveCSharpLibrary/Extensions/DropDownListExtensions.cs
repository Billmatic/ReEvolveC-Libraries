using System;
using System.Web.UI.WebControls;

namespace ReEvolveCSharpLibrary.Extensions
{
    /// <summary>
    /// This method sets the enums to a dropdownlist
    /// </summary>
    public static class DropDownListExtensions
    {
        public static void SetDropDownListItemsEnums(this DropDownList dropDownList, Type enumType)
        {
            string[] enumStatus = Enum.GetNames(enumType);

            foreach (string item in enumStatus)
            {
                int value = (int)Enum.Parse(enumType, item);
                ListItem listItem = new ListItem(item, value.ToString());
                dropDownList.Items.Add(listItem);
            }
        }
    }
}
