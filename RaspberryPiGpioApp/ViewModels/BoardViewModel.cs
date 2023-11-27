using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RaspberryPiGpioApp.Models;
using RaspberryPiGpioApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RaspberryPiGpioApp.ViewModels;
public partial class BoardViewModel : ObservableRecipient
{

    #region Fields
    [ObservableProperty]
    private ObservableCollection<BoardItem> boardItems = [];
    [ObservableProperty]
    private BoardItem? selectedBoardItem = null;
    #endregion

    #region Constructors
    public BoardViewModel()
    {
        this.BoardItems = GetBoardItems();
    }
    #endregion

    #region Private methods
    [RelayCommand]
    private async Task EditGPIO()
    {
        if (this.SelectedBoardItem is null)
        {
            return;
        }

        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "BoardItem", this.SelectedBoardItem }
        };
        await Shell.Current.GoToAsync($"EditPin", navigationParameter);
    }
    private ObservableCollection<BoardItem> GetBoardItems()
    {
        ObservableCollection<BoardItem> boardItems =
        [
            new BoardItem("3.3V Power", 1),
            new BoardItem("5V Power", 2),
            new BoardItem("GPIO 2 (I2C1 SDA)", 3, true),
            new BoardItem("5V Power", 4),
            new BoardItem("GPIO 3 (I2C1 SCL)", 5, true),
            new BoardItem("Ground", 6),
            new BoardItem("GPIO 4 (GPCLK0)", 7, true),
            new BoardItem("GPIO 14 (UART TX)", 8, true),
            new BoardItem("Ground", 9),
            new BoardItem("GPIO 15 (UART RX)", 10, true),
            new BoardItem("GPIO 17", 11, true),
            new BoardItem("GPIO 18 (PCM CLK)", 12, true),
            new BoardItem("GPIO 27", 13, true),
            new BoardItem("Ground", 14),
            new BoardItem("GPIO 22", 15, true),
            new BoardItem("GPIO 23", 16, true),
            new BoardItem("3.3V Power", 17),
            new BoardItem("GPIO 24", 18, true),
            new BoardItem("GPIO 10 (SPI0 MOSI)", 19, true),
            new BoardItem("Ground", 20),
            new BoardItem("GPIO 9 (SPI0 MISO)", 21, true),
            new BoardItem("GPIO 25", 22, true),
            new BoardItem("GPIO 11 (SPI0 SCLK)", 23, true),
            new BoardItem("GPIO 8 (SPI0 CE0)", 24, true),
            new BoardItem("Ground", 25),
            new BoardItem("GPIO 7 (SPI0 CE1)", 26, true),
            new BoardItem("GPIO 0 (EEPROM SDA)", 27, true),
            new BoardItem("GPIO 1 (EEPROM SCL)", 28, true),
            new BoardItem("GPIO 5", 29, true),
            new BoardItem("Ground", 30),
            new BoardItem("GPIO 6", 31, true),
            new BoardItem("GPIO 12 (PWM0)", 32, true),
            new BoardItem("GPIO 13 (PWM1)", 33, true),
            new BoardItem("Ground", 34),
            new BoardItem("GPIO 19 (PCM FS)", 35, true),
            new BoardItem("GPIO 16", 36, true),
            new BoardItem("GPIO 26", 37, true),
            new BoardItem("GPIO 20 (PCM DIN)", 38, true),
            new BoardItem("Ground", 39),
            new BoardItem("GPIO 21 (PCM DOUT)", 40, true),
        ];

        return boardItems;
    }
    #endregion

}
