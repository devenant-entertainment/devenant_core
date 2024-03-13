using System.Collections.Generic;

namespace Devenant
{
    public class AvatarDataContent : DataContent<Avatar, SOAvatar>
    {
        protected override Avatar Find(string name)
        {
            return values.Find((x)=> x.name == name);
        }

        protected override List<Avatar> SetupData(SOAvatar[] data)
        {
            List<Avatar> result = new List<Avatar>();

            foreach (SOAvatar avatar in data)
            {
                result.Add(new Avatar(avatar.name, avatar.icon, GetAchievement(avatar.achievement), GetPurchase(avatar.purchase)));
            }

            return result;
        }
    }
}
