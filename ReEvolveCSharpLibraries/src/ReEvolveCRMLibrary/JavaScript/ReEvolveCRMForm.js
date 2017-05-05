/**
* concatenates and sets the text field from other fields on the form
* @param {string} updatedfield the crm field name to be updated
* @param {string} dependentFields: comma seperated string that replaces crm form attributes with 
* the controls content.  Lookup, text and optionset are supported.
* @param {int} nameLength:  This limits the name to the lenght give.  Should be set the 
* target fields maximum length.
* and text fields
*/
function UpdateTextFromFields(updatefield, dependentFields, nameLength)
{
    var name = '';
    var dependentFieldsArry = dependentFields.split(',');

    var arrayLength = dependentFieldsArry.length;
    debugger;
    for (var i = 0; i < arrayLength; i++) {
        if (Xrm.Page.getAttribute(dependentFieldsArry[i]) != null) {
            var dependentValue = Xrm.Page.getAttribute(dependentFieldsArry[i]).getValue();
            if (dependentValue != null && typeof dependentValue === 'string') {
                name += ' ' + dependentValue;
            }
            else if (dependentValue != null && typeof dependentValue === 'object') {
                var dependentName = ' ' + dependentValue[0].name.trim();
                name += dependentName;
            }
            else if (dependentValue != null && typeof dependentValue === 'number') {
                var dependentName = ' ' + Xrm.Page.getAttribute(dependentFieldsArry[i]).getOption(dependentValue).text;
                name += dependentName;
            }
        }
        else
        {
            name += ' '+dependentFieldsArry[i];
        }

    }

    if (name.length >= nameLength) {
        name = name.substring(1, nameLength - 1).trim();
    }

    Xrm.Page.getAttribute(updatefield).setValue(name);
};