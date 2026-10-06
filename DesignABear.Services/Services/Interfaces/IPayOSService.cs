using Net.payOS.Types;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IPayOSService
    {
        Task<CreatePaymentResult> CreatePaymentLink(PaymentData paymentData);
        Task<PaymentLinkInformation> GetPaymentLinkInformation(long orderCode);
        WebhookData VerifyWebhook(WebhookType webhookBody);
        void ConfirmWebhook(string webhookUrl);
    }
}
