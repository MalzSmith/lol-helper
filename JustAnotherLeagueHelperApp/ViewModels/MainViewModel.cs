using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using JustAnotherLeagueHelperApp.Models;
using JustAnotherLeagueHelperApp.Services;
using PoniLCU;

namespace JustAnotherLeagueHelperApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SummonerService _summonerService;
        private readonly LobbyService _lobbyService;
        private string _apiKey;

        public MainViewModel(SummonerService summonerService, LeagueClient leagueClient, LobbyService lobbyService,
            GameFlowService gameFlowService)
        {
            // This is black magic
            BindingOperations.EnableCollectionSynchronization(AlliedSummoners, this);
            BindingOperations.EnableCollectionSynchronization(EnemySummoners, this);

            ApiKey = "YOUR API KEY HERE";

            _summonerService = summonerService;
            _lobbyService = lobbyService;

            GameflowPhaseChanged(gameFlowService.GameFlowPhase);

            lobbyService.LobbyChanged += LobbyServiceOnLobbyChanged;
            gameFlowService.GameFlowPhaseChanged += GameflowPhaseChanged;
        }

        private void LobbyServiceOnLobbyChanged(List<LobbyMember> obj)
        {
            if (obj is null)
            {
                SetupWithCurrent();
            }
            else
            {
                ResetData();

                SummonerViewModel current = _summonerService.GetCurrentSummoner();
                long team = obj.Find(l => l.Name == current.Name)?.Team ?? 100;


                foreach (var lobbyMember in obj)
                {
                    if (lobbyMember.Team == team)
                    {
                        AlliedSummoners.Add(_summonerService.GetSummonerData(lobbyMember.Name));
                    }
                    else
                    {
                        EnemySummoners.Add(_summonerService.GetSummonerData(lobbyMember.Name));
                    }
                }
            }
        }

        private void SetupWithCurrent()
        {
            ResetData();
            AlliedSummoners.Add(_summonerService.GetCurrentSummoner());
        }

        private void ResetData()
        {
            AlliedSummoners.Clear();
            EnemySummoners.Clear();
        }


        private void GameflowPhaseChanged(string phase)
        {
            switch (phase)
            {
                case "None":
                    SetupWithCurrent();
                    break;
                case "Lobby":
                case "Matchmaking":
                case "ReadyCheck":
                    LobbyServiceOnLobbyChanged(_lobbyService.GetLobbyMembers());
                    // TODO
                    break;
                case "ChampSelect":
                    // TODO
                    break;
                case "GameStart":
                    break;
                case "InProgress":
                    // TODO
                    break;
            }
        }


        public ObservableCollection<SummonerViewModel> AlliedSummoners { get; } = new();

        public ObservableCollection<SummonerViewModel> EnemySummoners { get; } = new();

        public string ApiKey
        {
            get => _apiKey;
            set
            {
                if (value == _apiKey) return;
                _apiKey = value;
                OnPropertyChanged();
            }
        }
    }
}