using System.Collections.Generic;
using System.Linq;
using ODataClientConsoleApplication.DataAccess.DataBase;
using ODataClientConsoleApplication.Default;

namespace ODataClientConsoleApplication
{
    public class ServerODataHandler
    {
        private readonly Container _container;

        public ServerODataHandler(Container container)
        {
            _container = container;
        }


        public IEnumerable<CurrencyRateByTime> GetCurrencyRate(string city, string currency)
        {
            var resultExpand = _container.CurrencyRateByTime.Expand(x => x.BankDepartment.Bank);
            resultExpand = resultExpand.Expand(x => x.BankDepartment.City);
            var result = resultExpand.Where(x => x.Currency.Name.Contains(currency) && x.BankDepartment.City.Name == city);
            return result.ToList();
        }

        public void GetBestRate(out double sale, out double purchase, string city, string currency)
        {
            var allResults = GetCurrencyRate(city, currency);
            sale = allResults.Max(x => x.Sale);
            purchase = allResults.Min(x => x.Purchase);
        }
    }
}
