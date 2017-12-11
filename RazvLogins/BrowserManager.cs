using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace RazvLogins
{
    public interface IBrowserManager
    {
        bool IsQuickLoadNeed { get; set; }
        void RunAndLogin(string login, string pass, string aeURL);
        void CloseBrowserIfRunning();
    }

    class BrowserManager : IBrowserManager
    {
        IWebDriver browser;
        WebDriverWait driverWait;
        public bool IsQuickLoadNeed { get; set; }

        public void CloseBrowserIfRunning()
        {
            if (browser != null)
            {
                browser.Quit();
            }
        }


        /// <summary>
        /// Запуск процесса входа на сайт. Предварительно запуск ChromeDriver, если он еще не запущен.
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="pass">Пароль пользователя</param>
        public void RunAndLogin(string login, string pass, string aeURL)
        {

            Console.WriteLine("Проверка, не открыт ли браузер");
            if (browser == null)
            {
                Console.WriteLine("Создание экземпляра браузера");
                browser = new ChromeDriver();
                browser.Manage().Window.Maximize();
                browser.Navigate().GoToUrl(aeURL);
                //Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                driverWait = new WebDriverWait(browser, TimeSpan.FromSeconds(6));
            }
            else
            {
                try
                {
                    Console.WriteLine("Баузер уже был запущен. Попытка найти элемент для ввода Email");
                    IWebElement email = browser.FindElement(By.Id("email"));
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
                throw ex;
            }

            StartLoadIfNeeded();

        }


        /// <summary>
        /// Открытие окна для выбора файла, если это необходимо (включен чекбокс)
        /// </summary>
        void StartLoadIfNeeded()
        {
            Console.WriteLine("Проверка чекбокса на запрос старта загрузки");
            if (IsQuickLoadNeed)
            {
                try
                {
                    Console.WriteLine("Запрос на загрузку получен");
                    //Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                    // ожидание появления элемента - загрузки СФ
                    IWebElement tryToWait = driverWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("_unit_label")));
                    IWebElement[] goSFs = browser.FindElements(By.ClassName("_unit_label")).ToArray();
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
                    throw ex;
                }
            }
        }


        /// <summary>
        /// Ввод данных пользователя в форму и нажати кнопки "Вход"
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        void EnterLogin(string login, string pass)
        {
            IWebElement email = driverWait.Until(ExpectedConditions.ElementIsVisible(By.Id("email")));
            email.SendKeys(login);
            IWebElement password = browser.FindElement(By.Id("password"));
            password.SendKeys(pass);
            password.Submit();

            //IWebElement enter = Browser.FindElement(By.ClassName("_btn"));
            //enter.Click();
        }


        /// <summary>
        /// Разлогинивание
        /// </summary>
        void LogOut()
        {
            try
            {
                IWebElement settings = browser.FindElement(By.ClassName("_logout_unit"));
                settings.Click();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
