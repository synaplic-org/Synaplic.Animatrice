using Synaplic.Inventory.Infrastructure.Sage.Configurations;
using Synaplic.Inventory.Shared.Extensions;
using Synaplic.Inventory.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Infrastructure.Sage.Manager
{
    public interface ISageQueryManager
    {
        string Version { get; set; }

        string GetQuery(string pName);
    }

    public class SageQueryManager : ISageQueryManager
    {

        private List<SageQuery> _ListQueries;

        public string Version { get; set; }

        public SageQueryManager()
        {
            Load();
        }

        public void Load()
        {
            _ListQueries = new List<SageQuery>();
            var olistResources = ReadResourceListe();
            foreach (var resource in olistResources)
            {
                var oparts = resource.Split(".");

                var oSqlQuery = new SageQuery()
                {
                    Path = resource,
                    Version = oparts[0],
                    Domain = oparts[1],
                    Name = oparts[2].ToUpper()
                };
                oSqlQuery.Query = ReadResource(oSqlQuery.Path);

                _ListQueries.Add(oSqlQuery);
            }
        }




        public string ReadResource(string path)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = path;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            if (!path.StartsWith("Uni.Sage."))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(path));
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public List<string> ReadResourceListe()
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            //var lisResources = assembly.GetManifestResourceNames();
            return assembly.GetManifestResourceNames().Where(o => o.Contains("Sage100") && o.EndsWith(".Sql")).Select(o => o.Substring(o.IndexOf("Sage100"))).ToList();
        }

        public string GetQuery(string pName)
        {
            if (_ListQueries.IsNullOrEmpty()) return "";
            var oSageQuery = _ListQueries.Where(o => o.Version == Version && o.Name == pName).FirstOrDefault();
            if (oSageQuery == null) oSageQuery = _ListQueries.Where(o => o.Version == "Sage100" && o.Name == pName).FirstOrDefault();
            return oSageQuery.Query;
        }
    }




}
