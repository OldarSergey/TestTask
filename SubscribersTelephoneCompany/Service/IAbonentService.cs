using SubscribersTelephoneCompany.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscribersTelephoneCompany.Service;

public interface IAbonentService
{
    public List<AbonentDto> GetAbonentListAsync();
} 
