using Microsoft.Extensions.DependencyInjection;
using SubscribersTelephoneCompany.Service;
using SubscribersTelephoneCompany.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SubscribersTelephoneCompany
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }
        /// <summary>
        /// Внедряем в наши зависимости интерфейс IAbonentService 
        /// </summary>
        /// <param name="services">необходим для внедрения зависимостей</param>
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<IAbonentService, AbonentService>();

            services.AddSingleton<AbonentWindow>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<AbonentWindow>();
            mainWindow.Show();
        }
    }
}
