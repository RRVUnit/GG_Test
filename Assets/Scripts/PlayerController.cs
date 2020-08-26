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

        private PlayerModel _playerModel;

        public HealthBarViewMediator HealthBar;
        
        public PlayerController(PlayerType playerType, Animator playerAnimator)
        {
            _playerType = playerType;
            _playerAnimator = playerAnimator;
        }

        public PlayerType PlayerType
        {
            get { return _playerType; }
        }

        public void Hit(int hitAmount)
        {
            PlayerModel.Hit(hitAmount);
            
            Debug.Log(PlayerType + " HITTED: HP :" + PlayerModel.HP);
            
            CreateHitAmountLabel(hitAmount);
            UpdateHPPanel();

            if (PlayerModel.IsDead()) {
                PlayDie();
            }
        }

        public void Attack()
        {
            PlayAttack();
        }

        public void RestoreHealth(int hpAmount)
        {
            PlayerModel.RestoreHealth(hpAmount);
            CreateHpRestoreLabel(hpAmount);
            UpdateHPPanel();
        }
        
        private void UpdateHPPanel()
        {
            HealthBar.Value = _playerModel.HP;
        }

        private void CreateHitAmountLabel(int hitAmount)
        {
            HealthBar.EmitHit(hitAmount);
        }
        
        private void CreateHpRestoreLabel(int hpAmount)
        {
            HealthBar.EmitHPRestore(hpAmount);
        }

        private void PlayAttack()
        {
            _playerAnimator.SetTrigger(ATTACK);
        }

        private void PlayDie()
        {
            UpdateHealthAnimationParam();
        }
        
        public PlayerModel PlayerModel
        {
            get { return _playerModel; }
            set
            {
                _playerModel = value;
                UpdateDefaultValuesFromModel();
            }
        }

        private void UpdateDefaultValuesFromModel()
        {
            CreateHpRestoreLabel(_playerModel.HP);

            UpdateHealthAnimationParam();

            HealthBar.MaxValue = _playerModel.HP;
            
            UpdateHPPanel();
        }

        private void UpdateHealthAnimationParam()
        {
            _playerAnimator.SetInteger(HEALTH, _playerModel.HP);
        }
    }
}