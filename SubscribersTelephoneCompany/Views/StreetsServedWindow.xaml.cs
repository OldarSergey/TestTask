using SubscribersTelephoneCompany.Entities;
using SubscribersTelephoneCompany.ViewModels;
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

namespace SubscribersTelephoneCompany.Views
{
    /// <summary>
    /// Логика взаимодействия для StreetsServedWindow.xaml
    /// </summary>
    public partial class StreetsServedWindow : Window
    {
        private StreetViewModel _viewModel;
        public StreetsServedWindow(List<StreetDto> streets)
        {
            InitializeComponent();
            _viewModel = new StreetViewModel(streets);
            this.DataContext = _viewModel;
        }
    }
}
