using System;

namespace Devenant
{
    public enum ConfigurationStatus
    {
        Active,
        Inactive
    }

    public class Configuration
    {
        public readonly Version version;
        public readonly ConfigurationStatus status;

        public Configuration(Version version, ConfigurationStatus status)
        {
            this.version = version;
            this.status = status;
        }

        public Configuration (ConfigurationResponse data)
        {
            version = new Version(data.version);
            status = Enum.Parse<ConfigurationStatus>(data.status);
        }
    }
}
