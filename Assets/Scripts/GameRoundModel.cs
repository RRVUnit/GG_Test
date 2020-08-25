using System.Collections.Generic;

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
    }
}