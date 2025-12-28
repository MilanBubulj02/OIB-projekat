using petShop.Repository;
using petShop.Services;

public class DaySalesService : SalesService
{
    public DaySalesService(IReceiptRepository receiptRepository, IPetRepository petRepository, ILogService logService) : base(receiptRepository, petRepository, logService) { }
    protected override decimal CalculateFinalAmount(decimal baseAmount)
    {
        return baseAmount * 0.85m;
    }
}