using petShop.Model;
using petShop.Repository;
using System;
using System.Collections.Generic;

namespace petShop.Services
{
    public abstract class SalesService : ISalesService
    {
        protected readonly IReceiptRepository receiptRepository;
        protected readonly IPetRepository petRepository;
        protected readonly ILogService logService;
        protected SalesService(IReceiptRepository receiptRepository, IPetRepository petRepository, ILogService logService)
        {
            this.receiptRepository = receiptRepository;
            this.petRepository = petRepository;
            this.logService = logService;
        }
        protected abstract decimal CalculateFinalAmount(decimal baseAmount);
        public Receipt SellPet(Pet pet)
        {
            logService.Log(LogType.INFO, "Pokusaj da prodas ljubimca");

            if (Session.CurrentUser == null)
            {
                logService.Log(LogType.ERROR, "Prodaja neuspesna, USER nije ulogovan");
                throw new UnauthorizedAccessException();
            }

            if (Session.CurrentUser.Role != Role.Seller)
            {
                logService.Log(LogType.WARNING, "Prodaja neuspesna, User koji je pokusao prodaju nema role PRODAVAC");
                throw new UnauthorizedAccessException("Samo prodavci mogu da prodaju ljubimce.");
            }

            if (pet.Sold)
            {
                logService.Log(LogType.WARNING, "Prodaja neuspesna, ljubimac je vec prodat.");
                throw new InvalidOperationException("Ljubimac je vec prodat.");
            }

            pet.MarkAsSold();
            petRepository.Update(pet);

            decimal finalAmount = CalculateFinalAmount(pet.SellingPrice);

            Receipt receipt = new Receipt(Session.CurrentUser, finalAmount);
            receiptRepository.Add(receipt);

            logService.Log(LogType.INFO, $"Ljubimac je prodat za {finalAmount}.");

            return receipt;
        }
        public IReadOnlyCollection<Receipt> GetAllReceipts()
        {
            logService.Log(LogType.INFO, $"Pokusaj da dobijes sve racune");

            if (Session.CurrentUser == null)
            {
                logService.Log(LogType.ERROR, "Dobavljanje racuna neuspesno, User nije ulogovan");
                throw new UnauthorizedAccessException();
            }

            if (Session.CurrentUser.Role != Role.Manager)
            {
                logService.Log(LogType.WARNING, "Dobavljanje racuna neuspesno, koji je pokusao dobavljanje nema role MANAGER");
                throw new UnauthorizedAccessException("Samo menager moze da pregleda racune.");
            }

            return receiptRepository.GetAll().AsReadOnly();
        }


    }
}
