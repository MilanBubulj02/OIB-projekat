using petShop.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace petShop.Repository
{
    public class JsonPetRepository : IPetRepository
    {
        private const string FilePath = "Data/pets.json";

        public void Add(Pet pet)
        {
            List<Pet> pets = GetAll();
            pets.Add(pet);
            Save(pets);
        }

        public List<Pet> GetAll()
        {
            if (!File.Exists(FilePath))
                return new List<Pet>();

            string json = File.ReadAllText(FilePath);

            List<Pet> pets = JsonSerializer.Deserialize<List<Pet>>(json);

            if (pets == null)
                return new List<Pet>();

            return pets;
        }

        public void Update(Pet pet)
        {
            List<Pet> pets = GetAll();

            int index = pets.FindIndex(p => p.Id == pet.Id);
            if (index == -1)
                throw new InvalidOperationException("Ljubimac nije pronadjen.");

            pets[index] = pet;
            Save(pets);
        }

        private void Save(List<Pet> pets)
        {
            string json = JsonSerializer.Serialize(pets, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(FilePath, json);
        }
    }
}
