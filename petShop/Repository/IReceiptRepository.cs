using petShop.Model;
using System.Collections.Generic;

namespace petShop.Repository
{
    public interface IReceiptRepository
    {
        void Add(Receipt receipt);
        List<Receipt> GetAll();
    }
}
