namespace Devenant
{
    public class AvatarManager : Singleton<AvatarManager>
    {
        public AvatarDataContent avatars = new AvatarDataContent();

        public void Setup(Action callback)
        {
            avatars.Setup((Avatar[] avatars) =>
            {
                callback?.Invoke();
            });
        }
    }
}