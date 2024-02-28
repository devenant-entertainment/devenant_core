namespace Devenant
{
    public class AvatarManager : Singleton<AvatarManager>
    {
        public Avatar[] avatars { get { return _avatars; } private set { _avatars = value; } }
        private Avatar[] _avatars;

        public void Setup(AvatarData[] avatars)
        {
            this.avatars = new Avatar[avatars.Length];

            for (int i = 0; i < avatars.Length; i ++)
            {
                string purchase = avatars[i].purchase != null ? avatars[i].purchase.name : string.Empty;
                string achievement = avatars[i].achievement != null ? avatars[i].achievement.name : string.Empty;

                this.avatars[i] = new Avatar(avatars[i].name, avatars[i].sprite, purchase, achievement);
            }
        }
    }
}