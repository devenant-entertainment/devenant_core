using System.Collections.Generic;

namespace Devenant
{
    public class Achievement
    {
        public Action<Achievement> onProgressed;
        public Action<Achievement> onCompleted;

        public class Info
        {
            public readonly string id;
            public readonly int value;

            public Info(string id, int value)
            {
                this.id = id;
                this.value = value;
            }
        }

        public readonly Info info;

        public int value { get { return _value; } private set { _value = value; } }
        private int _value;

        public Achievement(Info info, int value)
        {
            this.info = info;
            this.value = value;
        }

        public void Set(int value, Action<bool> callback = null)
        {
            if(value > info.value)
            {
                callback?.Invoke(false);

                return;
            }

            if(value <= this.value)
            {
                callback?.Invoke(false);

                return;
            }

            this.value = value;

            onProgressed?.Invoke(this);

            if(this.value == info.value)
            {
                onCompleted?.Invoke(this);
            }

            Dictionary<string, string> formFields = new Dictionary<string, string>()
            {
                {"id", info.id },
                {"value", value.ToString() }
            };

            Request.Post(ApplicationManager.instance.config.endpoints.achievementSet, formFields, UserManager.instance.user.token, (Request.Response response) =>
            {
                callback?.Invoke(response.success);
            });
        }
    }
}
