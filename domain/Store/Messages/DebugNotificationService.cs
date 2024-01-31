using System.Diagnostics;

namespace Store.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cellphone: {0}, code: {1:0000}.", cellPhone, code);
        }
    }
}
