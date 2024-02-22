using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Devenant
{
    public class AvatarManager : Singleton<AvatarManager>
    {
        private Dictionary<string, Sprite> avatars = new Dictionary<string, Sprite>();

        public void Get(string avatar, Action<Sprite> callback)
        {
            if(avatars.ContainsKey(avatar))
            {
                callback?.Invoke(avatars[avatar]);
            }
            else
            {
                UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(ApplicationManager.instance.config.endpoints + "avatars/" + avatar + ".png");

                unityWebRequest.SendWebRequest().completed += (AsyncOperation asyncOperation) =>
                {
                    if(asyncOperation.isDone)
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest);

                        if(texture != null)
                        {
                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                            avatars.Add(avatar, sprite);

                            callback?.Invoke(sprite);
                        }
                    }
                };
            }
        }
    }
}