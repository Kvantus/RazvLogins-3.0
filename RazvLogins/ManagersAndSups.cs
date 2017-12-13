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
    public interface IManagerAndSups
    {
        /// <summary>
        /// Информация о сотрудниках и поставщиках. Ключ - сотрудник. Значение - список его поставщиков и их логинов для сайта
        /// </summary>
        Dictionary<string, SortedDictionary<string, string>> EmployeesAndSuppliers { get; }

        /// <summary>
        /// Получение списка поставщиков
        /// </summary>
        /// <param name="managerName"></param>
        /// <returns></returns>
        IEnumerable<string> GetSupplierList(string managerName);

        /// <summary>
        /// Поиск логина поставщика по названию поставщика
        /// </summary>
        /// <param name="supplierName">Название поставщика</param>
        /// <returns></returns>
        string FindLogin(string supplierName);

        /// <summary>
        /// Заполнение информации о сотрудниках и поставщиках
        /// </summary>
        void FillEmployeeAndSuppliersInfo();
    }

    
    /// <summary>
    /// Класс, формирующий и содержащий информацию о сотрудниках и поставщиках
    /// </summary>
    public class ManagersAndSups : IManagerAndSups
    {
        /// <summary>
        /// Информация о сотрудниках и поставщиках. Ключ - сотрудник. Значение - список его поставщиков и их логинов для сайта
        /// </summary>
        public Dictionary<string, SortedDictionary<string, string>> EmployeesAndSuppliers { get; protected set; }


        string path = @"\\server\out\Отдел Развития\_INFO_\Поставщики";
        string supsFile = @"WS.xlsx";

        public IEnumerable<string> GetSupplierList(string managerName)
        {
            if (EmployeesAndSuppliers.Keys.Contains(managerName))
            {
                return EmployeesAndSuppliers[managerName].Keys; 
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Заполнение информации о сотрудниках и поставщиках
        /// </summary>
        public void FillEmployeeAndSuppliersInfo()
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

            EmployeesAndSuppliers = new Dictionary<string, SortedDictionary<string, string>>();

            for (int i = 3; i <= sheet.Dimension.End.Row; i++)
            {
                string manager = sheet.Cells[i, 1].Value?.ToString().Trim();
                string supplier = sheet.Cells[i, 3].Value?.ToString().Trim();
                string login = sheet.Cells[i, 4].Value?.ToString().Trim();
                // Иногда действует один логин на 2-3х поставщиков, поэтому в случае пустой ячейки с логином - пропускаем ее.
                // отдельная кнопка в этом случае не нужна
                if (string.IsNullOrEmpty(login))
                {
                    continue;
                }

                if (EmployeesAndSuppliers.ContainsKey(manager))
                {
                    if (EmployeesAndSuppliers[manager] == null)
                    {
                        EmployeesAndSuppliers[manager] = new SortedDictionary<string, string>();
                    }
                    EmployeesAndSuppliers[manager].Add(supplier, login);
                }
                else
                {
                    EmployeesAndSuppliers.Add(manager, new SortedDictionary<string, string>() { { supplier, login } });
                }
            }
        }


        /// <summary>
        /// Поиск логина поставщика по названию поставщика
        /// </summary>
        /// <param name="supplierName">Название поставщика</param>
        /// <returns></returns>
        public string FindLogin(string supplierName)
        {
            string login = null;
            foreach (var employee in EmployeesAndSuppliers)
            {
                SortedDictionary<string, string> suppliersAndLogins = employee.Value;
                
                if (suppliersAndLogins.ContainsKey(supplierName))
                {
                    login = suppliersAndLogins[supplierName];
                }
            }

            return login;
        }

    }


}
