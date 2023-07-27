using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class JsonPackService
    {
        public static bool TryLoadPack<T>(string filePath, out T? pack)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                pack = JsonConvert.DeserializeObject<T>(json);

                return pack is not null;
            }
            catch
            {
                pack = default;
                return false;
            }
        }

        public static bool TryLoadPacks<T>(string directoryPath, out ObservableCollection<T> packs)
        {
            packs = new ObservableCollection<T>();

            bool isAllPacksLoaded = true;

            try
            {
                foreach (string filePath in Directory.GetFiles(directoryPath, "*.json"))
                {
                    if (TryLoadPack(filePath, out T? pack))
                        packs.Add(pack!);
                    else
                        isAllPacksLoaded = false;
                }

                return isAllPacksLoaded;
            }
            catch
            {
                return false;
            }
        }
    }
}
