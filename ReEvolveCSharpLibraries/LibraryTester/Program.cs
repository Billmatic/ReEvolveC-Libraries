using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using ReEvolveCSharpLibrary.Extensions;
using ReEvolverCRMLibrary.Helpers;
using Microsoft.Xrm.Sdk.Query;
using ReEvolverCRMLibrary.Extensions;

namespace LibraryTester
{
    class Program
    {
        static void Main(string[] args)
        {
            CRMConnectionHelper crmConnectionHelper = new CRMConnectionHelper();

            IOrganizationService _service = crmConnectionHelper.GetCRMConnection("https://factor.api.crm.dynamics.com/XRMServices/2011/Organization.svc", "ideacateam@factor.ca","ideaca4me!");

            QueryExpression selectAllApplicationComponents = new QueryExpression() { EntityName= "factor_applicationcomponent", ColumnSet = new ColumnSet(true)};
            EntityCollection collection = _service.RetrieveMultiple(selectAllApplicationComponents);
            EntityCollection collectionAll = _service.RetrieveMultipleAll(selectAllApplicationComponents);


        }
    }
}
