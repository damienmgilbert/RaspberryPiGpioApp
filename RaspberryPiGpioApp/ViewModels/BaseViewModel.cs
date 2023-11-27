using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Linq;

namespace RaspberryPiGpioApp.ViewModels;
public class BaseViewModel : ObservableRecipient
{

    #region Protected methods
    protected async Task ShowToastMessage(string message)
    {
        CancellationTokenSource cancellationTokenSource = new();
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 16;
        var toast = Toast.Make(message, duration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }
    #endregion

}
