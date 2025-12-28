using petShop.Repository;
using petShop.Services;
using System;

public static class SalesServiceFactory
{
    public static ISalesService Create(IReceiptRepository receiptRepository, IPetRepository petRepository, ILogService logService)
    {
        var now = DateTime.Now.TimeOfDay;

        if (now >= new TimeSpan(8, 0, 0) && now < new TimeSpan(16, 0, 0))
            return new DaySalesService(receiptRepository, petRepository, logService);

        if (now >= new TimeSpan(16, 0, 0) && now < new TimeSpan(22, 0, 0))
            return new NightSalesService(receiptRepository, petRepository, logService);

        throw new InvalidOperationException("Radnja je zatvorena.");
    }
}