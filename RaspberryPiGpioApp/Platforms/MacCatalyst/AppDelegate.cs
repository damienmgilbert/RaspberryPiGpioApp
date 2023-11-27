using Foundation;

namespace RaspberryPiGpioApp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {

        #region Protected methods
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        #endregion

    }
}
