using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace RazvLogins
{
    class Presenter
    {
        readonly IBrowserManager browserManager;
        readonly IMainForm mainForm;
        readonly IManagerAndSups managersAndSups;
        readonly IMessageManager messageManager;

        [Obsolete ("не используется")] string aeURL2 = "http://supplier.autoeuro.ru/login"; // не используется
        string aeURL = "http://ws.autoeuro.ru/login";

        public Presenter(IBrowserManager browserManager, IMainForm mainForm, IManagerAndSups managersAndSups, IMessageManager messageManager)
        {
            this.browserManager = browserManager ?? throw new ArgumentNullException(nameof(browserManager));
            this.mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
            this.managersAndSups = managersAndSups ?? throw new ArgumentNullException(nameof(managersAndSups));
            this.messageManager = messageManager ?? throw new ArgumentNullException(nameof(messageManager));

            mainForm.BeforeClosingProgram += MainForm_BeforeClosingProgram;
            mainForm.IsMinimizeNeed = true;
            mainForm.RunButtonClick += MainForm_RunButtonClick;

            try
            {
                managersAndSups.GetSups();
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }

            try
            {
                mainForm.CreateButtons(managersAndSups.GetSupplierList("Брусакова Наталья"), 0);
                mainForm.CreateButtons(managersAndSups.GetSupplierList("Елена Л./Андрей"), 1);
                mainForm.CreateButtons(managersAndSups.GetSupplierList("Елена К./Екатерина П."), 2);
                mainForm.CreateButtons(managersAndSups.GetSupplierList("Рыжкова Мария"), 3);
                mainForm.CreateButtons(managersAndSups.GetSupplierList("Эмбах Александр"), 4);
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }
        }

        /// <summary>
        /// Запуск процесса входа на сайт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="buttonText">Название поставщика (текст кнопки на форме)</param>
        private void MainForm_RunButtonClick(object sender, string buttonText)
        {
            browserManager.IsQuickLoadNeed = mainForm.IsQuickLoadNeed;
            string supplier = buttonText;
            string login = managersAndSups.FindLogin(supplier);
            if (login == null)
            {
                messageManager.ExclamationShow("Имя поставщика не найдено!");
                return;
            }

            Action<string, string, string> runAndLoginMethod = new Action<string, string, string>(browserManager.RunAndLogin);
            try
            {
                runAndLoginMethod.BeginInvoke(login, login, aeURL, null, null);
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }
        }

        private void MainForm_BeforeClosingProgram(object sender, EventArgs e)
        {
            browserManager.CloseBrowserIfRunning();
        }


    }
}
