using PizzeriaDoublePineapple.Bl.Models;
using PizzeriaDoublePineapple.Data;
using PizzeriaDoublePineapple.Data.Models;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Bl
{
    public class SauceService
    {
        private readonly SauceRepository _sauceRepository = new SauceRepository();

        public void CreateNewSauce(string name, double price)
        {
            Sauce sauceBl = new Sauce(name, price);
            _sauceRepository.Add(MapToDataModel(sauceBl));
        }

        private SauceData MapToDataModel(Sauce sauceBl)
        {
            SauceData sauce = new SauceData
            {
                Id = sauceBl.Id,
                Name = sauceBl.Name,
                Price = sauceBl.Price
            };

            return sauce;
        }

        private Sauce MapToBusinessModel(SauceData sauce)
        {
            Sauce sauceBl = new Sauce(sauce.Name, sauce.Price)
            {
                Id = sauce.Id,
                Name = sauce.Name,
                Price = (double)sauce.Price
            };

            return sauceBl;
        }

        public List<Sauce> GetAllSauces()
        {
            List<SauceData> sauces = _sauceRepository.GetAllSauces();
            List<Sauce> sauceBls = new List<Sauce>();

            foreach (SauceData sauce in sauces)
            {
                sauceBls.Add(MapToBusinessModel(sauce));
            }

            return sauceBls;
        }
        public bool IsAnySauceAdded()
        {
            bool result = _sauceRepository.GetAllSauces().Count != 0;
            return result;
        }

        public Sauce GetSauceById(int id)
        {
            SauceData sauce = _sauceRepository.GetSauceById(id);
            Sauce sauceBl = MapToBusinessModel(sauce);

            return sauceBl;
        }
    }
}
