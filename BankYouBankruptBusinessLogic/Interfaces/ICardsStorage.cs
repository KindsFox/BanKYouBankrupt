using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.Interfaces
{
    public interface ICardsStorage
    {
        List<CardsViewModel> GetFullList();
        List<CardsViewModel> GetFilteredList(CardsBindingModels model);
        CardsViewModel GetElement(CardsBindingModels model);
        void Insert(CardsBindingModels model);
        void Update(CardsBindingModels model);
        void Delete(CardsBindingModels model);
    }
}
