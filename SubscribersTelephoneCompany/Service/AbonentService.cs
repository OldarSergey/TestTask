using CsvHelper.Configuration;
using CsvHelper;
using Dapper;
using SubscribersTelephoneCompany.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace SubscribersTelephoneCompany.Service
{
    public class AbonentService : IAbonentService
    {
        public string ConnectionString = @"Server=oldar;Database=dbSubscribersTelephoneCompany;Trusted_Connection=True;Encrypt=false";
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }

        public List<AbonentDto> GetAbonentList()
        {
            using (IDbConnection dbConnection = Connection)
            {
               var query = @"SELECT 
                                a.Id AS AbonentId,
                                a.FirstName,
                                a.LastName, 
                                a.MiddleName,
                                adr.Name AS AddressName,
                                adr.HouseNumber,
                                pn.HomePhoneNumber, 
                                pn.WorkPhoneNumber, 
                                pn.MobilePhoneNumber 
                            FROM Abonent a 
                                LEFT JOIN Address adr ON a.Id = adr.AbonentId 
                                LEFT JOIN PhoneNumber pn ON a.Id = pn.AbonentId;";
                dbConnection.Open();
                return  dbConnection.Query<AbonentDto>(query).ToList();
            }
        }

        

        public List<StreetDto> GetStreetAbonent()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = @"SELECT
                                s.StreetName as Name,
                                COUNT(a.Id) AS CountAbonent
                            FROM
                                Streets s 
	                            LEFT JOIN Address ad ON s.StreetName = ad.Name
	                            LEFT JOIN Abonent a ON ad.AbonentId = a.Id
                            GROUP BY
                                s.StreetName
                            ORDER BY
                                s.StreetName;";
                dbConnection.Open();
                return dbConnection.Query<StreetDto>(query).ToList();
            }
        }



        readonly Dictionary<string, string> propertyMappings = new Dictionary<string, string>
        {
            {"Имя", "FirstName"},
            {"Фамилия", "LastName"},
            {"Отчество", "MiddleName"},
            {"Улица", "AddressName"},
            {"Номер дома", "HouseNumber"},
            {"Домашний тел.", "HomePhoneNumber"},
            {"Рабочий тел.", "WorkPhoneNumber"},
            {"Мобильный тел.", "MobilePhoneNumber"},
        };

        /// <summary>
        ///     
        /// </summary>
        /// <param name="lvAbonents"></param>
        /// <param name="abonents"></param>
        public void ExportToCsv(ListView lvAbonents, ICollection<AbonentDto> abonents)
        {
            if (lvAbonents == null)
            {
                MessageBox.Show("ListView не инициализирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";
            dialog.FileName = "Abonents";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (var writer = new StreamWriter(dialog.FileName, false, Encoding.UTF8))
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
                    {
                        var gridView = lvAbonents.View as GridView;
                        if (gridView != null)
                        {
                            foreach (var column in gridView.Columns)
                            {
                                csv.WriteField(column.Header.ToString());
                            }
                            csv.NextRecord();

                            foreach (var item in abonents)
                            {
                                foreach (var column in gridView.Columns)
                                {
                                    var russianHeader = column.Header.ToString();

                                    if (propertyMappings.ContainsKey(russianHeader))
                                    {
                                        var englishPropertyName = propertyMappings[russianHeader];
                                        var propertyInfo = typeof(AbonentDto).GetProperty(englishPropertyName);
                                        if (propertyInfo != null)
                                        {
                                            var value = propertyInfo.GetValue(item);
                                            csv.WriteField(value != null ? value.ToString() : "");
                                        }
                                        else
                                        {
                                            csv.WriteField(string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        csv.WriteField(string.Empty);
                                    }
                                }
                                csv.NextRecord();
                            }
                        }
                    }
                    MessageBox.Show("Экспорт завершен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

    }
}
