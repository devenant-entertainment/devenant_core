using System.Linq;
using UnityEngine;

namespace Devenant
{
    public enum ApplicationEnvironment
    {
        Production,
        Development
    }

    public enum ApplicationStatus
    {
        Active,
        Inactive
    }

    public class ApplicationData
    {
        public readonly ApplicationEnvironment environment;

        public readonly string gameUrl;
        public readonly string legalUrl;
        public readonly string supportUrl;
        public readonly string storeUrl;

        public readonly float minInterfaceScale;
        public readonly float maxInterfaceScale;

        public ApplicationData(ApplicationDataAsset data)
        {
            environment = data.environment;

            gameUrl = data.gameUrl;
            legalUrl = data.legalUrl;
            supportUrl = data.supportUrl;
            storeUrl = data.storeUrls.ToList().Find((x) => x.platform == Application.platform).url;

            minInterfaceScale = data.minInterfaceScale;
            maxInterfaceScale = data.maxInterfaceScale;
        }
    }
}
