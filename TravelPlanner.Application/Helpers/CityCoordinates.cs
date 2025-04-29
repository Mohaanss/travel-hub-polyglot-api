namespace TravelPlanner.Application.Helpers
{
    public static class CityCoordinates
    {
        private static readonly Dictionary<string, (double Lat, double Lon)> Coordinates = new()
        {
            { "PAR", (48.8566, 2.3522) },
            { "NYC", (40.7128, -74.0060) },
            { "LON", (51.5074, -0.1278) },
            { "BER", (52.5200, 13.4050) },
            { "ROM", (41.9028, 12.4964) },
            { "TYO", (35.6895, 139.6917) }
        };

        public static (double Lat, double Lon)? Get(string code)
        {
            return Coordinates.TryGetValue(code.ToUpper(), out var coord) ? coord : null;
        }
    }
}
