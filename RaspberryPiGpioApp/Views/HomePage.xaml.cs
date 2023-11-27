using CommunityToolkit.Mvvm.DependencyInjection;
using RaspberryPiGpioApp.ViewModels;

namespace RaspberryPiGpioApp.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
		this.BindingContext = Ioc.Default.GetRequiredService<HomeViewModel>();
	}
}