using AudioBlend.API.DbInitializer.Models;
using Newtonsoft.Json;

namespace AudioBlend.API.DbInitializer
{
    
    public static class MusicDataReader<T> where T : class
    {
        public static T ReadJsonFile(string path)
        {
            using StreamReader r = new StreamReader(path);
            string json = r.ReadToEnd();

            T jsonInfo = JsonConvert.DeserializeObject<T>(json);

            return jsonInfo;
        }
    }
}
