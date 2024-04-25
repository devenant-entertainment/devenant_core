namespace Devenant
{
    [System.Serializable]
    public class PlayerNetworkData
    {
        public PlayerType type;
        public string nickname;
        public string avatar;

        public PlayerNetworkData(PlayerType type, string nickname, string avatar)
        {
            this.type = type;
            this.nickname = nickname;
            this.avatar = avatar;
        }
    }
}
