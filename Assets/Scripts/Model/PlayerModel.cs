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

        private readonly List<Buff> _buffs = new List<Buff>();

        private float _currentHp;
        
        public PlayerModel(PlayerType playerType, Stat[] initialStats)
        {
            _playerType = playerType;
            _initialStats = initialStats;
        }

        public int CollectStat(int statId)
        {
            var result = (int) _initialStats.Where(s => s.id == statId).Sum(s => s.value);
            _buffs.ForEach(b => {
                result += (int) b.stats.Where(bs => bs.statId == statId).Sum(s => s.value);
            });
            return result;
        }

        public void AddBuffs(Buff[] buffs)
        {
            _buffs.AddRange(buffs);
        }

        public void Start()
        {
            _currentHp = MaxHP;
        }

        public void Hit(float hitAmount)
        {
            _currentHp = Math.Max(0, _currentHp - hitAmount);
        }

        public int MaxHP
        {
            get { return CollectStat(StatsId.LIFE_ID); }
        }
        
        public int Damage
        {
            get { return CollectStat(StatsId.DAMAGE_ID); }
        }
        
        public int Armor
        {
            get { return CollectStat(StatsId.ARMOR_ID); }
        }
        
        public int LifeSteal
        {
            get { return CollectStat(StatsId.LIFE_STEAL_ID); }
        }
        
        public void RestoreHealth(float hpAmount)
        {
            _currentHp = Math.Min(MaxHP, _currentHp + hpAmount);
        }
        
        public bool IsDead()
        {
            return _currentHp <= 0;
        }

        public PlayerType PlayerType
        {
            get { return _playerType; }
        }

        public float HP
        {
            get { return _currentHp; }
        }

        public Buff[] CollectBuffs()
        {
            return _buffs.ToArray();
        }

        public Stat[] CollectStats()
        {
            Stat[] result = new Stat[_initialStats.Length];
            for (int i = 0; i < _initialStats.Length; i++) {
                Stat initialStat = CloneStat(_initialStats[i]);
                initialStat.value = CollectStat(initialStat.id);
                result[i] = initialStat;
            }
            return result;
        }

        private Stat CloneStat(Stat stat)
        {
            return new Stat() {
                    id = stat.id,
                    title = stat.title,
                    icon = stat.icon,
                    value = stat.value
            }; 
        }

        public bool HasStat(int statId)
        {
            return CollectStat(statId) > 0;
        }
    }
}