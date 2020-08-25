using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public enum PlayerType
    {
        PLAYER_1,
        PLAYER_2
    }

    public class PlayerModel
    {
        private readonly PlayerType _playerType;
        private readonly Stat[] _initialStats;

        private List<Buff> _buffs;

        private int _hp;
        
        public PlayerModel(PlayerType playerType, Stat[] initialStats)
        {
            _playerType = playerType;
            _initialStats = initialStats;

            _hp = CollectStat(StatsId.LIFE_ID);
        }

        private int CollectStat(int statId)
        {
            var result = (int) _initialStats.Where(s => s.id == statId).Sum(s => s.value);
            return result;
        }

        public void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
        }

        public void Hit(int hitAmount)
        {
            _hp = Math.Max(0, _hp - hitAmount);
        }

        public bool IsDead()
        {
            return _hp > 0;
        }

        public PlayerType PlayerType
        {
            get { return _playerType; }
        }
    }
}