using System;
using DesignPatternChallenge.Models;
using DesignPatternChallenge.Services.Coupon;
using DesignPatternChallenge.Services.Inventory;
using DesignPatternChallenge.Services.Notification;
using DesignPatternChallenge.Services.Payment;
using DesignPatternChallenge.Services.Shipping;

namespace DesignPatternChallenge.src.Facades;

public class OrderFacade
{
    private readonly ICouponSystem _couponSystem;
    private readonly IInventorySystem _inventorySystem;
    private readonly INotificationService _notificationService;
    private readonly IShippingService _shippingService;
    private readonly IPaymentGateway _paymentGateway;

    public OrderFacade(ICouponSystem couponSystem, 
                       IInventorySystem inventorySystem, 
                       INotificationService notificationService, 
                       IShippingService shippingService, 
                       IPaymentGateway paymentGateway)
    {
        _couponSystem = couponSystem;
        _inventorySystem = inventorySystem;
        _notificationService = notificationService;
        _shippingService = shippingService;
        _paymentGateway = paymentGateway;
    }

    public void Process(OrderDTO order)
    {
         try
            {
                if (!_inventorySystem.CheckAvailability(order.ProductId))
                {
                    Console.WriteLine("❌ Produto indisponível");
                    return;
                }

                _inventorySystem.ReserveProduct(order.ProductId, order.Quantity);

                decimal discount = 0;
                if (!string.IsNullOrEmpty(order.CouponCode))
                {
                    if (_couponSystem.ValidateCoupon(order.CouponCode))
                    {
                        discount = _couponSystem.GetDiscount(order.CouponCode);
                    }
                }

                decimal subtotal = order.ProductPrice * order.Quantity;
                decimal discountAmount = subtotal * discount;
                decimal shippingCost = _shippingService.CalculateShipping(order.ZipCode, order.Quantity * 0.5m);
                decimal total = subtotal - discountAmount + shippingCost;

                string transactionId = _paymentGateway.InitializeTransaction(total);

                if (!_paymentGateway.ValidateCard(order.CreditCard, order.Cvv))
                {
                    _inventorySystem.ReleaseReservation(order.ProductId, order.Quantity);
                    Console.WriteLine("❌ Cartão inválido");
                    return;
                }

                if (!_paymentGateway.ProcessPayment(transactionId, order.CreditCard))
                {
                    _inventorySystem.ReleaseReservation(order.ProductId, order.Quantity);
                    Console.WriteLine("❌ Pagamento recusado");
                    return;
                }

                string orderId = $"ORD{DateTime.Now.Ticks}";
                string labelId = _shippingService.CreateShippingLabel(orderId, order.ShippingAddress);
                _shippingService.SchedulePickup(labelId, DateTime.Now.AddDays(1));

                if (!string.IsNullOrEmpty(order.CouponCode))
                {
                    _couponSystem.MarkCouponAsUsed(order.CouponCode, order.CustomerEmail);
                }

                _notificationService.SendOrderConfirmation(order.CustomerEmail, orderId);
                _notificationService.SendPaymentReceipt(order.CustomerEmail, transactionId);
                _notificationService.SendShippingNotification(order.CustomerEmail, labelId);

                Console.WriteLine($"\n✅ Pedido {orderId} finalizado com sucesso!");
                Console.WriteLine($"   Total: R$ {total:N2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao processar pedido: {ex.Message}");
            }
    }
}
