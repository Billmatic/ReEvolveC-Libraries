using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

public class CRMServerConfig
{
    #region Public Fields

    public String ServerAddress;
    public String OrganizationName;
    public String DomainUriSuffix;

    public Uri DiscoveryUri
    {
        get
        {
            if (!String.IsNullOrEmpty(ServerAddress))
            {
                //return new Uri(String.Format("https://{0}/XRMServices/2011/Discovery.svc", ServerAddress));
                return new Uri(String.Format("http://{0}/XRMServices/2011/Discovery.svc", ServerAddress));
            }

            return null;
        }
    }

    public Uri OrganizationUri
    {
        get
        {
            if (!String.IsNullOrEmpty(ServerAddress) && !String.IsNullOrEmpty(OrganizationName))
            {
                //return new System.Uri("https://" + ServerAddress + "/" + OrganizationName + "/XRMServices/2011/Organization.svc");
                return new System.Uri("http://" + ServerAddress + "/" + OrganizationName + "/XRMServices/2011/Organization.svc");
            }

            return null;
        }
    }

    public Uri HomeRealmUri = null;
    public ClientCredentials DeviceCredentials = null;
    public ClientCredentials Credentials = null;

    public virtual AuthenticationProviderType EndpointType
    {
        get
        {
            if (DiscoveryUri != null)
            {
                return ServiceConfigurationFactory.CreateConfiguration<IDiscoveryService>(DiscoveryUri).AuthenticationType;
            }

            return AuthenticationProviderType.ActiveDirectory;
        }
    }

    public CRMServerConfig() { }

    #endregion
}
