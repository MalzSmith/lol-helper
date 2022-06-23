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
        private LeagueClient _leagueClient;

        public LobbyService(LeagueClient leagueClient)
        {
            _leagueClient = leagueClient;

            _leagueClient.Subscribe("/lol-lobby/v2/lobby/members", LobbyMembersChanged);
        }

        private void LobbyMembersChanged(OnWebsocketEventArgs obj)
        {
            if (obj.Type == "Update")
            {
                List<dynamic> data = ((JArray) obj.Data).Select(d => JsonConvert.DeserializeObject<ExpandoObject>(d.ToString(), new ExpandoObjectConverter()) as dynamic).ToList();
                List<LobbyMember> result = data
                    .Select(member => new LobbyMember {Name = member.summonerName, Team = member.teamId})
                    .ToList();
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