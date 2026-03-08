using NetTopologySuite.Geometries;

namespace WayPoint.Application.Common.Interfaces.ExternalServices;

public record GeocodingResult(Point Coordinate, double ConfidenceScore);

public interface IGeocodingService
{
    /// <summary>แปลงที่อยู่เป็นพิกัด GPS — ผ่าน Redis → ORS → Nominatim → ILike</summary>
    Task<GeocodingResult?> GeocodeAsync(string address, CancellationToken ct = default);
}
