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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;






namespace RazvLogins
{


    public partial class Form1 : Form
    {

        public Form1()
        {
            
            InitializeComponent();
            if (Environment.UserName == "viktor_k")
            {
                BTest.Visible = true;
            }
            
        }

        public ManagersSups managers = new ManagersSups();
        public IWebDriver Browser { get; set; }
        WebDriverWait driverWait;
        string aeURL2 = "http://supplier.autoeuro.ru/login"; // не используется
        string aeURL = "http://ws.autoeuro.ru/login";

        /// <summary>
        /// Тестовая кнопка, в работе не используется, видна только для разработчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTest_Click(object sender, EventArgs e)
        {
            string login = "infproject@autoeuro.ru";
            string pass = "infproject@autoeuro.ru";
            RunAndLogin(login, pass);

        }

        /// <summary>
        /// Запуск процесса входа на сайт. Предварительно запуск ChromeDriver, если он еще не запущен.
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="pass">Пароль пользователя</param>
        private void RunAndLogin(string login, string pass)
        {

            Console.WriteLine("Проверка, не открыт ли браузер");
            if (Browser == null)
            {
                Console.WriteLine("Создание экземпляра браузера");
                Browser = new ChromeDriver();
                Browser.Manage().Window.Maximize();
                Browser.Navigate().GoToUrl(aeURL);
                //Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                driverWait = new WebDriverWait(Browser, TimeSpan.FromSeconds(6));
            }
            else
            {
                try
                {
                    Console.WriteLine("Баузер уже был запущен. Попытка найти элемент для ввода Email");
                    IWebElement email = Browser.FindElement(By.Id("email"));
                }
                catch (Exception)
                {
                    Console.WriteLine("Элемент ввода Email не найден, идет разлогинивание");
                    LogOut();
                }

            }

            try
            {
                Console.WriteLine("Попытка ввода логина и пароля");
                EnterLogin(login, pass);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            StartLoadIfNeeded();

        }

        /// <summary>
        /// Открытие окна для выбора файла, если это необходимо (включен чекбокс)
        /// </summary>
        private void StartLoadIfNeeded()
        {
            Console.WriteLine("Проверка чекбокса на запрос старта загрузки");
            if (CheckLoad.Checked)
            {
                try
                {
                    Console.WriteLine("Запрос на загрузку получен");
                    //Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                    // ожидание появления элемента - загрузки СФ
                    IWebElement tryToWait = driverWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("_unit_label")));
                    IWebElement[] goSFs = Browser.FindElements(By.ClassName("_unit_label")).ToArray();
                    foreach (var link in goSFs)
                    {
                        if (link.Text == "Загрузить счет-фактуру")
                        {
                            link.Click();
                            break;
                        }
                    }

                    IWebElement loadButton = driverWait.Until(ExpectedConditions.ElementIsVisible(By.Id("upload_btn")));
                    loadButton.Click();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        [Obsolete]
        private void StartLoadIfNeeded2()
        {
            Console.WriteLine("Проверка чекбокса на запрос старта загрузки");
            if (CheckLoad.Checked)
            {
                try
                {
                    Console.WriteLine("Запрос на загрузку получен");
                    //Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                    IWebElement tryToWait = driverWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("_func_label")));
                    IWebElement[] goSFs = Browser.FindElements(By.ClassName("_func_label")).ToArray();
                    foreach (var link in goSFs)
                    {
                        if (link.Text == "Загрузить счет-фактуру")
                        {
                            link.Click();
                            break;
                        }
                    }

                    IWebElement tryToWait2 = driverWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("_flabel")));
                    IWebElement[] startLoads = Browser.FindElements(By.ClassName("_flabel")).ToArray();
                    foreach (var link in startLoads)
                    {
                        if (link.Text == "Загрузить счет-фактуру")
                        {
                            link.Click();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Разлогинивание
        /// </summary>
        private void LogOut()
        {
            try
            {
                IWebElement settings = Browser.FindElement(By.ClassName("_logout_unit"));
                settings.Click();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [Obsolete]
        /// <summary>
        /// Разлогинивание
        /// </summary>
        private void LogOut2()
        {
            try
            {
                IWebElement settings = Browser.FindElement(By.ClassName("_setting_label"));
                settings.Click();
                IWebElement exit = driverWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("_setting_unit")));
                exit.Click();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Ввод данных пользователя в форму и нажати кнопки "Вход"
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        private void EnterLogin(string login, string pass)
        {
            IWebElement email = driverWait.Until(ExpectedConditions.ElementIsVisible(By.Id("email")));
            email.SendKeys(login);
            IWebElement password = Browser.FindElement(By.Id("password"));
            password.SendKeys(pass);
            password.Submit();

            //IWebElement enter = Browser.FindElement(By.ClassName("_btn"));
            //enter.Click();
        }

        /// <summary>
        /// Закрытие программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEnd_Click(object sender, EventArgs e)
        {
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            this.Close();
        }


        /// <summary>
        /// Загрузка формы с разрисовкой кнопок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            managers.GetSups2();

            CreateButtons(managers.DiBrusakova, 0);
            CreateButtons(managers.DiLA, 1);
            CreateButtons(managers.DiLK, 2);
            CreateButtons(managers.DiRizhkova, 3);
            CreateButtons(managers.DiEmbah, 4);

        }

        /// <summary>
        /// Генерация кнопок на форме, разбитых по сотрудникам, с учетом данных из файла Excel
        /// </summary>
        /// <param name="managers">Список сотрудников</param>
        /// <param name="tabPageNumber">Номер вкладки в элементе MultiPage</param>
        private void CreateButtons(SortedDictionary<string, string> managers, int tabPageNumber)
        {
            int counter = 0;
            int locX = 15;
            int locY = 15;
            foreach (var item in managers)
            {
                Button a = new Button();
                a.Size = new Size(300, 30);
                a.Text = item.Key;
                a.Location = new Point(locX, locY);

                a.Click += new EventHandler(Smart_Click);
                SmartMultiPage.TabPages[tabPageNumber].Controls.Add(a);
                counter++;
                if (counter % 12 == 0)
                {
                    locX += 310;
                    locY = 15;
                }
                else
                {
                    locY += 35;
                }

            }
        }

                /// <summary>
                /// Кнопка, запускающая все необходимые процессы для входа на сайт
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
        private void Smart_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Button button = (Button)sender;
            string supplier = button.Text;
            string login = null;
            if (managers.DiBrusakova.ContainsKey(supplier))
            {
                login = managers.DiBrusakova[supplier];
            }
            else if (managers.DiLA.ContainsKey(supplier))
            {
                login = managers.DiLA[supplier];
            }
            else if (managers.DiLK.ContainsKey(supplier))
            {
                login = managers.DiLK[supplier];
            }
            else if (managers.DiRizhkova.ContainsKey(supplier))
            {
                login = managers.DiRizhkova[supplier];
            }
            else if (managers.DiEmbah.ContainsKey(supplier))
            {
                login = managers.DiEmbah[supplier];
            }
            else
            {
                return;
            }



            Action<string, string> runAndLoginMethod = new Action<string, string>(RunAndLogin);
            runAndLoginMethod.BeginInvoke(login, login, null, null);
        }

        /// <summary>
        /// Действия, выполняемые перед закрытием формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeforeClosing(object sender, FormClosingEventArgs e)
        {
            if (Browser != null)
            {
                Browser.Quit();
            }
        }

    }
}
