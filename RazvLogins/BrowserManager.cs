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

    /// <summary>
    /// Класс, выполняющий работу с браузером
    /// </summary>
    class BrowserManager : IBrowserManager
    {
        IWebDriver browser;
        WebDriverWait driverWait;
        public bool IsQuickLoadNeed { get; set; }

        /// <summary>
        /// Закрытие браузера
        /// </summary>
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

            if (browser == null)
            {
                LaunchBrowserAndGoToUrl(aeURL);
            }
            else
            {
                if (IsUserAlreadyLogged())
                {
                    LogOut();
                }
            }

            // вход на сайт
            try
            {
                EnterLogin(login, pass);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Причина", "Ошибка при попытке ввести логин и пароль");
                throw;
            }

            // Если был запрос на загрузку, то сразу открываем окно выбора файла
            StartLoadIfNeeded();

        }

        /// <summary>
        /// Запуск браузера и переход по ссылке, переданной в качестве параметра
        /// </summary>
        /// <param name="aeURL">Адрес страницы, на которую требуется перейти</param>
        private void LaunchBrowserAndGoToUrl(string aeURL)
        {
            browser = new ChromeDriver();
            browser.Manage().Window.Maximize();
            browser.Navigate().GoToUrl(aeURL);
            //Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driverWait = new WebDriverWait(browser, TimeSpan.FromSeconds(6));
        }

        /// <summary>
        /// Проверка, залогинен ли уже пользователь
        /// </summary>
        /// <returns>Возвращает true, если пользователь уже залогинен, в противном случае возвращает false</returns>
        private bool IsUserAlreadyLogged()
        {
            try
            {// Если пользователь уже залогинен (елемент для ввода логина отсутствует), в обработке исключений возвращаем true
                IWebElement email = browser.FindElement(By.Id("email"));
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }


        /// <summary>
        /// Открытие окна для выбора файла, если это необходимо (включен чекбокс)
        /// </summary>
        void StartLoadIfNeeded()
        {
            if (IsQuickLoadNeed)
            {
                try
                {
                    //Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                    // ожидание появления элемента - загрузки СФ
                    IWebElement tryToWait = driverWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("_unit_label")));
                    IWebElement[] findLoadSFTab = browser.FindElements(By.ClassName("_unit_label")).ToArray();
                    foreach (var link in findLoadSFTab)
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
        /// <param name="login">Имя пользователя</param>
        /// <param name="pass">Пароль пользователя</param>
        void EnterLogin(string login, string pass)
        {
            IWebElement email = driverWait.Until(ExpectedConditions.ElementIsVisible(By.Id("email")));
            email.Clear();
            email.SendKeys(login);
            IWebElement password = browser.FindElement(By.Id("password"));
            password.Clear();
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
