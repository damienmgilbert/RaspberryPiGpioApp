using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Extensions.Logging;
using RaspberryPiGpioApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;

namespace RaspberryPiGpioApp.ViewModels;
public partial class HomeViewModel : ObservableRecipient
{

    #region Fields
    private readonly ILogger<HomeViewModel> _logger;
    private readonly RpiService _rpiService;
    [ObservableProperty]
    private string baseUrl = string.Empty;
    [ObservableProperty]
    private bool isPinNumberValid;
    [ObservableProperty]
    private string numberingScheme = string.Empty;
    [ObservableProperty]
    private int pinCount;
    [ObservableProperty]
    private int pinNumber;
    [ObservableProperty]
    private int port;

    [ObservableProperty]
    private string pinMode = string.Empty;

    [ObservableProperty]
    private string selectedPinMode = string.Empty;
    #endregion

    #region Constructors
    public HomeViewModel()
    {
        this._logger = Ioc.Default.GetRequiredService<ILogger<HomeViewModel>>();
        this._rpiService = Ioc.Default.GetRequiredService<RpiService>();
    }
    #endregion

    #region Private methods
    [RelayCommand]
    private void ChangeBaseUrl()
    {
        this._rpiService.SetBaseAddress(this.BaseUrl);
    }

    private async Task ShowToastMessage(string message)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 16;
        var toast = Toast.Make(message, duration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }

    [RelayCommand]
    private void ChangePort()
    {
        this._rpiService.SetPort(this.Port);
    }
    [RelayCommand]
    private async Task ClosePin()
    {
        if (!this.IsPinNumberValid)
        {
            return;
        }

        bool result = await this._rpiService.ClosePin(this.PinNumber);
        if (result)
        {
            await this.ShowToastMessage($"Closed pin {this.PinNumber}");
            this._logger.LogDebug("Close pin {PinNumber}", PinNumber);
        }
        else
        {
            this._logger.LogDebug("Unable to close pin {PinNumber}", PinNumber);
        }
    }
    [RelayCommand]
    private async Task ConnectToDevice()
    {
        try
        {
            this._rpiService.Connet();
            string result = await this._rpiService.GetNumberingScheme();
            this.Port = this._rpiService.Port;
            this.BaseUrl = this._rpiService.BaseUrl;
            await this.ShowToastMessage($"Connected to http//{this.BaseUrl}:{this.Port}!");
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Could not connect to device");
        }
    }
    [RelayCommand]
    private async Task GetInformation()
    {
        this.PinCount = await this._rpiService.GetPinCount();
        this.NumberingScheme = await this._rpiService.GetNumberingScheme();
    }
    [RelayCommand]
    private async Task GetPinMode()
    {
        if (!this.IsPinNumberValid)
        {
            return;
        }

        this.PinMode = await this._rpiService.GetPinMode(this.PinNumber);
    }
    [RelayCommand]
    private async Task OpenPin()
    {
        if (!this.IsPinNumberValid)
        {
            return;
        }

        bool result = await this._rpiService.OpenPin(this.PinNumber);
        if (result)
        {
            await this.ShowToastMessage($"Opened pin {this.PinNumber}");
            this._logger.LogDebug("Opened pin {PinNumber}", this.PinNumber);
        }
        else
        {
            this._logger.LogDebug("Unable to open pin {PinNumber}", this.PinNumber);
        }
    }
    [RelayCommand]
    private async Task SetPinMode(string pinMode)
    {
        if (!this.IsPinNumberValid)
        {
            return;
        }

        bool result = await this._rpiService.SetPinMode(this.PinNumber, pinMode);
        if (result)
        {
            await this.ShowToastMessage($"Set the pin mode to {pinMode} for pin {this.PinNumber}");
            this._logger.LogDebug("Set the pin mode to {PinMode} for pin {PinNumber}", pinMode, PinNumber);
        }
        else
        {
            this._logger.LogDebug("Unable to set the pin mode to {PinMode} for pin {PinNumber}", pinMode, PinNumber);
        }
    }
    #endregion

    #region Public properties
    public List<string> PinModes => new List<string>()
    {
        "InputPullUp", "InputPullDown", "Input", "Output"
    };

    public List<string> WriteablePinValues => new List<string>()
    {
        "HIGH", "LOW"
    };
    #endregion

    [ObservableProperty]
    string pinValue = string.Empty;

    [RelayCommand]
    private async Task Read()
    {
        try
        {
            this.PinValue = await this._rpiService.ReadAsync(this.PinNumber);
        }
        catch(Exception ex)
        {
            this._logger.LogError(ex, "Unable to read pin value");
        }
    }

    [ObservableProperty]
    string writablePinValue = string.Empty;

    [RelayCommand]
    private async Task SelectedPinModeChanged()
    {
        try
        {
            bool result = await this._rpiService.SetPinMode(this.PinNumber, this.SelectedPinMode);
            if (result)
            {
                await this.ShowToastMessage($"Set the pin mode to {this.SelectedPinMode} for pin {this.PinNumber}");
            }
            else
            {
                await this.ShowToastMessage($"Unable to set the pin mode to {this.SelectedPinMode} for pin {this.PinNumber}");
            }
        }
        catch(Exception ex )
        {
            this._logger.LogError(ex, "Unable to set the pin mode to {SelectedPinMode} for pin {PinNumber}", this.SelectedPinMode, this.PinNumber);
            await this.ShowToastMessage($"Unable to set the pin mode to {this.SelectedPinMode} for pin {this.PinNumber}");
        }
    }

    [ObservableProperty]
    private string selectedWritablePinValue = string.Empty;

    [RelayCommand]
    private async Task SelectedWriteablePinValueChanged()
    {
        await this.Write();
    }

    [RelayCommand]
    private async Task Write()
    {
        try
        {
            bool result = await this._rpiService.WriteAsync(this.PinNumber, this.SelectedWritablePinValue);
            if (result)
            {
                await this.ShowToastMessage($"Wrote value of {this.SelectedWritablePinValue} to pin {this.PinNumber}");
            }
            else
            {
                await this.ShowToastMessage($"Unable to write value of {this.SelectedWritablePinValue} to pin {this.PinNumber}");
            }
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Unable to write value of {SelectedWritablePinValue} to pin {PinNumber}", this.SelectedWritablePinValue, this.PinNumber);
            await this.ShowToastMessage($"Unable to write value of {this.SelectedWritablePinValue} to pin {this.PinNumber}");
        }
    }
}
