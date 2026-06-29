using LorenaReels.Models;

namespace LorenaReels.Interfaces
{
    public interface IVideoScanner
    {
        Task<List<VideoItem>> GetAllVideosAsync();
    }
}
