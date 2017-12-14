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
    public interface IMainForm
    {
        bool IsQuickLoadNeed { get; }
        event SmartButtonEventHandler RunButtonClick;
        event EventHandler BeforeClosingProgram;
        event EventHandler LoadingMainForm;
        bool IsMinimizeNeed { get; set; }
        void CreateButtons(IEnumerable<string> suppliers, int tabPageNumber);
        void CreateButtons2(string tabName, IEnumerable<string> suppliers);
    }


    public delegate void SmartButtonEventHandler(object sender, string buttonText);

    public partial class MainForm : Form, IMainForm
    {
        // Открытые члены типа MainForm
        bool isquickloadNeed;
        public bool IsQuickLoadNeed { get { return isquickloadNeed; } }
        public event SmartButtonEventHandler RunButtonClick;
        public event EventHandler BeforeClosingProgram;
        public event EventHandler LoadingMainForm;
        public bool IsMinimizeNeed { get; set; }

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            if (Environment.UserName == "viktor_k")
            {
                BTest.Visible = true;
                TTestik.Visible = true;
            }

            Load += MainForm_Load;
            BEnd.Click += BEnd_Click;
            FormClosing += MainForm_BeforeClosingProgram;
            CheckLoad.CheckedChanged += CheckLoad_CheckedChanged;
            CreateToolTip();
        }

        /// <summary>
        /// Создание всплывающей подсказки для чекбокса
        /// </summary>
        void CreateToolTip()
        {
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 6000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 200;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.CheckLoad, "Вкл = После залогинивания сразу запускает окно выбора файла для загрузки");
        }


        /// <summary>
        /// Обработчик события изменения состояния CheckBox, отвечающего за необходимость сразу после входа на сайт начать загрузку файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckLoad_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckLoad.Checked)
            {
                isquickloadNeed = true;
            }
            else
            {
                isquickloadNeed = false;
            }
        }

        /// <summary>
        /// Обработчик события перед закрытием программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_BeforeClosingProgram(object sender, EventArgs e)
        {
            BeforeClosingProgram?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Зактытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadingMainForm?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку запуска процесса входа на сайт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmartButtonClick(object sender, EventArgs e)
        {
            if (IsMinimizeNeed)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            string buttonText = ((Button)sender).Text;

            RunButtonClick?.Invoke(this, buttonText);
        }


        [Obsolete("Метод не используется")]
        /// <summary>
        /// Создание кнопок на заданной вкладке
        /// </summary>
        /// <param name="suppliers">Список названий поставщиков, которые будут присвоены тексту созданных кнопок</param>
        /// <param name="tabPageNumber">Номер вкладки, на которой необходимо создать кнопки</param>
        public void CreateButtons(IEnumerable<string> suppliers, int tabPageNumber)
        {
            if (suppliers == null)
            {
                return;
            }

            int counter = 0;
            int locX = 15;
            int locY = 15;
            foreach (var item in suppliers)
            {
                Button a = new Button();
                a.Size = new Size(300, 30);
                a.Text = item;
                a.Location = new Point(locX, locY);

                a.Click += SmartButtonClick;
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
        /// Создание вкладок и кнопок внутри них
        /// </summary>
        /// <param name="tabName">Имя создаваемой вкладки</param>
        /// <param name="suppliers">Список поставщиков, используемых для отображения текста создаваемых кнопок</param>
        public void CreateButtons2(string tabName, IEnumerable<string> suppliers)
        {
            int counter = 0;
            int locX = 15;
            int locY = 15;

            TabPage myPage = new TabPage(tabName);
            
            SmartMultiPage.TabPages.Add(myPage);


            foreach (var item in suppliers)
            {
                Button a = new Button();
                a.Size = new Size(300, 30);
                a.Text = item;
                a.Location = new Point(locX, locY);

                a.Click += SmartButtonClick;


                myPage.Controls.Add(a);
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
        /// Тестовая кнопка, в работе не используется, видна только для разработчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTest_Click(object sender, EventArgs e)
        {

        }
    }
}
