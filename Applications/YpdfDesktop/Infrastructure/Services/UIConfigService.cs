using Newtonsoft.Json;
using System.IO;
using YpdfDesktop.Models.Configuration;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class UIConfigService
    {
        public static bool TryLoadUIConfiguration(out UIConfig config)
        {
            try
            {
                string path = SharedConfig.Files.UIConfig;
                string json = File.ReadAllText(path);

                config = JsonConvert.DeserializeObject<UIConfig>(json) ?? new UIConfig();
                return true;
            }
            catch
            {
                config = new UIConfig();
                return false;
            }
        }

        public static bool TrySaveUIConfiguration(UIConfig config)
        {
            try
            {
                string path = SharedConfig.Files.UIConfig;
                string json = JsonConvert.SerializeObject(config);

                File.WriteAllText(path, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
