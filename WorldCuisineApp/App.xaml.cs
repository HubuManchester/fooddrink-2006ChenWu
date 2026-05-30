namespace WorldCuisineApp;

public partial class App : Application
{
    private readonly AppShell _appShell;

    public App(AppShell shell)
    {
        _appShell = shell;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) =>
        new Window(_appShell);
}
