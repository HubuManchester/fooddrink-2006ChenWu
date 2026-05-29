namespace WorldCuisineApp.ViewModels;

public class HelpViewModel : BaseViewModel
{
    public HelpViewModel()
    {
        Title = "Help";
    }

    public string HelpText =>
        """
        World Cuisine Explorer — demo guide

        • Browse dishes on Home; tap a card for details.
        • Search by name, country, or region.
        • Shake your device for a random dish (accelerometer).
        • On a dish page: ♥ favorites (vibration), 🔊 read aloud (TTS).
        • Camera tab: capture a photo of food you try.
        • Settings: dark mode, adjustable text size, MockAPI URL.
        • Login validates empty fields and short passwords.

        Recording tips: show validation errors on Login, shake on Home,
        TTS on Detail, camera capture, and Settings accessibility options.
        """;
}
