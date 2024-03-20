using System;

namespace Devenant
{
    public class Status
    {
        public readonly ApplicationStatus status;
        public readonly Version version;

        public Status (ConfigurationResponse response)
        {
            status = Enum.Parse<ApplicationStatus>(response.status);
            version = new Version(response.version);
        }
    }
}
