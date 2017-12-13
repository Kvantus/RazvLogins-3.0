﻿using System;
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
        SortedDictionary<string, SortedDictionary<string, string>> EmployeesAndSuppliers { get; }

        SortedDictionary<string, List<SupplierInfo>> EmployeesAndSuppliers2 { get; }

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
        bool TryFindLoginAndPass(string supplierName, out string login, out string password);

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
        public SortedDictionary<string, SortedDictionary<string, string>> EmployeesAndSuppliers { get; protected set; }
        public SortedDictionary<string, List<SupplierInfo>> EmployeesAndSuppliers2 { get; protected set; }

        string path = @"\\server\out\Отдел Развития\_INFO_\Поставщики";
        string supsFile = @"WS.xlsx";

        public IEnumerable<string> GetSupplierList(string managerName)
        {
            List<string> supplierList = new List<string>();

            if (EmployeesAndSuppliers2.Keys.Contains(managerName))
            {
                foreach (SupplierInfo supplier in EmployeesAndSuppliers2[managerName])
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

            EmployeesAndSuppliers = new SortedDictionary<string, SortedDictionary<string, string>>();
            EmployeesAndSuppliers2 = new SortedDictionary<string, List<SupplierInfo>>();

            for (int i = 3; i <= sheet.Dimension.End.Row; i++)
            {
                string manager = sheet.Cells[i, 1].Value?.ToString().Trim();
                // необязательный блок кода. Переименовывает конкретных сотрудников, просто для лучшего отображения
                if (manager == "Елена Л./Андрей")
                {
                    manager = "Группа Л-А";
                }
                else if (manager == "Елена К./Екатерина П.")
                {
                    manager = "Группа Л-К";
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
                if (EmployeesAndSuppliers2.ContainsKey(manager))
                {
                    if (EmployeesAndSuppliers2[manager] == null)
                    {
                        EmployeesAndSuppliers2[manager] = new List<SupplierInfo>();
                    }
                    EmployeesAndSuppliers2[manager].Add(supplier);
                }
                else
                {
                    EmployeesAndSuppliers2.Add(manager, new List<SupplierInfo>() { { supplier } });
                }


                //if (EmployeesAndSuppliers.ContainsKey(manager))
                //{
                //    if (EmployeesAndSuppliers[manager] == null)
                //    {
                //        EmployeesAndSuppliers[manager] = new SortedDictionary<string, string>();
                //    }
                //    EmployeesAndSuppliers[manager].Add(supplierName, login);
                //}
                //else
                //{
                //    EmployeesAndSuppliers.Add(manager, new SortedDictionary<string, string>() { { supplierName, login } });
                //}
            }
        }


        /// <summary>
        /// Поиск логина поставщика по названию поставщика
        /// </summary>
        /// <param name="supplierName">Название поставщика</param>
        /// <returns></returns>
        public bool TryFindLoginAndPass(string supplierName, out string login, out string password)
        {

            var queryToFindSupplierLogin = from suppliers in EmployeesAndSuppliers2.Values
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

    public class SupplierInfo
    {
        public string SupName { get; set; }
        public string SupLogin { get; set; }
        public string SupPassword { get; set; }

        public SupplierInfo(string supName, string supLogin, string supPassword)
        {
            SupName = supName;
            SupLogin = supLogin;
            SupPassword = supPassword;
        }
    }

}
