using System;
using System.Text.Json.Serialization;


namespace petShop.Model
{
    public class Pet
    {
        public Guid Id { get; private set; }
        public string LatinName { get; private set; }
        public string Name { get; private set; }
        public Species Species { get; private set; }
        public decimal SellingPrice { get; private set; }
        public bool Sold { get; private set; }

        [JsonConstructor]
        public Pet(Guid id, string latinName, string name, Species species, decimal sellingPrice, bool sold)
        {
            Id = id;
            LatinName = latinName;
            Name = name;
            Species = species;
            SellingPrice = sellingPrice;
            Sold = sold;
        }
        public Pet(string latinName, string name, Species species, decimal sellingPrice)
        {
            Id = Guid.NewGuid();
            LatinName = latinName;
            Name = name;
            Species = species;
            SellingPrice = sellingPrice;
            Sold = false;
        }
        public void MarkAsSold() { Sold = true; }
    }
}
