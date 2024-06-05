namespace AudioBlend.Core.MusicData.Models.DTOs.Searches
{
    public class SearchRepoResult<T> where T : class
    {
        public T Result { get; set; }
        public int Score { get; set; }
    }
}
