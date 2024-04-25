using Unity.Netcode;
using UnityEngine;

namespace Devenant
{
    [RequireComponent(typeof(Player))]
    public abstract class PlayerComponent : NetworkBehaviour
    {
        public Player player
        {
            get
            {
                if(_player == null)
                {
                    _player = GetComponent<Player>();
                }

                return _player;
            }
        }
        private Player _player;
    }
}
