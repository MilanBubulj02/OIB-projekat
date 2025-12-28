using petShop.Model;
using System.Collections.Generic;

namespace petShop.Repository
{
    public interface IPetRepository
    {
        void Add(Pet pet);
        List<Pet> GetAll();
        void Update(Pet pet);
    }
}
