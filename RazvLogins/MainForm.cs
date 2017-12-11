﻿using System;
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
        void CreateButtons(SortedDictionary<string, string> managers, int tabPageNumber);
    }


    public delegate void SmartButtonEventHandler(object sender, string buttonText);

    public partial class MainForm : Form, IMainForm
    {
        bool isquickloadNeed;
        public bool IsQuickLoadNeed { get { return isquickloadNeed; } }
        public event SmartButtonEventHandler RunButtonClick;
        public event EventHandler BeforeClosingProgram;
        public event EventHandler LoadingMainForm;
        public bool IsMinimizeNeed { get; set; }

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
        }

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

        private void SmartButtonClick(object sender, EventArgs e)
        {
            if (IsMinimizeNeed)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            string buttonText = ((Button)sender).Text;

            RunButtonClick?.Invoke(this, buttonText);
        }

        /// <summary>
        /// Генерация кнопок на форме, разбитых по сотрудникам, с учетом данных из файла Excel
        /// </summary>
        /// <param name="managers">Список сотрудников</param>
        /// <param name="tabPageNumber">Номер вкладки в элементе MultiPage</param>
        public void CreateButtons(SortedDictionary<string, string> managers, int tabPageNumber)
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
        /// Тестовая кнопка, в работе не используется, видна только для разработчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTest_Click(object sender, EventArgs e)
        {

        }


        

    }


}