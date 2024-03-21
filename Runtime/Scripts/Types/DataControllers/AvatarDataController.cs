using System.Collections.Generic;

namespace Devenant
{
    public class AvatarDataController : UnlockableDataController<Avatar, SOAvatar>
    {
        protected override List<Avatar> NormalizeData(SOAvatar[] data)
        {
            List<Avatar> result = new List<Avatar>();

            foreach (SOAvatar avatar in data)
            {
                result.Add(new Avatar(avatar.name, avatar.icon, GetAchievement(avatar), GetPurchase(avatar)));
            }

            return result;
        }
    }
}
