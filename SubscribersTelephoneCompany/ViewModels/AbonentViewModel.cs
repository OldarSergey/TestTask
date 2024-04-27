using ReactiveUI;
using SubscribersTelephoneCompany.Entities;
using SubscribersTelephoneCompany.Service;
using SubscribersTelephoneCompany.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SubscribersTelephoneCompany.ViewModels
{
    public class AbonentViewModel : ReactiveObject
    {
        private ObservableCollection<AbonentDto> _allAbonents;
        private readonly IAbonentService _abonentService;
        private ListView _lvAbonents;
        private string _searchAbonent;

        public string SearchAbonent
        {
            get => _searchAbonent;
            set
            {
                _searchAbonent = value;
                this.RaisePropertyChanged(nameof(SearchAbonent));
                UpdateList();
            }
        }

        public ObservableCollection<AbonentDto> AllAbonents { get => _allAbonents; set => this.RaiseAndSetIfChanged(ref _allAbonents, value); }

        public AbonentViewModel() {}

        public AbonentViewModel(ListView lvAbonents, IAbonentService abonentService)
        {
            _lvAbonents = lvAbonents;
            _abonentService = abonentService;
            List<AbonentDto> abonents = _abonentService.GetAbonentList();

            AllAbonents = new ObservableCollection<AbonentDto>(abonents);

            MessageBus.Current.Listen<string>().Subscribe(value =>  //подсписываемся на получение сообщения
            {
                // Обновление абонента при получении сообщения
                SearchAbonent = value;
            });

        }

        /// <summary>
        /// сортируем наш список согласно заданым параметрам сортировки и вызываем метод на экспорт в CSV формат
        /// </summary>
        public ReactiveCommand<Object, Unit> ExportToCsvCommand => ReactiveCommand.Create<object>(o =>
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(_lvAbonents.ItemsSource);
            List<AbonentDto> sortedItems = new List<AbonentDto>();

            if (collectionView != null)
            {
                foreach (var item in collectionView)
                {
                    sortedItems.Add((AbonentDto)item);
                }
            }
            _abonentService.ExportToCsv(_lvAbonents, sortedItems);
        });

        /// <summary>
        /// открываем модальное окно, которое содержит ифнормацию о обслуживаемых улицах
        /// Передаем в конструктор список, который содержит коллекцию обслуживаемых улиц и количество проживающих там абонентов компании
        /// </summary>
        public ReactiveCommand<Object, Unit> OpenStreetWindowCommand => ReactiveCommand.Create<object>(o =>
        {
            List<StreetDto> streets = new List<StreetDto>();
            streets = _abonentService.GetStreetAbonent();

            var streetWindow = new StreetsServedWindow(streets);
            streetWindow.Show();

        });


        /// <summary>
        /// открываем модальное окно "Обслуживаемые улицы"
        /// </summary>
        public ReactiveCommand<Object, Unit> OpenSearchWindowCommand => ReactiveCommand.Create<object>(o =>
        {
            var searchWindow = new SearchWindow();
            searchWindow.Show();
        });

        /// <summary>
        /// открываем модальное окно "Поиск"
        /// </summary>
        public ReactiveCommand<Object, Unit> SearchAbonentCommand => ReactiveCommand.Create<object>(o =>
        {
            var searchWindow = new SearchWindow();
            searchWindow.Show();
        });

        /// <summary>
        /// SearchAbonent - критерий поиска. Фильтруем наш список по этому полю и перезаписываем список
        /// </summary>
        private void UpdateList()
        {
            if (_abonentService != null)
            {
                var abonents = _abonentService.GetAbonentList();

                // Проверяем, что список абонентов получен успешно и не равен null
                if (abonents != null)
                {
                    // Фильтруем абонентов по введенному значению
                    var filteredAbonents = abonents.Where(abonent =>
                        (abonent.HomePhoneNumber != null && abonent.HomePhoneNumber.Contains(SearchAbonent)) ||
                        (abonent.WorkPhoneNumber != null && abonent.WorkPhoneNumber.Contains(SearchAbonent)) ||
                        (abonent.MobilePhoneNumber != null && abonent.MobilePhoneNumber.Contains(SearchAbonent)))
                        .ToList();

                    if (filteredAbonents != null && filteredAbonents.Count>0)
                    {
                        AllAbonents = new ObservableCollection<AbonentDto>(filteredAbonents);
                    }
                    else
                    {
                        MessageBox.Show("Нет абонентов, удовлетворяющих критерию поиска", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        AllAbonents = new ObservableCollection<AbonentDto>(abonents);

                    }
                }
            }
        }


    }

}







