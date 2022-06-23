using System;
using System.Dynamic;
using System.Runtime.Caching;
using JustAnotherLeagueHelperApp.ViewModels;
using PoniLCU;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JustAnotherLeagueHelperApp.Services
{
    public class SummonerService
    {
        private readonly LeagueClient _leagueClient;
        private readonly CacheItemPolicy _policy;

        public SummonerService(LeagueClient leagueClient)
        {
            _leagueClient = leagueClient;
            _policy = new CacheItemPolicy();
            _policy.AbsoluteExpiration = DateTimeOffset.Now + TimeSpan.FromMinutes(2);
        }

        public SummonerViewModel GetSummonerData(string name)
        {
            ObjectCache cache = MemoryCache.Default;
            if (cache[name] is SummonerViewModel result)
            {
                return result;
            }

            result = new SummonerViewModel();
            // TODO
            result.Name = name;
            
            cache.Set(name, result, _policy);

            return result;
        }

        public SummonerViewModel GetCurrentSummoner()
        {
            var text = _leagueClient.Request(LeagueClient.requestMethod.GET, "/lol-summoner/v1/current-summoner").Result;
            dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(text, new ExpandoObjectConverter());
            return data == null ? null :  GetSummonerData(data.displayName);
        }
    }
}