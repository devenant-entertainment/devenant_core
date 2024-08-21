using System;

namespace Devenant
{
    public class StatusData
    {
        public readonly ApplicationStatus status;
        public readonly Version version;

        public StatusData (StatusResponse response)
        {
            status = Enum.Parse<ApplicationStatus>(response.status);
            version = new Version(response.version);
        }
    }
}
