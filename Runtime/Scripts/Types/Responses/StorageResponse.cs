namespace Devenant
{
    [System.Serializable]
    public class StorageResponse
    {
        public Storage[] storages;

        [System.Serializable]
        public class Storage
        {
            public string name;
            public string type;
        }
    }
}
