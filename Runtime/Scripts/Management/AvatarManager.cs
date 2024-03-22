using System.Collections.Generic;

namespace Devenant
{
    public class AvatarManager : Singleton<AvatarManager>
    {
        public AssetArray<Avatar> avatars;

        public void Setup(Action callback)
        {
            AssetManager.instance.GetAll((SOAvatar[] soAvatars) =>
            {
                List<Avatar> avatarList = new List<Avatar>();

                foreach(SOAvatar avatar in soAvatars)
                {
                    avatarList.Add(new Avatar(avatar));
                }

                avatars = new AssetArray<Avatar>(avatarList.ToArray());

                callback?.Invoke();
            });
        }

        public Avatar Get(string name)
        {
            foreach(Avatar avatar in avatars.Get())
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