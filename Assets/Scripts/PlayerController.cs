using UnityEngine;

namespace Game
{
    public class PlayerController
    {
        private const string HEALTH_PARAM = "Health";
        private const string ATTACK_PARAM = "Attack";
        
        private static readonly int HEALTH = Animator.StringToHash(HEALTH_PARAM);
        private static readonly int ATTACK = Animator.StringToHash(ATTACK_PARAM);
        
        private PlayerType _playerType;
        private readonly Animator _playerAnimator;

        public PlayerController(PlayerType playerType, Animator playerAnimator)
        {
            _playerType = playerType;
            _playerAnimator = playerAnimator;
        }

        public PlayerType PlayerType
        {
            get { return _playerType; }
        }

        public void Hit(int hpAmount)
        {
            
        }

        public void Attack()
        {
            _playerAnimator.SetTrigger(ATTACK);
        }

        public void Die()
        {
            _playerAnimator.SetInteger(HEALTH, 0);
        }
    }
}