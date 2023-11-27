using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Linq;

namespace RaspberryPiGpioApp.Models;
public partial class BoardItem : ObservableObject
{

    #region Fields
    [ObservableProperty]
    private bool isGPIO;
    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private int pinNumber;
    #endregion

    #region Constructors
    public BoardItem(string name, int pinNumber)
    {
        this.name = name;
        this.pinNumber = pinNumber;
    }
    public BoardItem(string name, int pinNumber, bool isGPIO)
    {
        this.name = name;
        this.pinNumber = pinNumber;
        this.isGPIO = isGPIO;
    }
    #endregion

}