namespace TravelPlanner.Application.Helpers
{
    public static class CityCoordinates
    {
        private static readonly Dictionary<string, (double Lat, double Lon)> Coordinates = new()
        {
            { "PAR", (48.8566, 2.3522) },      // Paris
            { "NYC", (40.7128, -74.0060) },    // New York City
            { "LON", (51.5074, -0.1278) },     // London
            { "BER", (52.5200, 13.4050) },     // Berlin
            { "ROM", (41.9028, 12.4964) },     // Rome
            { "TYO", (35.6895, 139.6917) },    // Tokyo
            { "BKK", (13.7563, 100.5018) },    // Bangkok
            { "SIN", (1.3521, 103.8198) },     // Singapore
            { "SYD", (-33.8688, 151.2093) },   // Sydney
            { "DEL", (28.6139, 77.2090) },     // Delhi
            { "SHA", (31.2304, 121.4737) },    // Shanghai
            { "HKG", (22.3193, 114.1694) },    // Hong Kong
            { "CAI", (30.0444, 31.2357) },     // Cairo
            { "MEX", (19.4326, -99.1332) },    // Mexico City
            { "SFO", (37.7749, -122.4194) },   // San Francisco
            { "LAX", (34.0522, -118.2437) },   // Los Angeles
            { "JNB", (-26.2041, 28.0473) },    // Johannesburg
            { "IST", (41.0082, 28.9784) },     // Istanbul
            { "MOW", (55.7558, 37.6176) },     // Moscow
            { "SAO", (-23.5505, -46.6333) },   // São Paulo
            { "BUE", (-34.6037, -58.3816) },   // Buenos Aires
            { "TOR", (43.651070, -79.347015) },// Toronto
            { "DXB", (25.2048, 55.2708) },     // Dubai
            { "AMS", (52.3676, 4.9041) },      // Amsterdam
            { "MAD", (40.4168, -3.7038) }      // Madrid
        };

        public static (double Lat, double Lon)? Get(string code)
        {
            return Coordinates.TryGetValue(code.ToUpper(), out var coord) ? coord : null;
        }
    }
}
