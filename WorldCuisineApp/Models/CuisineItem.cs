namespace WorldCuisineApp.Models;

/// <summary>
/// Represents a world cuisine entry from the API or local fallback data.
/// </summary>
public class CuisineItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int SpiceLevel { get; set; }
    public string FunFact { get; set; } = string.Empty;
}
