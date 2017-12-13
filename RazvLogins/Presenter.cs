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

        string aeURL = "http://ws.autoeuro.ru/login";

        /// <summary>
        /// Констуктор класса Presenter, инициализирует интерфейсные поля, получает информацию о поставщиках и запускает создание кнопок на форме
        /// </summary>
        /// <param name="browserManager">Менеджер управления браузером</param>
        /// <param name="mainForm">Главная форма, взаимодействующая с пользователем</param>
        /// <param name="managersAndSups">Класс, формирующий информацию о менеджерах и поставщиках</param>
        /// <param name="messageManager">Менеджер вывода сообщений пользователю</param>
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
                managersAndSups.FillEmployeeAndSuppliersInfo();
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }

            try
            {
                CreateControls();
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }
        }

        /// <summary>
        /// Выборка имен сотруников и поставщиков для каждого из них с одновременным запуском генерации соответствующих вкладок и кнопок на форме
        /// </summary>
        void CreateControls()
        {
            foreach (var item in managersAndSups.EmployeesAndSuppliers)
            {
                string tabNAme = item.Key; // текст, отображаемый на вкладке = имя сотрудника

                // необязательный блок кода. Переименовывает конкретных сотрудников, просто для лучшего отображения
                if (tabNAme == "")
                {

                }

                IEnumerable<string> suplist = item.Value.Keys;  // список поставщиков, которых ведет текущий сотрудник
                mainForm.CreateButtons2(tabNAme, suplist);

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
                // на данный момент у поставщиков логин и пароль совпадают
                runAndLoginMethod.BeginInvoke(login, login, aeURL, null, null); 
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Message);
            }
        }

        /// <summary>
        /// Перед закрытием програм происходит закрытие браузера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_BeforeClosingProgram(object sender, EventArgs e)
        {
            browserManager.CloseBrowserIfRunning();
        }


    }
}
