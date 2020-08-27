using System;

namespace Game
{
    public class GameMath
    {
        public int CalculateHitAmount(PlayerController playerController, PlayerController enemyController)
        {
            int playerDamage = playerController.PlayerModel.Damage;
            int enemyArmor = enemyController.PlayerModel.Armor;

            int blockedDamage = (int) Math.Round((float) playerDamage * enemyArmor / 100);
            return playerDamage - blockedDamage;
        }

        public int CalculateRestoreHPAmount(PlayerController playerController, int hitAmount)
        {
            PlayerModel playerModel = playerController.PlayerModel;
            int lifeSteal = playerModel.LifeSteal;
            if (lifeSteal == 0) {
                return 0;
            }
            return (int) Math.Round((float) hitAmount * lifeSteal / 100);
        }
    }
}