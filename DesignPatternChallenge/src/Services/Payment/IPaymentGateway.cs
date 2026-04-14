namespace DesignPatternChallenge.Services.Payment
{
    public interface IPaymentGateway
    {
        string InitializeTransaction(decimal amount);
        bool ValidateCard(string cardNumber, string cvv);
        bool ProcessPayment(string transactionId, string cardNumber);
        void RollbackTransaction(string transactionId);
    }
}
