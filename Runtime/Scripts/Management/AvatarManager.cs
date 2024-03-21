namespace Devenant
{
    public class AvatarManager : Singleton<AvatarManager>
    {
        public Avatar[] avatars { get { return _avatars; } private set { _avatars = value; } }
        private Avatar[] _avatars;

        public void Setup(Action callback)
        {
            DataManager.instance.avatarDataController.Get((Avatar[] avatars) =>
            {
                this.avatars = avatars;

                callback?.Invoke();
            });
        }

        public Avatar Get(string name)
        {
            foreach(Avatar avatar in avatars)
            {
                if (name == avatar.name)
                {
                    return avatar;
                }
            }

            return null;
        }
    }
}