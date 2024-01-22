using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommRepo.Models;

namespace Observer.Observer
{
    public class NotifyUser
    {
        /// <summary>
        /// OnRegistration Method to send the SMS to respective users
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void OnProductAdd(NotificationModel notificationModel)
        {
            Console.WriteLine("Product {0} has been added successfully", notificationModel.Description);
            Console.WriteLine("--------------------------------------");
        }
    }
}
