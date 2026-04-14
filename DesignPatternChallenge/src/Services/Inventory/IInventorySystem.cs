namespace DesignPatternChallenge.Services.Inventory
{
    public interface IInventorySystem
    {
        bool CheckAvailability(string productId);
        void ReserveProduct(string productId, int quantity);
        void ReleaseReservation(string productId, int quantity);
    }
}
