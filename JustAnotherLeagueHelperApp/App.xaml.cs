using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using JustAnotherLeagueHelperApp.Services;
using JustAnotherLeagueHelperApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using PoniLCU;

namespace JustAnotherLeagueHelperApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Random stolen hack for creating a single exe program

        // Stole this code from https://stackoverflow.com/questions/1025843/merging-dlls-into-a-single-exe-with-wpf
        // I have no idea how it works...
        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            var path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
                path = String.Format(@"{0}\{1}", assemblyName.CultureInfo, path);

            using Stream stream = executingAssembly.GetManifestResourceStream(path);
            if (stream == null) return null;

            var assemblyRawBytes = new byte[stream.Length];
            stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
            return Assembly.Load(assemblyRawBytes);
        }

        #endregion


        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            IServiceProvider provider = CreateServiceProvider();

            Window window = provider.GetRequiredService<MainWindow>();
            window.Show();
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(new LeagueClient(LeagueClient.credentials.cmd));
            services.AddSingleton<SummonerService>();
            services.AddSingleton<LobbyService>();
            services.AddSingleton<GameFlowService>();

            services.AddScoped<MainViewModel>();
            services.AddScoped<MainWindow>();


            return services.BuildServiceProvider();
        }
    }
}