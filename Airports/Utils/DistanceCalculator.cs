namespace Airports.Utils
{
    public static class DistanceCalculator
    {
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadius = 6371;

            var lat1Rad = ToRadians(lat1);
            var lat2Rad = ToRadians(lat2);
            var lon1Rad = ToRadians(lon1);
            var lon2Rad = ToRadians(lon2);

            var dLat = lat2Rad - lat1Rad;
            var dLon = lon2Rad - lon1Rad;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadius * c;
        }

        private static double ToRadians(double degrees) 
        {
            return (degrees * Math.PI / 180); 
        }
    }
}
