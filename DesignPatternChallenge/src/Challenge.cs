// DESAFIO: Sistema de E-commerce Complexo
// PROBLEMA: O processo de finalização de pedido envolve múltiplos subsistemas (estoque, pagamento,
// envio, notificação, cupons) cada um com interfaces complexas. O cliente precisa conhecer e
// orquestrar todos esses sistemas, resultando em código complexo e acoplado

using System;
using DesignPatternChallenge.Models;
using DesignPatternChallenge.Services.Coupon;
using DesignPatternChallenge.Services.Inventory;
using DesignPatternChallenge.Services.Notification;
using DesignPatternChallenge.Services.Payment;
using DesignPatternChallenge.Services.Shipping;
using DesignPatternChallenge.src.Facades;

namespace DesignPatternChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de E-commerce ===\n");

            IInventorySystem inventory = new InventorySystem();
            IPaymentGateway payment = new PaymentGateway();
            IShippingService shipping = new ShippingService();
            ICouponSystem coupon = new CouponSystem();
            INotificationService notification = new NotificationService();

            var order = new OrderDTO
            {
                ProductId = "PROD001",
                Quantity = 2,
                CustomerEmail = "cliente@email.com",
                CreditCard = "1234567890123456",
                Cvv = "123",
                ShippingAddress = "Rua Exemplo, 123",
                ZipCode = "12345-678",
                CouponCode = "PROMO10",
                ProductPrice = 100.00m
            };

            var orderFacade = new OrderFacade(coupon, inventory, notification, shipping, payment);

            orderFacade.Process(order);
        }
    }
}
