using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using LorenaReels.Interfaces;
using LorenaReels.Models;
using LorenaReels.ViewModels;

namespace LorenaReels.ViewModels;

public sealed class ReelsViewModel : BaseViewModel
{
    private readonly IVideoScanner _scanner;

    public ObservableCollection<VideoItem> Videos { get; } = new();

    private VideoItem? _currentVideo;
    public VideoItem? CurrentVideo
    {
        get => _currentVideo;
        set => SetProperty(ref _currentVideo, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private int _currentIndex;
    public int CurrentIndex
    {
        get => _currentIndex;
        set => SetProperty(ref _currentIndex, value);
    }

    public ICommand LoadCommand { get; }
    public ICommand NextCommand { get; }
    public ICommand PreviousCommand { get; }

    public ReelsViewModel(IVideoScanner scanner)
    {
        _scanner = scanner;

        LoadCommand = new Command(async () => await LoadAsync());
        NextCommand = new Command(Next);
        PreviousCommand = new Command(Previous);
    }

    public async Task LoadAsync()
    {
        if (IsLoading)
            return;

        IsLoading = true;
        ClearError();

        try
        {
            var list = await _scanner.GetAllVideosAsync();

            Videos.Clear();
            foreach (var v in list)
                Videos.Add(v);

            if (Videos.Count > 0)
            {
                CurrentIndex = 0;
                CurrentVideo = Videos[0];
            }
        }
        catch (Exception ex)
        {
            SetError($"Load failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void Next()
    {
        if (CurrentIndex < Videos.Count - 1)
        {
            CurrentIndex++;
            CurrentVideo = Videos[CurrentIndex];
        }
    }

    public void Previous()
    {
        if (CurrentIndex > 0)
        {
            CurrentIndex--;
            CurrentVideo = Videos[CurrentIndex];
        }
    }
}