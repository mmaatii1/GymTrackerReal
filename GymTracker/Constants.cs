namespace GymTracker
{
    public static class Constants
    {

        // URL of REST service (Android does not use localhost)
        // Use http cleartext for local deployment. Change to https for production
        public static string LocalhostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
        public static string Scheme = "https"; // or http
        public static string Port = "7059";
        public static string RestUrl = $"{Scheme}://{LocalhostUrl}:{Port}/api/";

        //public static string Scheme = "https"; // or http
        //public static string RestUrl = $"{Scheme}://gymtrackerapireal20230612173617.azurewebsites.net/api/";
    }
}
