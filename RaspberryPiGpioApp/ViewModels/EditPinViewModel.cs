using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaspberryPiGpioApp.Models;
using RaspberryPiGpioApp.Services;
using System;
using System.Linq;

namespace RaspberryPiGpioApp.ViewModels;
public partial class EditPinViewModel : BaseViewModel
{

    #region Fields
    readonly ILogger<EditPinViewModel> _logger;
    readonly RpiService _rpiService;
    [ObservableProperty]
    private BoardItem? boardItem = null;
    [ObservableProperty]
    bool isOpen;
    [ObservableProperty]
    string pinMode = string.Empty;
    #endregion

    #region Constructors
    public EditPinViewModel()
    {
        this._logger = Ioc.Default.GetRequiredService<ILogger<EditPinViewModel>>();
        this._logger.LogDebug("Creating EditPinViewModel");
        this._rpiService = Ioc.Default.GetRequiredService<RpiService>();
        this.PropertyChanged += EditPinViewModel_PropertyChanged;
    }
    #endregion

    #region Destructors
    ~EditPinViewModel()
    {
        this._logger.LogDebug("Destroying EditPinViewModel");
        this.PropertyChanged -= EditPinViewModel_PropertyChanged;
    }
    #endregion

    #region Private methods
    private async void EditPinViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        string propertyName = e.PropertyName ?? string.Empty;

        if (propertyName == nameof(IsOpen))
        {
            int pinNumber = this.BoardItem!.PinNumber;
            if (this.IsOpen)
            {
                var result = await this._rpiService.OpenPin(pinNumber);
                this.IsOpen = await this._rpiService.IsPinOpenAsync(pinNumber);
            }
            else
            {
                var result = await this._rpiService.ClosePinAsync(pinNumber);
                this.IsOpen = await this._rpiService.IsPinOpenAsync(pinNumber);
            }
        }
    }
    private async Task GetPinMode()
    {
        var pinNumber = this.BoardItem!.PinNumber;
        this.PinMode = await this._rpiService.GetPinModeAsync(pinNumber);
        this.IsOpen = await this._rpiService.IsPinOpenAsync(pinNumber);
    }
    #endregion

    #region Public methods
    public void SetBoardItem(BoardItem boardItem)
    {
        this.BoardItem = boardItem;
    }
    #endregion

}