using ECommRepo.Models;
using System;

namespace Observer.Observer
{
    /// <summary>
    /// Registration Class to register the courses
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Delegate Notification Handler to perform the activities post reigstration of courses
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        public delegate void NotificationHandler(NotificationModel notificationModel);
        /// <summary>
        /// Event Registering
        /// </summary>
        public event NotificationHandler Notifying;
        public void AvailableProduct(NotificationModel p)
        {
            Console.WriteLine("Product Successfully added");
            OnProductAddition(p);
        }
        protected virtual void OnProductAddition(NotificationModel p)
        {
            if (Notifying != null)
                Notifying(p);

        }
    }
}
