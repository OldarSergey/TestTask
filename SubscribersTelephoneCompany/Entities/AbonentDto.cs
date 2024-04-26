using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscribersTelephoneCompany.Entities;



    public class AbonentDto
    {
        public int AbonentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string AddressName { get; set; }
        public string HouseNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }

        public AbonentDto() // Add this constructor
        {
        }

        public AbonentDto(int abonentId, string firstName, string lastName, string middleName, string addressName, string houseNumber, string homePhoneNumber, string workPhoneNumber, string mobilePhoneNumber)
        {
            AbonentId = abonentId;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            AddressName = addressName;
            HouseNumber = houseNumber;
            HomePhoneNumber = homePhoneNumber;
            WorkPhoneNumber = workPhoneNumber;
            MobilePhoneNumber = mobilePhoneNumber;
        }
    }
