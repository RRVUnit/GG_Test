namespace Game
{
    public class GameRoundController
    {
        private readonly GameViewContext _gameViewContext;
        private readonly Data _dataConfig;

        private GameRoundModel _currentGameRoundModel;
        
        public GameRoundController(GameViewContext gameViewContext, Data dataConfig)
        {
            _gameViewContext = gameViewContext;
            _dataConfig = dataConfig;
        }

        public void StartRound(GameRoundModel roundModel)
        {
            _currentGameRoundModel = roundModel;

            ResetRound();

            DrawRoundSettings();
        }

        private void DrawRoundSettings()
        {
            
        }

        private void ResetRound()
        {
            
        }
    }
}