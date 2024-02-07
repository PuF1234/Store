using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace Store.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cellphone: {0}, code: {1:0000}.", cellPhone, code);
        }

        public void StartProcess(Order order)
        {
            using (var client = new SmtpClient())
            {
                var message = new MailMessage("from@at.my.domain", "to@at.my.domain");
                message.Subject = "Order #" + order.Id;

                var builder = new StringBuilder();
                var s = "";
                foreach(var item in order.Items) 
                { 
                    builder.Append("{0}, {1}", item.BicycleId, item.Count);
                    s += string.Format("0{}");
                    builder.AppendLine();
                }
            }
        }
    }
}
