using CommunityToolkit.Mvvm.DependencyInjection;
using RaspberryPiGpioApp.ViewModels;

namespace RaspberryPiGpioApp.Views;
public partial class BoardPage : ContentPage
{

    #region Constructors
    public BoardPage()
    {
        InitializeComponent();
        this.BindingContext = Ioc.Default.GetRequiredService<BoardViewModel>();
    }
    #endregion

}