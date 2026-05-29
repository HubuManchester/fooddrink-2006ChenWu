using WorldCuisineApp.Constants;

namespace WorldCuisineApp.Services;

/// <summary>
/// Wraps device hardware: TTS, vibration, camera, and shake detection.
/// </summary>
public class HardwareService : IHardwareService, IDisposable
{
    private Action? _onShake;
    private DateTime _lastShakeUtc = DateTime.MinValue;
    private bool _listening;

    public bool IsAccelerometerAvailable => Accelerometer.Default.IsSupported;

    public async Task SpeakAsync(string text, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        try
        {
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            var locale = locales.FirstOrDefault(l => l.Language.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                         ?? locales.FirstOrDefault();

            var options = new SpeechOptions
            {
                Pitch = 1.0f,
                Volume = 1.0f,
                Locale = locale
            };

            await TextToSpeech.Default.SpeakAsync(text, options, cancellationToken);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"TTS failed: {ex.Message}");
            throw;
        }
    }

    public void VibrateSuccess()
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(80));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Vibration failed: {ex.Message}");
        }
    }

    public void VibrateError()
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(40));
            Task.Delay(120).ContinueWith(_ =>
            {
                try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(40)); }
                catch { /* ignore */ }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Vibration failed: {ex.Message}");
        }
    }

    public async Task<FileResult?> CapturePhotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
                return null;

            return await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
            {
                Title = "Capture your dish"
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Camera failed: {ex.Message}");
            return null;
        }
    }

    public void StartShakeDetection(Action onShake)
    {
        _onShake = onShake;
        if (!Accelerometer.Default.IsSupported || _listening)
            return;

        Accelerometer.Default.ReadingChanged += OnAccelerometerReading;
        Accelerometer.Default.Start(SensorSpeed.Game);
        _listening = true;
    }

    public void StopShakeDetection()
    {
        if (!_listening)
            return;

        Accelerometer.Default.ReadingChanged -= OnAccelerometerReading;
        Accelerometer.Default.Stop();
        _listening = false;
        _onShake = null;
    }

    private void OnAccelerometerReading(object? sender, AccelerometerChangedEventArgs e)
    {
        var a = e.Reading.Acceleration;
        var magnitude = Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);

        if (magnitude < AppConstants.ShakeThreshold)
            return;

        var now = DateTime.UtcNow;
        if ((now - _lastShakeUtc).TotalMilliseconds < AppConstants.ShakeCooldownMs)
            return;

        _lastShakeUtc = now;
        MainThread.BeginInvokeOnMainThread(() => _onShake?.Invoke());
    }

    public void Dispose() => StopShakeDetection();
}
