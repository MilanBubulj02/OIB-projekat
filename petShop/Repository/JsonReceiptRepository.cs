using petShop.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace petShop.Repository
{
    public class JsonReceiptRepository : IReceiptRepository
    {
        private const string FilePath = "Data/receipts.json";
        public void Add(Receipt receipt)
        {
            List<Receipt> receipts = GetAll();
            receipts.Add(receipt);
            Save(receipts);
        }
        public List<Receipt> GetAll()
        {
            if (!File.Exists(FilePath))
                return new List<Receipt>();

            string json = File.ReadAllText(FilePath);

            List<Receipt> receipts = JsonSerializer.Deserialize<List<Receipt>>(json);

            if (receipts == null)
                return new List<Receipt>();

            return receipts;
        }

        private void Save(List<Receipt> receipts)
        {
            string json = JsonSerializer.Serialize(receipts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
