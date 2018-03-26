using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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
            foreach (var manager in managersAndSups.GetManagersNames())
            {
                string managerName = manager; // текст, отображаемый на вкладке = имя сотрудника

                // получения списка поставщиков для текущего менеджера, названия которых начинаются с A-Z (eng) и c А (рус)
                var queryForTab1 = managersAndSups.GetSupplierList(managerName)
                    .TakeWhile(name => !name.StartsWith("Б", true, CultureInfo.CurrentCulture));

                // получения списка поставщиков для текущего менеджера, названия которых начинаются с Б-Я (рус)
                var queryForTab2 = managersAndSups.GetSupplierList(managerName)
                    .SkipWhile(name => !name.StartsWith("Б", true, CultureInfo.CurrentCulture));

                // для каждого менеджера создаются две вкладки с поставщиками согласно предыдущим выборкам
                mainForm.CreateTabsAndButtons(managerName + " | eng+А", queryForTab1);
                mainForm.CreateTabsAndButtons(managerName + " | Б-Я", queryForTab2);
            }
        }


        /// <summary>
        /// Метод - обработчик нажатия на кнопку с наименованием поставщика. Поиск логина и пароля по наименованию 
        /// поставщика с последующей передачей их классу BrowserManager для осуществления входа на сайт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="buttonText">Название поставщика (текст кнопки на форме)</param>
        private void MainForm_RunButtonClick(object sender, string buttonText)
        {
            browserManager.IsQuickLoadNeed = mainForm.IsQuickLoadNeed;
            string supplier = buttonText;
            string login;
            string password;

            bool isAllOk = managersAndSups.TryFindLoginAndPass(supplier, out login, out password);
            if (!isAllOk)
            {
                messageManager.ExclamationShow("Имя поставщика не найдено!");
                return;
            }

            Action<string, string, string> runAndLoginMethod = new Action<string, string, string>(browserManager.RunAndLogin);

            // на данный момент у поставщиков логин и пароль совпадают
            var aRslt = runAndLoginMethod.BeginInvoke(login, password, aeURL, null, null);

            try
            {
                runAndLoginMethod.EndInvoke(aRslt);
            }
            catch (Exception ex)
            {
                messageManager.ErrorShow(ex.Data["Причина"] + "\n" + ex.Message);
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
