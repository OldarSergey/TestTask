using SubscribersTelephoneCompany.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SubscribersTelephoneCompany.Service;

public interface IAbonentService
{
    public List<AbonentDto> GetAbonentList();
    public List<StreetDto> GetStreetAbonent();
    public void ExportToCsv(ListView lvAbonents, ICollection<AbonentDto> abonents);
} 
