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
        /// <summary>
        /// Строка подключения к БД
        /// P.S. у бэкапа базы другое название: SubscribersTelephoneCompanyDb
        /// </summary>
        public string ConnectionString = @"Server=localhost\SQLExpress;Database=SubscribersTelephoneCompany;Trusted_Connection=True;Encrypt=false";
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }

        /// <summary>
        /// Получение всех необходимый нам полей из таблицы Abonent, а также подгрузка данных из таблиц Address и PhoneNumber
        /// </summary>
        /// <returns></returns>
        public List<AbonentDto> GetAbonentList()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = """
                    SELECT
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
                        LEFT JOIN PhoneNumber pn ON a.Id = pn.AbonentId;
                    """;
                dbConnection.Open();
                return  dbConnection.Query<AbonentDto>(query).ToList();
            }
        }

        
        /// <summary>
        /// Возвращаем список обслуживаемых улиц и количество проживающий там абонентов компании
        /// </summary>
        /// <returns></returns>
        public List<StreetDto> GetStreetAbonent()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query ="""
                    SELECT 
                        s.StreetName as Name,
                        COUNT(a.Id) AS CountAbonent
                    FROM
                        Streets s 
                        LEFT JOIN Address ad ON s.StreetName = ad.Name
                        LEFT JOIN Abonent a ON ad.AbonentId = a.Id
                    GROUP BY
                        s.StreetName
                    ORDER BY
                        s.StreetName;
                    """;
                dbConnection.Open();
                return dbConnection.Query<StreetDto>(query).ToList();
            }
        }


        /// <summary>
        /// данная коллекция нужна для сапостовления английский и русских слов
        /// </summary>
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
        ///     Данный метод позволяет экспортировать данные из ListView в csv формат
        /// </summary>
        /// <param name="lvAbonents">ListView с абонентами</param>
        /// <param name="abonents">список абонентов</param>
        public void ExportToCsv(ListView lvAbonents, ICollection<AbonentDto> abonents)
        {
            if (lvAbonents == null)
            {
                MessageBox.Show("ListView не инициализирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";
            DateTime now = DateTime.Now;
            string fileName = $"report_{now:yyyy-MM-dd_HH_mm}";
            dialog.FileName = fileName;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using var writer = new StreamWriter(dialog.FileName, false, Encoding.UTF8);
                    using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," });

                    var gridView = lvAbonents.View as GridView;
                    if (gridView == null)
                    {
                        MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Записываем заголовки столбцов
                    foreach (var column in gridView.Columns)
                    {
                        csv.WriteField(column.Header.ToString());
                    }
                    csv.NextRecord();

                    // Записываем данные для каждого элемента
                    foreach (var item in abonents)
                    {
                        foreach (var column in gridView.Columns)
                        {
                            var russianHeader = column.Header.ToString();

                            if (propertyMappings.TryGetValue(russianHeader, out var englishPropertyName))
                            {
                                var propertyInfo = typeof(AbonentDto).GetProperty(englishPropertyName);
                                var value = propertyInfo?.GetValue(item) ?? string.Empty;
                                csv.WriteField(value.ToString());
                            }
                            else
                            {
                                csv.WriteField(string.Empty);
                            }
                        }
                        csv.NextRecord();
                    }

                    MessageBox.Show("Экспорт завершен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        public List<AbonentDto> SearchAbonentByNubmer()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = """
                    SELECT 
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
                    """;
                dbConnection.Open();

                return dbConnection.Query<AbonentDto>(query).ToList();
            }
        }
    }
}
