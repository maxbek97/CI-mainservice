namespace CI_mainservice.Settings
{
    public class ConnectionStrings
    {
        public DatabaseSection Databases { get; set; }
        public ExternalServicesSection ExtServices { get; set; }

        public class DatabaseSection
        {
            public string MainDB { get; set; }
        }

        public class ExternalServicesSection
        {
            public string AI_service { get; set; }
        }
    }
}
