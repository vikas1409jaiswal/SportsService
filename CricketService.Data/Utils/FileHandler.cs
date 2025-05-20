using Newtonsoft.Json;

namespace CricketService.Data.Utils
{
    public class FileHandler
    {
        public static void WriteObjectToJsonFile(string filePath, object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
        }
    }
}
