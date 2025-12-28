using petShop.Model;
using System.Collections.Generic;

namespace petShop.Services
{
    public interface ISalesService
    {
        Receipt SellPet(Pet pet);
        IReadOnlyCollection<Receipt> GetAllReceipts();
    }
}
