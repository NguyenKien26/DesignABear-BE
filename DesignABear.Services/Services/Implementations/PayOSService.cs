using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class PayOSService : IPayOSService
    {
        private readonly PayOS _payOS;

        public PayOSService(IConfiguration configuration)
        {
            _payOS = new PayOS(
                configuration["PayOS:Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Missing PAYOS_CLIENT_ID"),
                configuration["PayOS:Environment:PAYOS_API_KEY"] ?? throw new Exception("Missing PAYOS_API_KEY"),
                configuration["PayOS:Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Missing PAYOS_CHECKSUM_KEY"),
                configuration["PayOS:Environment:PAYOS_PARTNER_CODE"] ?? ""
            );
        }

        public async Task<CreatePaymentResult> CreatePaymentLink(PaymentData paymentData)
        {
            return await _payOS.createPaymentLink(paymentData);
        }

        public async Task<PaymentLinkInformation> GetPaymentLinkInformation(long orderCode)
        {
            return await _payOS.getPaymentLinkInformation(orderCode);
        }

        public WebhookData VerifyWebhook(WebhookType webhookBody)
        {
            return _payOS.verifyPaymentWebhookData(webhookBody);
        }

        public void ConfirmWebhook(string webhookUrl)
        {
            _payOS.confirmWebhook(webhookUrl);
        }
    }
}
