using LorenaReels.Interfaces;
using LorenaReels.Models;

#if ANDROID
using Android.Provider;
using Android.Content;
#endif

namespace LorenaReels.Services;

public sealed class VideoScanner : IVideoScanner
{
    public async Task<List<VideoItem>> GetAllVideosAsync()
    {
        var status = await Permissions.RequestAsync<Permissions.StorageRead>();
        if (status != PermissionStatus.Granted)
            return new List<VideoItem>();

        return await Task.Run(() => ScanVideos());
    }

    private List<VideoItem> ScanVideos()
    {
        var results = new List<VideoItem>();

#if ANDROID
        var context = Android.App.Application.Context;
        var uri     = MediaStore.Video.Media.ExternalContentUri!;

        string[] projection =
        {
            MediaStore.Video.Media.InterfaceConsts.Title,
            MediaStore.Video.Media.InterfaceConsts.Data,
        };

        using var cursor = context.ContentResolver!.Query(
            uri, projection, null, null, null);

        if (cursor == null) return results;

        int colTitle = cursor.GetColumnIndexOrThrow(
            MediaStore.Video.Media.InterfaceConsts.Title);
        int colPath  = cursor.GetColumnIndexOrThrow(
            MediaStore.Video.Media.InterfaceConsts.Data);

        while (cursor.MoveToNext())
        {
            var title = cursor.GetString(colTitle) ?? "Unknown";
            var path  = cursor.GetString(colPath)  ?? string.Empty;

            if (!string.IsNullOrEmpty(path))
                results.Add(new VideoItem { Title = title, FilePath = path });
        }
#endif
        // ترتيب عشوائي
        var rng = new Random();
        for (int i = results.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (results[i], results[j]) = (results[j], results[i]);
        }

        return results;
    }
}