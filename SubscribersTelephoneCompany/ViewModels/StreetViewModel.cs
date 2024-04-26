using ReactiveUI;
using SubscribersTelephoneCompany.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscribersTelephoneCompany.ViewModels
{
    public class StreetViewModel : ReactiveObject
    {
        private ObservableCollection<StreetDto> _streets;

        public ObservableCollection<StreetDto> Streets { get => _streets; set => this.RaiseAndSetIfChanged(ref _streets, value); }

        public StreetViewModel()
        {
            
        }
        public StreetViewModel(List<StreetDto> streets)
        {
            Streets = new ObservableCollection<StreetDto>(streets);
        }
    }
    
}
