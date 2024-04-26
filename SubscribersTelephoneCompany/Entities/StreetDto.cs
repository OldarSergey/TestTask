using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscribersTelephoneCompany.Entities
{
    public class StreetDto
    {
        public string Name { get; set; }
        public int CountAbonent { get; set; }


        public StreetDto()
        {
            
        }

        public StreetDto(string name, int countAbonent)
        {
            Name = name;
            CountAbonent = countAbonent;
        }
    }
}
