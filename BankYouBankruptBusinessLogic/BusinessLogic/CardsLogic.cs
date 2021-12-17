using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BankYouBankruptBusinessLogic.BusinessLogic
{
    public class CardsLogic
    {
        private readonly ICardsStorage _cardsStorage;
        public CardsLogic(ICardsStorage cardsStorage)
        {
            _cardsStorage = cardsStorage;
        }
        public List<CardsViewModel> Read(CardsBindingModels model)
        {
            if (model == null)
            {
                return _cardsStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<CardsViewModel> { _cardsStorage.GetElement(model) };
            }
            return _cardsStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(CardsBindingModels model)
        {
            CardsViewModel cards = _cardsStorage.GetElement(new CardsBindingModels
            {
                CardsNumder = model.CardsNumder
            });
            if (cards != null && cards.Id != model.Id)
            {
                throw new Exception("Карта уже существует");
            }
            if (model.Id.HasValue)
            {
                _cardsStorage.Update(model);
            }
            else
            {
                _cardsStorage.Insert(model);
            }
        }
        public void Delete(CardsBindingModels model)
        {
            CardsViewModel cards = _cardsStorage.GetElement(new CardsBindingModels
            {
                Id = model.Id
            });
            if (cards == null)
            {
                throw new Exception("Карта не найдена");
            }
            _cardsStorage.Delete(model);
        }
    }
}
