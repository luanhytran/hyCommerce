using hyCommerce.Domain.Entities.OrderAggregate;

namespace hyCommerce.Application.DTOs;

public class CreateOrderDto
{
    public required ShippingAddress ShippingAddress { get; set; }
    public required PaymentSummary PaymentSummary { get; set; }
}