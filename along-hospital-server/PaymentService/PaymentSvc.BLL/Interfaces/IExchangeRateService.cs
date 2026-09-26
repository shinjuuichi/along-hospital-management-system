namespace PaymentSvc.BLL.Interfaces
{
    public interface IExchangeRateService
    {
        Task<decimal> GetExchageRate(string from, string to);
    }
}