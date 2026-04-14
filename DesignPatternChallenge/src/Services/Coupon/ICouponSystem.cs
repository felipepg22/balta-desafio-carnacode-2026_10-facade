namespace DesignPatternChallenge.Services.Coupon
{
    public interface ICouponSystem
    {
        bool ValidateCoupon(string code);
        decimal GetDiscount(string code);
        void MarkCouponAsUsed(string code, string customerId);
    }
}
