using petShop.Model;
using petShop.Repository;
using petShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;

public class PetService : IPetService
{
    private const int MaxPets = 10;

    private readonly IPetRepository petRepository;
    private readonly ILogService logService;

    public PetService(IPetRepository petRepository, ILogService logService)
    {
        this.petRepository = petRepository;
        this.logService = logService;
    }

    public void AddPet(Pet pet)
    {
        logService.Log(LogType.INFO, "Pokusaj da dodas ljubimca.");

        if (Session.CurrentUser == null)
        {
            logService.Log(LogType.ERROR, "Dodavanje ljubimca neuspesno. Nedozvoljen pristup, USER nije ulogovan.");
            throw new UnauthorizedAccessException();
        }

        if (Session.CurrentUser.Role != Role.Manager)
        {
            logService.Log(LogType.WARNING, "Nedozvoljen pristup, USER koji nije MANAGER je pokusao da doda ljubimca.");
            throw new UnauthorizedAccessException("Samo menager moze da doda ljubimca.");
        }

        List<Pet> pets = petRepository.GetAll();
        if (pets.Count >= MaxPets)
        {
            logService.Log(LogType.WARNING, "Prekoracen kapacitet radnje.");
            throw new InvalidOperationException("Sva mesta u radnji su puna.");
        }

        petRepository.Add(pet);
        logService.Log(LogType.INFO, $"Ljubimac dodat: {pet.Name}.");
    }

    public IReadOnlyCollection<Pet> GetAllPets()
    {
        logService.Log(LogType.INFO, "Pokusaj da izlistas sve ljubimce.");
        if (Session.CurrentUser == null)
        {
            logService.Log(LogType.ERROR, "Dobavljane liste ljubinaca neuspesno, USER nije ulogovan.");
            throw new UnauthorizedAccessException();
        }

        return petRepository.GetAll().AsReadOnly();
    }

    public IReadOnlyCollection<Pet> GetAvailablePets()
    {
        logService.Log(LogType.INFO, "Pokusaj da izlistas sve dostupne ljubimce.");

        if (Session.CurrentUser == null)
        {
            logService.Log(LogType.ERROR, "Dobavljane liste dostupnih ljubinaca neuspesno, USER nije ulogovan.");
            throw new UnauthorizedAccessException();
        }

        return petRepository.GetAll().Where(p => !p.Sold).ToList().AsReadOnly();
    }
}