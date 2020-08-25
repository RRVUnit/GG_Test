using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class GameRoundModel
    {
        private Dictionary<PlayerType, PlayerModel> _players;

        public void AddPlayer(PlayerType playerType, PlayerModel playerModel)
        {
            _players.Add(playerType, playerModel);
        }

        public PlayerModel GetPlayerByType(PlayerType playerType)
        {
            if (!_players.ContainsKey(playerType)) {
                return null;
            }
            return _players[playerType];
        }

        public bool IsGameOver()
        {
            return _players.Values.Any(p => p.IsDead());
        }
    }
}