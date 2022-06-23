using System.Collections.Generic;

namespace JustAnotherLeagueHelperApp.ViewModels
{
    public class RecentGameListViewModel : ViewModelBase
    {
        private List<bool> _games;

        public List<bool> Games
        {
            get => _games;
            set
            {
                if (Equals(value, _games)) return;
                _games = value;
                OnPropertyChanged();
            }
        }

        public bool HasGames => _games?.Count != 0;
    }
}