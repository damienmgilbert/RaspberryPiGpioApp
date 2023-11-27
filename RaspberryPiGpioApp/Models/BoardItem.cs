using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Linq;

namespace RaspberryPiGpioApp.Models;

public partial class BoardItem : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private int pinNumber;

    [ObservableProperty]
    private bool isGPIO;

    public BoardItem(string name, int pinNumber, bool isGPIO)
    {
        this.name = name;
        this.pinNumber = pinNumber;
        this.isGPIO = isGPIO;
    }

    public BoardItem(string name, int pinNumber)
    {
        this.name = name;
        this.pinNumber = pinNumber;
    }
}