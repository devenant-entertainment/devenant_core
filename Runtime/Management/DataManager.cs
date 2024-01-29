using UnityEngine;

namespace Devenant
{
    public class DataManager : Singleton<DataManager>
    {
        public ApplicationData applicationData { get { return _applicationData; } }
        [SerializeField] private ApplicationData _applicationData;

        public AvatarData[] avatars { get { return _avatars; } }
        [SerializeField] private AvatarData[] _avatars;

        public PurchaseData[] purchases { get { return _purchases; } }
        [SerializeField] private PurchaseData[] _purchases;
    }
}
