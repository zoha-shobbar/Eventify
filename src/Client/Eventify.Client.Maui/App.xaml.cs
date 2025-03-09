using Microsoft.Extensions.Logging;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Eventify.Client.Maui;

public partial class App
{
    private readonly Page mainPage;
    private readonly ILogger<App> logger;
    private readonly IExceptionHandler exceptionHandler;
    private readonly IBitDeviceCoordinator deviceCoordinator;
    private readonly IStringLocalizer<AppStrings> localizer;

    public App(MainPage mainPage,
        ILogger<App> logger,
        IExceptionHandler exceptionHandler,
        IBitDeviceCoordinator deviceCoordinator,
        IStringLocalizer<AppStrings> localizer)
    {
        this.logger = logger;
        this.localizer = localizer;
        this.exceptionHandler = exceptionHandler;
        this.deviceCoordinator = deviceCoordinator;
        this.mainPage = new NavigationPage(mainPage);


        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(mainPage) { };
    }

    protected override async void OnStart()
    {
        try
        {
            base.OnStart();

            await deviceCoordinator.ApplyTheme(AppInfo.Current.RequestedTheme is AppTheme.Dark);

            #if Android
            const int minimumSupportedWebViewVersion = 84;
            // Download link for Android emulator (x86 or x86_64)
            // https://www.apkmirror.com/apk/google-inc/chrome/chrome-84-0-4147-89-release/
            // https://www.apkmirror.com/apk/google-inc/android-system-webview/android-system-webview-84-0-4147-111-release/

            if (Version.TryParse(Android.Webkit.WebView.CurrentWebViewPackage?.VersionName, out var webViewVersion) &&
                webViewVersion.Major < minimumSupportedWebViewVersion)
            {
                var webViewName = Android.Webkit.WebView.CurrentWebViewPackage.PackageName;
                logger.LogWarning("{webViewName} version {version} is not supported.", webViewName, webViewVersion);
                await App.Current!.Windows[0].Page!.DisplayAlert("Eventify", localizer[nameof(AppStrings.UpdateWebViewThroughGooglePlay)], localizer[nameof(AppStrings.Ok)]);
                await Launcher.OpenAsync($"https://play.google.com/store/apps/details?id={webViewName}");
            }
            #endif
            
        }
        catch (Exception exp)
        {
            exceptionHandler.Handle(exp);
        }
    }

}
