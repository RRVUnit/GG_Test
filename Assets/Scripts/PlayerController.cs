using System;
using UnityEngine;
using UnityEngine.UI;

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

        private HealthBarEmitter _healthBarEmitter;
        
        public PlayerController(PlayerType playerType, Animator playerAnimator)
        {
            _playerType = playerType;
            _playerAnimator = playerAnimator;
            
            _healthBarEmitter = new HealthBarEmitter();
        }

        public PlayerType PlayerType
        {
            get { return _playerType; }
        }

        public void Hit(float hitAmount)
        {
            PlayerModel.Hit(hitAmount);
            
            CreateHitAmountLabel(ToInt(hitAmount).ToString());
            UpdateHPPanel();

            if (PlayerModel.IsDead()) {
                PlayDie();
            }
        }

        public void Attack()
        {
            PlayAttack();
        }

        public void RestoreHealth(float hpAmount)
        {
            PlayerModel.RestoreHealth(hpAmount);
            CreateHpRestoreLabel(ToInt(hpAmount).ToString());
            UpdateHPPanel();
        }
        
        private void UpdateHPPanel()
        {
            HealthBar.Value = HPToInt();
        }

        private void CreateHitAmountLabel(string hitAmount)
        {
            Text hitText = HealthBar.CreateHitText(hitAmount);
            _healthBarEmitter.Add(hitText);
        }
        
        private void CreateHpRestoreLabel(string hpAmount)
        {
            Text hitText = HealthBar.CreateHPRestoreText(hpAmount);
            _healthBarEmitter.Add(hitText);
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
            CreateHpRestoreLabel(_playerModel.MaxHP.ToString());

            UpdateHealthAnimationParam();

            HealthBar.MaxValue = _playerModel.MaxHP;
            
            UpdateHPPanel();
        }

        private void UpdateHealthAnimationParam()
        {
            _playerAnimator.SetInteger(HEALTH, HPToInt());
        }

        private int HPToInt()
        {
            return ToInt(_playerModel.HP);
        }

        private int ToInt(float amount)
        {
            return (int) Math.Round(amount);
        }

        public void Tick()
        {
            _healthBarEmitter.Tick();
        }
    }
}