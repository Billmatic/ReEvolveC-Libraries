using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ReEvolveCRMLibrary.Extensions
{
    public static class IOrganizationServceExtension
    {
        public static EntityCollection RetrieveMultipleAll(this IOrganizationService _service, QueryExpression queryExpression)
        {
            EntityCollection pagedEntityCollection = new EntityCollection();
            EntityCollection resultEntityCollection = new EntityCollection();

            queryExpression.PageInfo = new PagingInfo();
            queryExpression.PageInfo.Count = 4999;
            queryExpression.PageInfo.PageNumber = 1;
            queryExpression.PageInfo.PagingCookie = null;

            while (true)
            {
                pagedEntityCollection = _service.RetrieveMultiple(queryExpression);

                foreach (Entity entity in pagedEntityCollection.Entities)
                {
                    resultEntityCollection.Entities.Add(entity);
                }

                // Increment the page number to retrieve the next page.
                queryExpression.PageInfo.PageNumber++;

                // Set the paging cookie to the paging cookie returned from current results.
                queryExpression.PageInfo.PagingCookie = pagedEntityCollection.PagingCookie;

                if (pagedEntityCollection.MoreRecords)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return resultEntityCollection;
        }
    }
}
