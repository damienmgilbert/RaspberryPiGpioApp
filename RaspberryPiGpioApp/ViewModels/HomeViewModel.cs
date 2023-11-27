using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RaspberryPiGpioApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPiGpioApp.ViewModels;
public partial class HomeViewModel : BaseViewModel
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
    private string pinMode = string.Empty;
    [ObservableProperty]
    private int pinNumber;
    [ObservableProperty]
    string pinValue = string.Empty;
    [ObservableProperty]
    private int port;
    [ObservableProperty]
    private string selectedPinMode = string.Empty;
    [ObservableProperty]
    private string selectedWritablePinValue = string.Empty;
    [ObservableProperty]
    string writablePinValue = string.Empty;
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

        bool result = await this._rpiService.ClosePinAsync(this.PinNumber);
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
            this._rpiService.Connect();
            string result = await this._rpiService.GetNumberingSchemeAsync();
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
        this.PinCount = await this._rpiService.GetPinCountAsync();
        this.NumberingScheme = await this._rpiService.GetNumberingSchemeAsync();
    }
    [RelayCommand]
    private async Task GetPinMode()
    {
        if (!this.IsPinNumberValid)
        {
            return;
        }

        this.PinMode = await this._rpiService.GetPinModeAsync(this.PinNumber);
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
    private async Task Read()
    {
        try
        {
            this.PinValue = await this._rpiService.ReadAsync(this.PinNumber);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Unable to read pin value");
        }
    }
    [RelayCommand]
    private async Task SelectedPinModeChanged()
    {
        try
        {
            bool result = await this._rpiService.SetPinModeAsync(this.PinNumber, this.SelectedPinMode);
            if (result)
            {
                await this.ShowToastMessage($"Set the pin mode to {this.SelectedPinMode} for pin {this.PinNumber}");
            }
            else
            {
                await this.ShowToastMessage($"Unable to set the pin mode to {this.SelectedPinMode} for pin {this.PinNumber}");
            }
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Unable to set the pin mode to {SelectedPinMode} for pin {PinNumber}", this.SelectedPinMode, this.PinNumber);
            await this.ShowToastMessage($"Unable to set the pin mode to {this.SelectedPinMode} for pin {this.PinNumber}");
        }
    }
    [RelayCommand]
    private async Task SelectedWriteablePinValueChanged()
    {
        await this.Write();
    }
    [RelayCommand]
    private async Task SetPinMode(string pinMode)
    {
        if (!this.IsPinNumberValid)
        {
            return;
        }

        bool result = await this._rpiService.SetPinModeAsync(this.PinNumber, pinMode);
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

}
