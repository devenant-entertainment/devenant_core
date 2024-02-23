namespace Devenant
{
    public class Notification
    {
        public readonly string message;
        public readonly Action action;

        public Notification(string message)
        {
            this.message = message;
        }

        public Notification(string message, Action action)
        {
            this.message = message;
            this.action = action;
        }
    }
}
