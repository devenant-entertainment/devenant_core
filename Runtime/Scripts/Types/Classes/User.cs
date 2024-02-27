using System;

namespace Devenant
{
    public enum UserType
    {
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

    public class User
    {
        public readonly string token;
        public string nickname;
        public string avatar;
        public string email;
        public UserType type;
        public UserStatus status;

        public User(string email, UserResponse data)
        {
            this.email = email;
            token = data.token;
            nickname = data.nickname;
            avatar = data.avatar;
            type = Enum.Parse<UserType>(data.type);
            status = Enum.Parse<UserStatus>(data.status);
        }
    }
}
