namespace DesignPatternChallenge.Services.Notification
{
    public interface INotificationService
    {
        void SendOrderConfirmation(string email, string orderId);
        void SendPaymentReceipt(string email, string transactionId);
        void SendShippingNotification(string email, string trackingCode);
    }
}
