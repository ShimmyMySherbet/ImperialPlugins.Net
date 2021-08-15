using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Notifications
{
    public class NotificationTypeParseClient
    {
        public ENotificationType GetNotificationType(IPNotification notification)
        {
            if (notification.Type == "purchase.new.merchant")
            {
                return ENotificationType.Sale;
            } else if (notification.Type == "ticket.new.merchant")
            {
                if (notification.MarkdownContent.EndsWith(": Whitelist request"))
                {
                    return ENotificationType.WhitelistRequest;
                } else
                {
                    return ENotificationType.Ticket;
                }


            }
            return ENotificationType.Unknown;
        }



    }
}
