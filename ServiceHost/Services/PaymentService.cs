// فایل: ServiceHost/Services/PaymentService.cs
using System.Threading.Tasks;

public class PaymentService
{
    // سه توکن/merchant تستی برای توسعه محلی
    private readonly string[] _testMerchants = new[] { "TEST_MERCHANT_1", "TEST_MERCHANT_2", "TEST_MERCHANT_3" };

    public Task<(bool Success, string PaymentUrl, string Authority)> CreatePaymentAsync(long orderId, double amount)
    {
        var authority = "TEST_AUTH_" + orderId;
        var url = $"https://sandbox.example.com/pay/{authority}";
        return Task.FromResult((true, url, authority));
    }

    public Task<(bool Success, string RefId)> VerifyPaymentAsync(string authority, double amount)
    {
        return Task.FromResult((true, "TEST_REF_123456"));
    }
}