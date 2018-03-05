using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace RazvLogins
{
    /// <summary>
    /// Данный тип содержит информацию о сотрудниках и поставщиках, а также выдает ее частями с помощью открытых методов
    /// </summary>
    public interface IManagerAndSups
    {


        /// <summary>
        /// Получение списка поставщиков для указанного сотрудника
        /// </summary>
        /// <param name="managerName">Сотрудник, для которого требуется получить список его поставщиков</param>
        /// <returns></returns>
        IEnumerable<string> GetSupplierList(string managerName);

        /// <summary>
        /// Поиск логина поставщика по названию поставщика
        /// </summary>
        /// <param name="supplierName">Название поставщика</param>
        /// <returns>True, если удалось найти логин и пароль поставщика, иначе false</returns>
        bool TryFindLoginAndPass(string supplierName, out string login, out string password);

        /// <summary>
        /// Получение коллекции имен сотрудников
        /// </summary>
        /// <returns>Коллекция, состоящая из имен сотрудников</returns>
        IEnumerable<string> GetManagersNames();
    }


    /// <summary>
    /// Класс, формирующий и содержащий информацию о сотрудниках и поставщиках
    /// </summary>
    public class ManagersAndSups : IManagerAndSups
    {
        /// <summary>
        /// Информация о сотрудниках и поставщиках. Ключ - сотрудник. Значение - список его поставщиков (объекты класса SupplierInfo)
        /// </summary>
        SortedDictionary<string, List<SupplierInfo>> employeesAndSuppliers2;

        string path = @"\\server\out\Отдел Развития\_INFO_\Поставщики";
        string supsFile = @"WS.xlsx";

        public ManagersAndSups()
        {
            FillEmployeeAndSuppliersInfo();
        }

        public IEnumerable<string> GetSupplierList(string managerName)
        {
            List<string> supplierList = new List<string>();

            if (employeesAndSuppliers2.Keys.Contains(managerName))
            {
                foreach (SupplierInfo supplier in employeesAndSuppliers2[managerName])
                {
                    supplierList.Add(supplier.SupName);
                }
                supplierList.Sort();
                return supplierList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Заполнение информации о сотрудниках и поставщиках
        /// </summary>
        void FillEmployeeAndSuppliersInfo()
        {

            if (!File.Exists(path + "\\" + supsFile))
            {
                throw new FileNotFoundException("Файл с информацией о поставщиках не обнаружен!");
            }

            GetInfoFromExcelFile();
        }


        /// <summary>
        /// Обращение к файлу с необходимой инсормацией и извлечение ее в поле EmployeesAndSuppliers
        /// </summary>
        void GetInfoFromExcelFile()
        {
            var mySuppsFile = new FileStream(path + "\\" + supsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            ExcelPackage eP = new ExcelPackage(mySuppsFile);
            ExcelWorkbook book = eP.Workbook;
            ExcelWorksheet sheet = book.Worksheets[1];

            employeesAndSuppliers2 = new SortedDictionary<string, List<SupplierInfo>>();

            for (int i = 3; i <= sheet.Dimension.End.Row; i++)
            {
                string manager = sheet.Cells[i, 1].Value?.ToString().Trim();
                // необязательный блок кода. Переименовывает конкретных сотрудников, просто для лучшего отображения
                if (manager == "Елена Л./Андрей")
                {
                    manager = "Группа ЛА";
                }
                else if (manager == "Елена К./Екатерина П.")
                {
                    manager = "Группа ЛК";
                }

                string supplierName = sheet.Cells[i, 3].Value?.ToString().Trim();
                string login = sheet.Cells[i, 4].Value?.ToString().Trim();
                string password = sheet.Cells[i, 5].Value?.ToString().Trim();
                if (string.IsNullOrEmpty(password))
                {
                    password = login;
                }


                // Иногда действует один логин на 2-3х поставщиков, поэтому в случае пустой ячейки с логином - пропускаем ее.
                // отдельная кнопка в этом случае не нужна
                if (string.IsNullOrEmpty(login))
                {
                    continue;
                }

                SupplierInfo supplier = new SupplierInfo(supplierName, login, password);



                //TODO переделать дальнейший код, в соответствии с тем, что поставщик теперь будет представлять собой объект
                if (employeesAndSuppliers2.ContainsKey(manager))
                {
                    if (employeesAndSuppliers2[manager] == null)
                    {
                        employeesAndSuppliers2[manager] = new List<SupplierInfo>();
                    }
                    employeesAndSuppliers2[manager].Add(supplier);
                }
                else
                {
                    employeesAndSuppliers2.Add(manager, new List<SupplierInfo>() { { supplier } });
                }

            }
        }

        /// <summary>
        /// Получить коллекцию имен сотрудников
        /// </summary>
        /// <returns>Колекция, состоящая из имен сотрудников</returns>
        public IEnumerable<string> GetManagersNames()
        {
            foreach (var managerName in employeesAndSuppliers2.Keys)
            {
                yield return managerName;
            }
        }

        /// <summary>
        /// Поиск логина поставщика по названию поставщика
        /// </summary>
        /// <param name="supplierName">Название поставщика</param>
        /// <returns></returns>
        public bool TryFindLoginAndPass(string supplierName, out string login, out string password)
        {

            var queryToFindSupplierLogin = from suppliers in employeesAndSuppliers2.Values
                                           from sup in suppliers
                                           where sup.SupName == supplierName
                                           select new { sup.SupLogin, sup.SupPassword };

            login = queryToFindSupplierLogin.FirstOrDefault().SupLogin;
            password = queryToFindSupplierLogin.FirstOrDefault().SupPassword;

            if (login != null && password != null)
            {
                return true;
            }
            return false;
        }



    }

    /// <summary>
    /// Класс, представляющий поставщика
    /// </summary>
    public class SupplierInfo
    {
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public string SupName { get; set; }

        /// <summary>
        /// Логин поставщика
        /// </summary>
        public string SupLogin { get; set; }

        /// <summary>
        /// Пароль поставщика
        /// </summary>
        public string SupPassword { get; set; }

        /// <summary>
        /// Инициализация необходимой информации о поставщике
        /// </summary>
        /// <param name="supName"></param>
        /// <param name="supLogin"></param>
        /// <param name="supPassword"></param>
        public SupplierInfo(string supName, string supLogin, string supPassword)
        {
            SupName = supName;
            SupLogin = supLogin;
            SupPassword = supPassword;
        }
    }

}
