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
public partial class HomeViewModel : ObservableRecipient
{

    #region Fields
    private readonly ILogger<HomeViewModel> _logger;
    
    private readonly RpiService _rpiService;

    [ObservableProperty]
    private bool isPinNumberValid;
    
    [ObservableProperty]
    private string numberingScheme = string.Empty;
    
    [ObservableProperty]
    private int pinCount;

    [ObservableProperty]
    private int pinNumber;

    [ObservableProperty]
    private string baseUrl = string.Empty;

    [ObservableProperty]
    private int port;
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
    private async Task ClosePin()
    {
        if (!this.IsPinNumberValid)
        {
            return;
        }

        bool result = await this._rpiService.ClosePin(this.PinNumber);
        if (result)
        {
            this._logger.LogDebug("Close pin {PinNumber}", PinNumber);
        }
        else
        {
            this._logger.LogDebug("Unable to close pin {PinNumber}", PinNumber);
        }
    }

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
    private async Task ConnectToDevice()
    {
        try
        {
            this._rpiService.Connet();
            string result = await this._rpiService.GetNumberingScheme();
            this.Port = this._rpiService.Port;
            this.BaseUrl = this._rpiService.BaseUrl;
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

        string result = await this._rpiService.GetPinMode(this.PinNumber);
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
            this._logger.LogDebug("Set the pin mode to {PinMode} for pin {PinNumber}", pinMode, PinNumber);
        }
        else
        {
            this._logger.LogDebug("Unable to set the pin mode to {PinMode} for pin {PinNumber}", pinMode, PinNumber);
        }
    }
    #endregion

}
