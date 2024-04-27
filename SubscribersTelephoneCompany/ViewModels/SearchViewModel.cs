using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscribersTelephoneCompany.ViewModels
{
    class SearchViewModel : ReactiveObject
    {
        private string _searchAbonent;
        public string SearchAbonent
        {
            get 
            { 
                return _searchAbonent; 
            }
            set 
            { 
                
                _searchAbonent = value;
                MessageBus.Current.SendMessage(value); //На каждое изменение SearchAbonent отправляем сообщение
            }
        }
    }
}
