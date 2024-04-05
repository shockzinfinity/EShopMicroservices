namespace Ordering.Application.Dtos;

// Mapster 에서 전부 대문자인 경우 레코드 타입에서 제대로 매핑되지 않음 (Cvv)
public record PaymentDto(string CardName, string CardNumber, string Expiration, string Cvv, int Paymethod);