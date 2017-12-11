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

        string aeURL2 = "http://supplier.autoeuro.ru/login"; // не используется
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
                managersAndSups.GetSups2();
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }

            try
            {
                mainForm.CreateButtons(managersAndSups.DiBrusakova, 0);
                mainForm.CreateButtons(managersAndSups.DiLA, 1);
                mainForm.CreateButtons(managersAndSups.DiLK, 2);
                mainForm.CreateButtons(managersAndSups.DiRizhkova, 3);
                mainForm.CreateButtons(managersAndSups.DiEmbah, 4);
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }
        }


        private void MainForm_RunButtonClick(object sender, string buttonText)
        {
            browserManager.IsQuickLoadNeed = mainForm.IsQuickLoadNeed;
            string supplier = buttonText;
            string login = null;
            if (managersAndSups.DiBrusakova.ContainsKey(supplier))
            {
                login = managersAndSups.DiBrusakova[supplier];
            }
            else if (managersAndSups.DiLA.ContainsKey(supplier))
            {
                login = managersAndSups.DiLA[supplier];
            }
            else if (managersAndSups.DiLK.ContainsKey(supplier))
            {
                login = managersAndSups.DiLK[supplier];
            }
            else if (managersAndSups.DiRizhkova.ContainsKey(supplier))
            {
                login = managersAndSups.DiRizhkova[supplier];
            }
            else if (managersAndSups.DiEmbah.ContainsKey(supplier))
            {
                login = managersAndSups.DiEmbah[supplier];
            }
            else
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
