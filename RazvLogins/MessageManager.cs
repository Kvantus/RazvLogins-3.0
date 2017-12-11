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


    class MessageManager : IMessageManager
    {
        public void MessageShow (string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ErrorShow(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ExclamationShow(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

    }
}
