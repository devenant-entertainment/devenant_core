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
        Banned
    }

    public class User
    {
        public readonly string token;
        public string nickname;
        public string avatar;
        public string email;
        public UserType type;
        public UserStatus status;

        public User(string token, string nickname, string avatar, string email, UserType type, UserStatus status)
        {
            this.token = token;
            this.nickname = nickname;
            this.avatar = avatar;
            this.email = email;
            this.type = type;
            this.status = status;
        }

        public User(UserResponse data)
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
