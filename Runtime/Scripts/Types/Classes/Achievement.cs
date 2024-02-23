using System.Collections.Generic;

namespace Devenant
{
    public class Achievement
    {
        public Action<Achievement> onProgressed;
        public Action<Achievement> onCompleted;

        public readonly string id;
        public readonly int maxValue;

        public int value { get { return _value; } private set { _value = value; } }
        private int _value;

        public bool completed
        {
            get 
            {
                return value == maxValue; 
            }
        }

        public Achievement(string id, int maxValue, int value)
        {
            this.id = id;
            this.maxValue = maxValue;
            this.value = value;
        }

        public void Set(int value, Action<bool> callback = null)
        {
            if(value > maxValue)
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

            if(this.value == maxValue)
            {
                onCompleted?.Invoke(this);
            }

            Dictionary<string, string> formFields = new Dictionary<string, string>()
            {
                {"id", id },
                {"value", value.ToString() }
            };

            Request.Post(ApplicationManager.instance.backend.achievementSet, formFields, UserManager.instance.user.token, (Request.Response response) =>
            {
                callback?.Invoke(response.success);
            });
        }
    }
}
