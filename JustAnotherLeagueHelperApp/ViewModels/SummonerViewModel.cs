namespace JustAnotherLeagueHelperApp.ViewModels
{
    public class SummonerViewModel : ViewModelBase
    {
        private string _name;
        private RecentGameListViewModel _recentGames;

        public RecentGameListViewModel RecentGames
        {
            get => _recentGames;
            set
            {
                if (Equals(value, _recentGames)) return;
                _recentGames = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }
    }
}