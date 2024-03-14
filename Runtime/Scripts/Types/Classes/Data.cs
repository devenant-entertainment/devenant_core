namespace Devenant
{
    public class Data
    {
        public readonly string name;

        public Data(DataResponse.Data data)
        {
            name = data.name;
        }
    }
}
