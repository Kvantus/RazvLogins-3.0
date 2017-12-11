using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using mshtml;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace RazvLogins
{
    public interface IManagerAndSups
    {
        SortedDictionary<string, string> DiBrusakova { get; set; }
        SortedDictionary<string, string> DiLA { get; set; }
        SortedDictionary<string, string> DiLK { get; set; }
        SortedDictionary<string, string> DiRizhkova { get; set; }
        SortedDictionary<string, string> DiEmbah { get; set; }

        void GetSups2();
    }


    /// <summary>
    /// Класс, содержащий SortedDictionary с названием поставщика и логином для каждого сотрудника.
    /// </summary>
    public class ManagersAndSups : IManagerAndSups
    {
        public SortedDictionary<string, string> DiBrusakova { get; set; }
        public SortedDictionary<string, string> DiLA { get; set; }
        public SortedDictionary<string, string> DiLK { get; set; }
        public SortedDictionary<string, string> DiRizhkova { get; set; }
        public SortedDictionary<string, string> DiEmbah { get; set; }

        /// <summary>
        /// Метод не используется
        /// </summary>
        [Obsolete]
        public void GetSups()     // заполняем словари значениями поставщик-логин
        {
            string path = @"\\server\out\Отдел Развития\_INFO_\Макросы";   // откуда берем файл
            string supsFile = @"RazvLoginsButtons.txt";
            if (!File.Exists(path + "\\" + supsFile))   //проверка наличия файла
            {
                MessageBox.Show("Файл с кнопками не обнаружен");
                Environment.Exit(0);
                return;
            }

            DiBrusakova = new SortedDictionary<string, string>();
            DiLA = new SortedDictionary<string, string>();
            DiLK = new SortedDictionary<string, string>();
            DiRizhkova = new SortedDictionary<string, string>();
            DiEmbah = new SortedDictionary<string, string>();


            // считываем каждую строчку и в случае распознавания сотрудника - добавляем ключ-значение в соответствующий словарь
            StreamReader supsReader = new StreamReader(path + "\\" + supsFile, Encoding.GetEncoding(1251));
            while (!supsReader.EndOfStream)
            {
                string currentLine = supsReader.ReadLine();
                string[] splitCurrentLine = currentLine.Split('\t');
                string manager = splitCurrentLine[0];
                string supplier = splitCurrentLine[2];
                string login = splitCurrentLine[3];
                if (string.IsNullOrEmpty(login))
                {
                    continue;
                }
                switch (manager)
                {
                    case "Брусакова Наталья":
                        DiBrusakova.Add(supplier, login);
                        break;

                    case "Елена Л./Андрей ":
                        DiLA.Add(supplier, login);
                        break;
                    case "Елена К./Екатерина П.":
                        DiLK.Add(supplier, login);
                        break;
                    case "Рыжкова Мария":
                        DiRizhkova.Add(supplier, login);
                        break;
                    case "Эмбах Александр":
                        DiEmbah.Add(supplier, login);
                        break;

                    default:
                        continue;
                }
            }

            supsReader.Close();

        }

        /// <summary>
        /// Заполнение словарей всех сотрудников. Ключ - название поставщика, значение - логин. На данный момент логин также используется в качестве пароля
        /// </summary>
        public void GetSups2()
        {
            string path = @"\\server\out\Отдел Развития\_INFO_\Поставщики";
            string supsFile = @"WS.xlsx";
            if (!File.Exists(path + "\\" + supsFile))
            {
                MessageBox.Show("Файл с кнопками не обнаружен\nПрограмма будет закрыта!");
                Environment.Exit(0);
                return;
            }
            var mySuppsFile = new FileStream(path + "\\" + supsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            ExcelPackage eP = new ExcelPackage(mySuppsFile);
            ExcelWorkbook book = eP.Workbook;
            ExcelWorksheet sheet = book.Worksheets[1];

            DiBrusakova = new SortedDictionary<string, string>();
            DiLA = new SortedDictionary<string, string>();
            DiLK = new SortedDictionary<string, string>();
            DiRizhkova = new SortedDictionary<string, string>();
            DiEmbah = new SortedDictionary<string, string>();



            for (int i = 1; i <= sheet.Dimension.End.Row; i++)
            {
                string manager = sheet.Cells[i, 1].Value?.ToString().Trim();
                string supplier = sheet.Cells[i, 3].Value?.ToString().Trim();
                string login = sheet.Cells[i, 4].Value?.ToString().Trim();
                // В на некоторых поставщиков действует один логин, поэтому в случае пустой ячейки с логином - пропускаем ее.
                // отдельная кнопка в этом случае не нужна
                if (string.IsNullOrEmpty(login))
                {
                    continue;
                }
                switch (manager)
                {
                    case "Брусакова Наталья":
                        DiBrusakova.Add(supplier, login);
                        break;

                    case "Елена Л./Андрей":
                        DiLA.Add(supplier, login);
                        break;
                    case "Елена К./Екатерина П.":
                        DiLK.Add(supplier, login);
                        break;
                    case "Рыжкова Мария":
                        DiRizhkova.Add(supplier, login);
                        break;
                    case "Эмбах Александр":
                        DiEmbah.Add(supplier, login);
                        break;

                    default:
                        continue;
                }
            }

        }


    }


}
