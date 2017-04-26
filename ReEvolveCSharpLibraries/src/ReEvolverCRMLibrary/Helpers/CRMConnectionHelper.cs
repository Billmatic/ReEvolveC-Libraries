using System;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;

namespace ReEvolverCRMLibrary.Helpers
{
    public class CRMConnectionHelper
    {

        /// <summary>
        /// The Class Constructor this requires a class the organization url, username and
        /// password.  
        /// </summary>
        /// <param name="organizationUri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public CRMConnectionHelper()
        {

        }


        /// <summary>
        /// Get the IOrganization service for the CRM connection.  This function need the organization url
        /// the user name and the user password to connect.
        /// </summary>
        /// <param name="organizationUri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IOrganizationService GetCRMConnection(string organizationUri, string userName, string password)
        {
            Uri OrganizationUri;
            ClientCredentials DeviceCrednetials;
            ClientCredentials Credentials;
            OrganizationServiceProxy _serviceProxy;
            IOrganizationService _service;
            DeviceCrednetials = null;
            Credentials = new ClientCredentials();

            try
            {
                OrganizationUri = new Uri(organizationUri);
                Credentials.UserName.UserName = userName;
                Credentials.UserName.Password = password;
                _serviceProxy = new Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy(OrganizationUri, null, Credentials, DeviceCrednetials);
                _serviceProxy.EnableProxyTypes();
                _service = (Microsoft.Xrm.Sdk.IOrganizationService)_serviceProxy;
                return _service;
            }
            catch (Exception ex)
            {
                string exeption = ex.Message;
                return null;
            }
        }
    }
}
