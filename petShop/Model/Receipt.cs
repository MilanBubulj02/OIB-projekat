using System;

namespace petShop.Model
{
    public class Receipt
    {
        public Guid Id { get; private set; }
        public User Seller { get; private set; }
        public DateTime DateTimeSale { get; private set; }
        public decimal TotalAmount { get; private set; }

        public Receipt(User seller, decimal totalAmount)
        {
            if (seller.Role != Role.Seller)
                throw new InvalidOperationException("Samo prodavac moze da izda fiskalne racune.");

            Id = Guid.NewGuid();
            Seller = seller;
            DateTimeSale = DateTime.Now;
            TotalAmount = totalAmount;
        }

    }
}
