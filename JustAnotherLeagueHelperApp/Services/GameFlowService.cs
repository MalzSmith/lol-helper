using System;
using Newtonsoft.Json.Linq;
using PoniLCU;

namespace JustAnotherLeagueHelperApp.Services
{
    public class GameFlowService
    {
        private const string URL = "/lol-gameflow/v1/gameflow-phase";
        private readonly LeagueClient _leagueClient;

        public GameFlowService(LeagueClient leagueClient)
        {
            _leagueClient = leagueClient;

            _leagueClient.Subscribe(URL, _gameFlowPhaseChanged);
        }

        public event Action<string> GameFlowPhaseChanged;

        private void _gameFlowPhaseChanged(OnWebsocketEventArgs obj)
        {
            var value = ((JValue) obj.Data).Value;
            if (value != null) OnGameFlowPhaseChanged(value.ToString());
        }

        protected virtual void OnGameFlowPhaseChanged(string obj)
        {
            GameFlowPhaseChanged?.Invoke(obj);
        }

        public string GameFlowPhase => string.Join("", _leagueClient.Request(LeagueClient.requestMethod.GET, URL).Result.Split('\"'));
    }
}