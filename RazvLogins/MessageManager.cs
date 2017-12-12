using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RazvLogins
{
    public interface IMessageManager
    {
        void MessageShow(string message);
        void ErrorShow(string message);
        void ExclamationShow(string message);
    }

    /// <summary>
    /// Класс для вывода сообщений пользователю
    /// </summary>
    class MessageManager : IMessageManager
    {
        /// <summary>
        /// Вывод информационного сообщения
        /// </summary>
        /// <param name="message"></param>
        public void MessageShow (string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Вывод сообщения об ошибке
        /// </summary>
        /// <param name="message"></param>
        public void ErrorShow(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Вывод восклицательного сообщения
        /// </summary>
        /// <param name="message"></param>
        public void ExclamationShow(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

    }
}
