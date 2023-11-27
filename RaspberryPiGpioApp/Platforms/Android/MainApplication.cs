using Android.App;
using Android.Runtime;

namespace RaspberryPiGpioApp
{
    [Application]
    public class MainApplication : MauiApplication
    {

        #region Constructors
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }
        #endregion

        #region Protected methods
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        #endregion

    }
}
