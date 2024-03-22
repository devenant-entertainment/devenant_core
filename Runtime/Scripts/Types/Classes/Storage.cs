namespace Devenant
{
    public class Storage
    {
        public readonly string name;
        public readonly string type;

        public Storage (string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        public Storage(StorageResponse.Storage storage)
        {
            name = storage.name;
            type = storage.type;
        }
    }
}
