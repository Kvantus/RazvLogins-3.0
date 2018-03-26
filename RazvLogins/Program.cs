using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RazvLogins
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DateTime myNewDate = new DateTime(2018, 8, 1);
            //DateTime myDate = DateTime.Parse("11.01.2017", System.Globalization.CultureInfo.InvariantCulture);
            if (DateTime.Now > myNewDate)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // инициализация классов, участвующих в программе
            MainForm mainForm = new MainForm();

            ManagersAndSups managersAndSups = new ManagersAndSups();
            MessageManager messageManager = new MessageManager();
            BrowserManager browserManager = null;
            try
            {
                browserManager = new BrowserManager();
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }
            Presenter presenter = new Presenter( browserManager, mainForm, managersAndSups, messageManager);

            Application.Run(mainForm);
        }
    }
}
