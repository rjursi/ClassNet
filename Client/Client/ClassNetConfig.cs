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
        public static void DoProtection()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = config.AppSettings;

            if (!section.SectionInformation.IsProtected && !section.ElementInformation.IsLocked)
            {
                {
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    section.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Full);

                    ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
                }
            }
        }

        public static string GetAppConfig(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = config.AppSettings;
            string result = "";

            try
            {


                if (section != null)
                {
                    if (section.SectionInformation.IsProtected)
                    {
                        section.SectionInformation.UnprotectSection();
                        section.SectionInformation.ForceDeclaration(true);
                        section.SectionInformation.ForceSave = true;
                        config.Save(ConfigurationSaveMode.Full);
                    }

                }

                result = ConfigurationManager.AppSettings[key];
            }
            catch (ConfigurationErrorsException)
            {

                DoProtection();

                if (section != null)
                {
                    if (section.SectionInformation.IsProtected)
                    {
                        section.SectionInformation.UnprotectSection();
                        section.SectionInformation.ForceDeclaration(true);
                        section.SectionInformation.ForceSave = true;
                        config.Save(ConfigurationSaveMode.Full);
                    }
                    result = ConfigurationManager.AppSettings[key];
                }

            }
            DoProtection();

            return result;

        }

        public static void SetAppConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSection section = config.AppSettings;

            if (section != null)
            {
                if (section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.UnprotectSection();
                    section.SectionInformation.ForceDeclaration(true);
                    section.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Full);
                }
            }

            KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

            cfgCollection.Remove(key);
            cfgCollection.Add(key, value);


            config.Save(ConfigurationSaveMode.Modified);

            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            section.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Full);

            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }
    }
}
