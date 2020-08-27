using System;

namespace Game
{
    public class GameMath
    {
        public float CalculateHitAmount(PlayerController playerController, PlayerController enemyController)
        {
            int playerDamage = playerController.PlayerModel.Damage;
            int enemyArmor = enemyController.PlayerModel.Armor;

            float blockedDamage = (float) Math.Round((float) playerDamage * enemyArmor / 100);
            return playerDamage - blockedDamage;
        }

        public float CalculateRestoreHPAmount(PlayerController playerController, float hitAmount)
        {
            PlayerModel playerModel = playerController.PlayerModel;
            int lifeSteal = playerModel.LifeSteal;
            if (lifeSteal == 0) {
                return 0;
            }
            return (float) Math.Round(hitAmount * lifeSteal / 100);
        }
    }
}