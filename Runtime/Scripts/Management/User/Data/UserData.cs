using System;

namespace Devenant
{
    public enum UserType
    {
        Guest,
        Player,
        Admin
    }

    public enum UserStatus
    {
        Active,
        Unvalidated,
        Banned,
        Deleted
    }

    public class UserData
    {
        public readonly string token;
        public string nickname;
        public string avatar;
        public string email;
        public UserType type;
        public UserStatus status;

        public UserData(UserDataResponse data)
        {
            token = data.token;
            nickname = data.nickname;
            avatar = data.avatar;
            email = data.email;
            type = Enum.Parse<UserType>(data.type);
            status = Enum.Parse<UserStatus>(data.status);
        }
    }
}
