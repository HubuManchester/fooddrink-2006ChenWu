namespace WorldCuisineApp.Services;

public interface IHardwareService
{
    Task SpeakAsync(string text, CancellationToken cancellationToken = default);
    void VibrateSuccess();
    void VibrateError();
    Task<FileResult?> CapturePhotoAsync();
    Task<FileResult?> PickPhotoAsync();
    void StartShakeDetection(Action onShake);
    void StopShakeDetection();
    bool IsAccelerometerAvailable { get; }
}
