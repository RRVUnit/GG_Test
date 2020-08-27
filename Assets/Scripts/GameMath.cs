using System;

namespace Game
{
    public class GameMath
    {
        public float CalculateHitAmount(PlayerController playerController, PlayerController enemyController)
        {
            float playerDamage = playerController.PlayerModel.Damage;
            float enemyArmor = enemyController.PlayerModel.Armor;

            float blockedDamage = playerDamage * enemyArmor / 100;
            return playerDamage - blockedDamage;
        }

        public float CalculateRestoreHPAmount(PlayerController playerController, float hitAmount)
        {
            PlayerModel playerModel = playerController.PlayerModel;
            float lifeSteal = playerModel.LifeSteal;
            if (lifeSteal == 0) {
                return 0;
            }
            return (float) Math.Round(hitAmount * lifeSteal / 100);
        }
    }
}