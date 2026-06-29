using LorenaReels.ViewModels;

namespace LorenaReels.Views;

public partial class ReelsPage : ContentPage
{
    private readonly ReelsViewModel _vm;

    public ReelsPage(ReelsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_vm.Videos.Count == 0)
             _vm.LoadCommand.Execute(null);
    }
}