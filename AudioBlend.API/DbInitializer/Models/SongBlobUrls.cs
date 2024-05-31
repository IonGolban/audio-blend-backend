using static System.Net.WebRequestMethods;

namespace AudioBlend.API.DbInitializer.Models
{
    public static class SongBlobUrls
    {
        private static List<string> _urls = new List<string>();

        private static void initUrls()
        {
            _urls = new List<string>()
            {
                "https://golbanionstorage.blob.core.windows.net/default/Affogato.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/All For You.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Africa.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Alone.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Andromeda.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Aurora.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Bake A Pie.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Better Days.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Biscuit.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Bloom.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Bonjour.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Break Up.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Brunch.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Butter.mp",
                "https://golbanionstorage.blob.core.windows.net/default/Cafe.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Cheese.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Chocolate.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Cloud.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Cold.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Coral Reef.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Cream.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Cuba.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Daily.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Dreaming After Work.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Dreaming.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Empty.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Every Day - Jazz version.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Every Day.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Every Day - Jazz version.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Flowers.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Flying.mp3",
                "https://golbanionstorage.blob.core.windows.net/default/Forest.mp3"
            };
        }

        public static List<string> GetUrls()
        {
            initUrls();
            return _urls;
        }

        public static void AddUrl(string url)
        {
            _urls.Add(url);
        }
    }
}
