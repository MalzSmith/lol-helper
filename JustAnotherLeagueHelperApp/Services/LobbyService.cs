using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using JustAnotherLeagueHelperApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PoniLCU;

namespace JustAnotherLeagueHelperApp.Services
{
    public class LobbyService
    {
        private const string Path = "/lol-lobby/v2/lobby/members";
        private readonly LeagueClient _leagueClient;

        public LobbyService(LeagueClient leagueClient)
        {
            _leagueClient = leagueClient;

            _leagueClient.Subscribe("/lol-lobby/v2/lobby/members", LobbyMembersChanged);
        }

        public List<LobbyMember> GetLobbyMembers()
        {
            var text = _leagueClient.Request(LeagueClient.requestMethod.GET, Path).Result;
            return JsonConvert.DeserializeObject<List<LobbyMember>>(text);
        }

        private void LobbyMembersChanged(OnWebsocketEventArgs obj)
        {
            if (obj.Type == "Update")
            {
                var result =
                    JsonConvert.DeserializeObject<List<LobbyMember>>(((JArray) obj.Data).ToString());
                OnLobbyChanged(result);
            }
            else
            {
                OnLobbyChanged(null);
            }
        }

        public event Action<List<LobbyMember>> LobbyChanged;

        protected virtual void OnLobbyChanged(List<LobbyMember> obj)
        {
            LobbyChanged?.Invoke(obj);
        }
    }
}