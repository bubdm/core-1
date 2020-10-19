using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;


namespace WpfApp1.Services
{
    class ShedulerEmailSender
    {
        DispatcherTimer timer = new DispatcherTimer();
        EmailSender emailSender;
        DateTime dtSend;
        IQueryable<Email> emails;
        public ShedulerEmailSender()
        {
        }


        private void Timer_Tick(object sender, EventArgs e)
        {

        }
    }
}
