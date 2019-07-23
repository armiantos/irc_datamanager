using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;
using TitaniumAS.Opc.Client.Da.Browsing;

namespace irc_datamanager.DataSourceWrappers
{
    public class OpcDaWrapper
    {
        private static OpcDaWrapper opcDaWrapper;
        private OpcDaServer currentServer;
        private OpcDaGroup mainGroup;

        // maps server name with Guid to connect to OPC servers 
        // that contain spaces
        private Dictionary<string, Guid> serverMapping;

        /// <summary>
        /// Browses through the opc tree recursively using the given browser.
        /// Adds item names to itemName collection (can be list or hashset).
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="itemNames"></param>
        /// <param name="browser"></param>
        private void BrowseChildren(string parent, ICollection<string> itemNames, OpcDaBrowser1 browser)
        {
            OpcDaBrowseElement[] elements = browser.GetElements(parent);
            foreach (OpcDaBrowseElement element in elements)
            {
                if (element.IsItem)
                    itemNames.Add(element.ItemId);
                if (!element.HasChildren)
                    continue;
                BrowseChildren(element.ItemId, itemNames, browser);
            }
        }

        /// <summary>
        /// Lists all available servers at current host.
        /// </summary>
        /// <param name="host">address of opc server host</param>
        /// <returns></returns>
        public List<string> ListServers(string host)
        {
            try
            {
                serverMapping.Clear();
                var serverDescs = new OpcServerEnumeratorAuto().Enumerate(host, OpcServerCategory.OpcDaServers);
                foreach (var serverDesc in serverDescs)
                {
                    serverMapping.Add(serverDesc.ProgId, serverDesc.CLSID);
                }
                return new List<string>(serverMapping.Keys);
            }
            catch
            {
                throw new Exception($"Access Denied. Unable to list OPC DA servers @ {host}");
            }
        }

        /// <summary>
        /// Retrieves all available item names in current server.
        /// </summary>
        /// <returns></returns>
        public List<string> ListItems()
        {
            OpcDaBrowser1 browser = new OpcDaBrowser1(currentServer);
            List<string> itemNames = new List<string>();
            BrowseChildren(null, itemNames, browser);
            return itemNames;
        }

        /// <summary>
        /// Updates items to be retrieved by OpcDa server. 
        /// Overrides previously included items.
        /// </summary>
        /// <param name="itemNames">List of items names to be included</param>
        public void UpdateIncludedItems(List<string> itemNames)
        {
            // clear server of any groups
            var tempGroupHolder = currentServer.Groups;
            for (int i = 0; i < tempGroupHolder.Count; i++)
            {
                currentServer.RemoveGroup(tempGroupHolder[i]);
            }

            // initialize default group for sync reading
            mainGroup = currentServer.AddGroup("mainGroup");
            mainGroup.IsActive = true;
            mainGroup.RemoveItems(mainGroup.Items);

            // include specified items
            OpcDaItemDefinition[] itemDefs = new OpcDaItemDefinition[itemNames.Count];
            for (int i = 0; i < itemNames.Count; i++)
            {
                OpcDaItemDefinition itemDef = new OpcDaItemDefinition { ItemId = itemNames[i], IsActive = true };
                itemDefs[i] = itemDef;
            }
            
            // evaluate results
            OpcDaItemResult[] results = mainGroup.AddItems(itemDefs);
            foreach (OpcDaItemResult result in results)
            {
                if (result.Error.Failed)
                    throw new Exception($"Failed to add item {result.Item.ItemId}");
            }
        }

        /// <summary>
        /// Gets the item values asynchronously. Items should be previously defined using
        /// UpdateIncludedItems().
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> GetItems()
        {
            Dictionary<string, object> items = new Dictionary<string, object>();
            OpcDaItemValue[] values =  await mainGroup.ReadAsync(mainGroup.Items);
            items.Add("Timestamp", values[0].Timestamp.UtcDateTime);
            foreach (OpcDaItemValue value in values)
            {
                string itemName = value.Item.ItemId.Replace('.', '_').Replace(" ", string.Empty)
                    .Replace("[", string.Empty).Replace(']', '_').Replace('-', '_')
                    .Replace(':', '_');
                if (!items.ContainsKey(itemName))
                {
                    items.Add(itemName, value.Value);
                }
            }
            return items;
        }

        /// <summary>
        /// Connects to specified opc server.
        /// </summary>
        /// <param name="serverName">name of opc server returned from ListServers()</param>
        /// <param name="host">address of opc server, should be consistent with host given
        /// to ListServers()</param>
        public void Connect(string serverName, string host)
        {
            if (currentServer != null && currentServer.IsConnected)
            {
                currentServer.Disconnect();
                currentServer = null;
            }
            if (serverMapping.TryGetValue(serverName, out Guid clsid))
            {
                currentServer = new OpcDaServer(clsid, host);
                currentServer.Connect();
            }
        }

        /// <summary>
        /// Singleton constructor. Only maintain one single opc connection.
        /// </summary>
        private OpcDaWrapper()
        {
            serverMapping = new Dictionary<string, Guid>();

        }

        /// <summary>
        /// Get singleton instance. 
        /// </summary>
        /// <returns></returns>
        public static OpcDaWrapper GetInstance()
        {
            if (opcDaWrapper == null)
                opcDaWrapper = new OpcDaWrapper();
            return opcDaWrapper;
        }
    }
}
