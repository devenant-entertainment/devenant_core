using UnityEngine;
using UnityEngine.Networking;

namespace Devenant
{
    public class AvatarManager : Singleton<AvatarManager>
    {
        public void Setup(Action action)
        {

        }

        public void Get(string avatar, Action<Sprite> callback)
        {
            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(Application.config.coreApiUrl + "avatars/" + avatar + ".png");

            unityWebRequest.SendWebRequest().completed += (AsyncOperation asyncOperation) =>
            {
                if(asyncOperation.isDone)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest);

                    if(texture != null) 
                    {
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                        callback?.Invoke(sprite);
                    }
                }
            };
        }
    }
}
