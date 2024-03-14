namespace Devenant
{
    [System.Serializable]
    public class DataResponse
    {
        public Data[] datas;

        [System.Serializable]
        public class Data
        {
            public string name;
        }
    }
}
