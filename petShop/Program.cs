using petShop.Model;
using petShop.Repository;
using petShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        IPetRepository petRepository = new JsonPetRepository();
        IReceiptRepository receiptRepository = new JsonReceiptRepository();

        ILogService logService = new FileLogService();

        IPetService petService = new PetService(petRepository, logService);

        ISalesService salesService;

        try
        {
            salesService = SalesServiceFactory.Create(receiptRepository, petRepository, logService);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Radno vreme: 08–22");
            Console.ReadKey();
            return;
        }

        List<User> users = new List<User>
            {
                new User("MenagerStefan", "manager", "Stefan", "Stefanov", Role.Manager),
                new User("MenagerDejan", "manager2", "Dejan", "Satara", Role.Manager),
                new User("ProdavacMilan", "prodavac", "Milan", "Tripkovic", Role.Seller),
                new User("ProdavacAleksa", "prodavac2", "Aleksa", "Aleksic", Role.Seller)
            };

        IAuthService authService = new AuthService(users);


        while (true)
        {
            Console.Clear();
            Console.WriteLine("--===== LOGIN =====--");
            Console.WriteLine("X. Exit");

            Console.Write("Username: ");
            string username = Console.ReadLine();
            if (username.Equals("X", StringComparison.OrdinalIgnoreCase))
                return;

            Console.Write("Password: ");
            string password = Console.ReadLine();

            try
            {
                authService.Login(username, password);

                if (Session.CurrentUser.Role == Role.Manager)
                    ManagerMenu(petService, salesService);
                else
                    SellerMenu(petService, salesService);

                authService.Logout();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

    }

    static void ManagerMenu(IPetService petService, ISalesService salesService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--===== Menager meni =====--");
            Console.WriteLine("1. Dodaj ljubimca");
            Console.WriteLine("2. Izlistaj sve ljubimce");
            Console.WriteLine("3. Izlistaj sve racune");
            Console.WriteLine("0. Logout");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Latinsko ime: ");
                        string latin = Console.ReadLine();

                        Console.Write("Ime: ");
                        string name = Console.ReadLine();

                        Species species;
                        while (true)
                        {
                            Console.Write("Vrsta (0 - Sisar, 1 - Gmizavac, 2 - Glodar): ");
                            string input = Console.ReadLine();

                            if (int.TryParse(input, out int value) && Enum.IsDefined(typeof(Species), value))
                            {
                                species = (Species)value;
                                break;
                            }

                            Console.WriteLine("Invalidna vrsta. Unesite 0, 1 ili 2 da bi odabrali vrstu.");
                        }

                        Console.Write("Price: ");
                        decimal price;
                        while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
                        {
                            Console.WriteLine("Invalidna cena. Cena mora da bude pozitivan broj:");
                        }


                        petService.AddPet(new Pet(latin, name, species, price));
                        Console.WriteLine("Ljubimac dodat.");
                        break;

                    case "2":
                        List<Pet> pets = petService.GetAllPets().ToList();

                        if (!pets.Any())
                        {
                            Console.WriteLine("Nema dostupnih ljubimaca.");
                        }
                        else
                        {
                            foreach (Pet p in pets)
                                Console.WriteLine($"{p.Name} - {p.Species} - Sold: {p.Sold}");
                        }
                        break;

                    case "3":
                        List<Receipt> receipts = salesService.GetAllReceipts().ToList();

                        if (!receipts.Any())
                        {
                            Console.WriteLine("Nema dostupnih racuna.");
                        }
                        else
                        {
                            foreach (Receipt r in salesService.GetAllReceipts())
                                Console.WriteLine($"{r.Seller.Name} {r.Seller.Surname} | {r.TotalAmount} | {r.DateTimeSale}");
                        }
                        break;

                    case "0":
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
    static void SellerMenu(IPetService petService, ISalesService salesService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--===== Meni Prodavca =====--");
            Console.WriteLine("1. Pogledaj dostupne ljubimce");
            Console.WriteLine("2. Prodaj ljubimca");
            Console.WriteLine("0. Logout");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        List<Pet> availablePets = petService.GetAvailablePets().ToList();

                        if (!availablePets.Any())
                        {
                            Console.WriteLine("Nema dostupnih ljubimaca.");
                            break;
                        }

                        Console.WriteLine("Dostupni ljubimci:");
                        for (int i = 0; i < availablePets.Count; i++)
                        {
                            Console.WriteLine($"{availablePets[i].Name} - {availablePets[i].SellingPrice}");
                        }
                        break;

                    case "2":
                        List<Pet> pets = petService.GetAvailablePets().ToList();

                        if (!pets.Any())
                        {
                            Console.WriteLine("Nema dostupnih ljubimaca.");
                            break;
                        }

                        Console.WriteLine("Dostupni ljubimci:");
                        for (int i = 0; i < pets.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {pets[i].Name} - {pets[i].SellingPrice}");
                        }

                        Console.Write("Izaberite ljubinca za prodaju: ");
                        if (!int.TryParse(Console.ReadLine(), out int choiceNumber))
                        {
                            Console.WriteLine("Invalidan unos.");
                            break;
                        }

                        if (choiceNumber < 1 || choiceNumber > pets.Count)
                        {
                            Console.WriteLine("Broj izvan opsega.");
                            break;
                        }

                        Pet pet = pets[choiceNumber - 1];
                        Receipt receipt = salesService.SellPet(pet);

                        Console.WriteLine($"Sold {pet.Name} for {receipt.TotalAmount}");
                        break;

                    case "0":
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}