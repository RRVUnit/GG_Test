namespace Game
{
    public class GameRoundFactory
    {
        public static GameRoundModel Create(GameType gameType, Data gameSettings)
        {
            return new GameRoundModel();
        }
    }
}