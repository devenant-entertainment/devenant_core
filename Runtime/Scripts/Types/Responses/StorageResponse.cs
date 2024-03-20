namespace Devenant
{
    [System.Serializable]
    public class StorageResponse
    {
        public Data[] datas;

        [System.Serializable]
        public class Data
        {
            public string name;
            public string type;
        }
    }
}
