using CommunityToolkit.Mvvm.DependencyInjection;
using RaspberryPiGpioApp.Models;
using RaspberryPiGpioApp.ViewModels;

namespace RaspberryPiGpioApp.Views;
[QueryProperty(nameof(BoardItem), "BoardItem")]
public partial class EditPinPage : ContentPage
{

    #region Fields
    private BoardItem? boardItem;
    #endregion

    #region Constructors
    public EditPinPage()
    {
        InitializeComponent();
        this.BindingContext = Ioc.Default.GetRequiredService<EditPinViewModel>();
        this.NavigatedTo += this.OnNavigatedTo;
    }
    #endregion

    #region Destructors
    ~EditPinPage()
    {
        this.NavigatedTo -= this.OnNavigatedTo;
    }
    #endregion

    #region Private methods
    void OnNavigatedTo(object? sender, NavigatedToEventArgs e)
    {
        EditPinViewModel? vm = this.BindingContext as EditPinViewModel;

        if (this.BoardItem is null)
        {
            throw new NullReferenceException("Board item is null!");
        }

        vm?.SetBoardItem(this.BoardItem!);
    }
    #endregion

    #region Public properties
    public BoardItem? BoardItem
    {
        get => boardItem; set
        {
            boardItem = value;
            OnPropertyChanged();
        }
    }
    #endregion

}