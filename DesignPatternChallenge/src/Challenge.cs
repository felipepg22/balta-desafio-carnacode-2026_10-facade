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

            Console.WriteLine("=== Processando Pedido (Código Complexo) ===\n");

            try
            {
                if (!inventory.CheckAvailability(order.ProductId))
                {
                    Console.WriteLine("❌ Produto indisponível");
                    return;
                }

                inventory.ReserveProduct(order.ProductId, order.Quantity);

                decimal discount = 0;
                if (!string.IsNullOrEmpty(order.CouponCode))
                {
                    if (coupon.ValidateCoupon(order.CouponCode))
                    {
                        discount = coupon.GetDiscount(order.CouponCode);
                    }
                }

                decimal subtotal = order.ProductPrice * order.Quantity;
                decimal discountAmount = subtotal * discount;
                decimal shippingCost = shipping.CalculateShipping(order.ZipCode, order.Quantity * 0.5m);
                decimal total = subtotal - discountAmount + shippingCost;

                string transactionId = payment.InitializeTransaction(total);

                if (!payment.ValidateCard(order.CreditCard, order.Cvv))
                {
                    inventory.ReleaseReservation(order.ProductId, order.Quantity);
                    Console.WriteLine("❌ Cartão inválido");
                    return;
                }

                if (!payment.ProcessPayment(transactionId, order.CreditCard))
                {
                    inventory.ReleaseReservation(order.ProductId, order.Quantity);
                    Console.WriteLine("❌ Pagamento recusado");
                    return;
                }

                string orderId = $"ORD{DateTime.Now.Ticks}";
                string labelId = shipping.CreateShippingLabel(orderId, order.ShippingAddress);
                shipping.SchedulePickup(labelId, DateTime.Now.AddDays(1));

                if (!string.IsNullOrEmpty(order.CouponCode))
                {
                    coupon.MarkCouponAsUsed(order.CouponCode, order.CustomerEmail);
                }

                notification.SendOrderConfirmation(order.CustomerEmail, orderId);
                notification.SendPaymentReceipt(order.CustomerEmail, transactionId);
                notification.SendShippingNotification(order.CustomerEmail, labelId);

                Console.WriteLine($"\n✅ Pedido {orderId} finalizado com sucesso!");
                Console.WriteLine($"   Total: R$ {total:N2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao processar pedido: {ex.Message}");
            }

            Console.WriteLine("\n=== PROBLEMAS ===");
            Console.WriteLine("✗ Cliente precisa conhecer 5 subsistemas diferentes");
            Console.WriteLine("✗ Código complexo com muitos passos interdependentes");
            Console.WriteLine("✗ Alto acoplamento entre cliente e subsistemas");
            Console.WriteLine("✗ Lógica de negócio espalhada no código cliente");
            Console.WriteLine("✗ Difícil garantir consistência e tratamento de erros");
            Console.WriteLine("✗ Código repetido em diferentes pontos da aplicação");
        }
    }
}
