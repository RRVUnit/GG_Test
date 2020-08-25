using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameRoundFactory
    {
        public static GameRoundModel Create(GameType gameType, Data gameSettings)
        {
            bool withBuffs = gameType == GameType.WITH_BUFFS;
            
            GameRoundModel result = new GameRoundModel();
            for (int i = 0; i < gameSettings.settings.playersCount; i++) {
                PlayerType playerType = GetPlayerTypeByIndex(i);
                PlayerModel player = CreatePlayerModel(gameSettings, playerType, withBuffs); 
                result.AddPlayer(playerType, player);
            }
            return result;
        }

        private static PlayerModel CreatePlayerModel(Data gameSettings, PlayerType playerType, bool withBuffs)
        {
            PlayerModel playerModel = new PlayerModel(playerType, gameSettings.stats);
            if(!withBuffs)
                return playerModel;
            List<Buff> buffs = CreateRandomPlayerBuffs(gameSettings);
            buffs.ForEach(buff => playerModel.AddBuff(buff));
            return playerModel;
        }

        private static List<Buff> CreateRandomPlayerBuffs(Data gameSettings)
        {
            int buffsCount = Random.Range(gameSettings.settings.buffCountMin, gameSettings.settings.buffCountMax);
            if (buffsCount == 0) {
                return new List<Buff>();
            }
            if (gameSettings.settings.allowDuplicateBuffs) {
                return CreateListOfDuplicatedBuffs(buffsCount, gameSettings);
            }
            return CreateListOfUniqueBuffs(buffsCount, gameSettings);
        }

        /*
         * TODO: Сделать тесты если время останется
         */
        
        private static List<Buff> CreateListOfUniqueBuffs(int buffsCount, Data gameSettings)
        {
            Buff[] gameSettingsBuffs = gameSettings.buffs;
            if (gameSettingsBuffs.Length <= buffsCount) {
                return gameSettingsBuffs.ToList();
            }
            Buff[] shuffledBuffs = new Buff[gameSettingsBuffs.Length];
            gameSettingsBuffs.ToList().CopyTo(shuffledBuffs);
            System.Random rnd = new System.Random();
            shuffledBuffs = shuffledBuffs.OrderBy((item) => rnd.Next()).ToArray();
            return shuffledBuffs.ToList().GetRange(0, buffsCount);
        }

        private static List<Buff> CreateListOfDuplicatedBuffs(int buffsCount, Data gameSettings)
        {
            Buff[] gameSettingsBuffs = gameSettings.buffs;
            Buff[] randomBuffs = new Buff[buffsCount];
            for (int i = 0; i < buffsCount; i++) {
                Buff buff = gameSettingsBuffs[Random.Range(0, gameSettingsBuffs.Length - 1)];
                randomBuffs[i] = buff;
            }
            return randomBuffs.ToList();
        }

        /*
         * TODO: Think now to refactor
         */
        
        private static PlayerType GetPlayerTypeByIndex(int index)
        {
            switch (index) {
                case 0:
                    return PlayerType.PLAYER_1;
                case 1:
                default:
                    return PlayerType.PLAYER_2;
            }
        }

        private static PlayerModel CreatePlayerModel(PlayerType playerType, Stat[] initialStats)
        {
            return new PlayerModel(playerType, initialStats);
        }
    }
}