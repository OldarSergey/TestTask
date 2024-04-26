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
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Shapes;
using SubscribersTelephoneCompany.ViewModels;

namespace SubscribersTelephoneCompany.Views
{
    /// <summary>
    /// Логика взаимодействия для AbonentWindow.xaml
    /// </summary>
    public partial class AbonentWindow : Window
    {
        private AbonentViewModel _viewModel;

        public AbonentWindow()
        {
            InitializeComponent();
            _viewModel = new AbonentViewModel(dgv); // передаем dgv в конструктор AbonentViewModel
            DataContext = _viewModel;
        }

       
    }

}
