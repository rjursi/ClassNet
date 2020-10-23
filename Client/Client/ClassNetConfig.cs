using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Client
{
    class ClassNetConfig
    {
        

        public static string GetAppConfig(string key)
        {
       
            return ConfigurationManager.AppSettings[key];
  
        }

        public static void SetAppConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

            cfgCollection.Remove(key);
            cfgCollection.Add(key, value);


            config.Save(ConfigurationSaveMode.Modified);

   
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

       
    }
}
