using petShop.Model;
using System.Collections.Generic;

namespace petShop.Services
{
    public interface IPetService
    {
        void AddPet(Pet pet);
        IReadOnlyCollection<Pet> GetAllPets();
        IReadOnlyCollection<Pet> GetAvailablePets();
    }
}
