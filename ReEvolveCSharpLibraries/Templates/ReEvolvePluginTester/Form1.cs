using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Description;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using ReEvolvePluginTester.AbstractClasses;
using Microsoft.Xrm.Sdk.Query;
//using [plugindll]

namespace ReEvolvePluginTester
{
    public partial class Form1 : Form
    {
        private OrganizationServiceProxy _serviceProxy = null;
        private CRMServerConfig _crmServerConfig = null;

        private const string SERVER_ADDRESS = "fc-th-crm-appd";
        private const string ORGANIZATION_NAME = "FCRDEV";
        //private const string SERVER_ADDRESS = "fc-th-crm-app1";
        //private const string ORGANIZATION_NAME = "FCRPROD";
        //private const string SERVER_ADDRESS = "fc-th-crm-appt";
        //private const string ORGANIZATION_NAME = "FCRTEST";

        private const string PLUGIN_ASSEMBLY_PREFIX = "HSL.FCR.Plugins";

        public Form1()
        {
            InitializeComponent();
        }

        private ClientCredentials GetCredentials()
        {
            ClientCredentials clientCredentials = new ClientCredentials();

            clientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("Traian.Nitoi", "Donkey1!", "FIRSTCAPITAL");
            //clientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("Administrator", "pass@word1", "contoso");

            return clientCredentials;
        }

        private void LoadPluginAssemblies()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var files = Directory.GetFiles(path, PLUGIN_ASSEMBLY_PREFIX + "*.dll");
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var file in files)
            {
                var assemblyName = AssemblyName.GetAssemblyName(file);
                if (!loadedAssemblies.Any(a => a.FullName.Equals(assemblyName.FullName, StringComparison.OrdinalIgnoreCase)))
                {
                    AppDomain.CurrentDomain.Load(assemblyName);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load plugins.
            this.LoadPluginAssemblies();

            IList<ListItem> plugins = new List<ListItem>();
            plugins.Add(new ListItem(null, ""));

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(PLUGIN_ASSEMBLY_PREFIX, StringComparison.OrdinalIgnoreCase));
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Plugin)));
                foreach (var type in types)
                {
                    //plugins.Add(new ListItem(type, type.FullName.Replace(PLUGIN_ASSEMBLY_PREFIX + ".", string.Empty)));
                    plugins.Add(new ListItem(type, type.Name));
                }
            }

            ddlEntity.DataSource = plugins.OrderBy(en => en.Text).ToList();
            ddlEntity.DisplayMember = "Text";
            ddlEntity.ValueMember = "Value";
        }

        private class ListItem
        {
            public Type Value { set; get; }
            public string Text { set; get; }

            public ListItem(Type value, string text)
            {
                Value = value;
                Text = text;
            }
        }

        private void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEntity.SelectedItem != null && ((ListItem)ddlEntity.SelectedItem).Value != null)
            {
                IList<ListItem> handlers = new List<ListItem>();
                handlers.Add(new ListItem(null, ""));

                Type pluginType = ((ListItem)ddlEntity.SelectedItem).Value;
                var plugin = (Plugin)Activator.CreateInstance(pluginType);
                var handlerDescriptors = plugin.GetRegisteredEventHandlers();
                foreach (var descriptor in handlerDescriptors)
                {
                    var handlerType = descriptor.Handler.Method.GetGenericArguments()[0];
                    handlers.Add(new ListItem(handlerType, handlerType.Name));
                }

                ddlPlugin.DataSource = handlers.OrderBy(p => p.Text).ToList();
                ddlPlugin.DisplayMember = "Text";
                ddlPlugin.ValueMember = "Value";
                ddlPlugin.Enabled = true;
            }
            else
            {
                ddlPlugin.DataSource = null;
                txtRecordId.Text = "";
                ddlPlugin.Enabled = false;
                txtRecordId.Enabled = false;
                btnExecute.Enabled = false;
            }
        }

        private void ddlPlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPlugin.SelectedItem != null && ((ListItem)ddlPlugin.SelectedItem).Value != null)
            {
                txtRecordId.Enabled = true;
                btnExecute.Enabled = true;
            }
            else
            {
                txtRecordId.Text = "";
                txtRecordId.Enabled = false;
                btnExecute.Enabled = false;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlPlugin.SelectedItem == null || ((ListItem)ddlPlugin.SelectedItem).Value == null || string.IsNullOrEmpty(txtRecordId.Text))
                {
                    MessageBox.Show("Missing required field(s)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Prepare CRM server config.
                _crmServerConfig = new CRMServerConfig();

                _crmServerConfig.ServerAddress = SERVER_ADDRESS;
                _crmServerConfig.OrganizationName = ORGANIZATION_NAME;

                _crmServerConfig.Credentials = GetCredentials();

                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = new OrganizationServiceProxy(_crmServerConfig.OrganizationUri,
                                                                    _crmServerConfig.HomeRealmUri,
                                                                    _crmServerConfig.Credentials,
                                                                    _crmServerConfig.DeviceCredentials))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior(Assembly.GetExecutingAssembly()));

                    IOrganizationService service = (IOrganizationService)_serviceProxy;

                    var handlerType = ((ListItem)ddlPlugin.SelectedItem).Value;
                    var handler = (PluginEventHandler)Activator.CreateInstance(handlerType);
                    handler.Initialize(null, service);
                    handler.Run(new Guid(txtRecordId.Text));
                }

                MessageBox.Show("Plugin execution completed", "Sucessful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PluginTester Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
