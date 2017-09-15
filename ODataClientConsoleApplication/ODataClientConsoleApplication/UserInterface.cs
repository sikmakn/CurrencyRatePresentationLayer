using System;
using System.Collections.Generic;
using System.Linq;
using ODataClientConsoleApplication.DataAccess.DataBase;

namespace ODataClientConsoleApplication
{
    public class UserInterface
    {
        private readonly ServerODataHandler _serverHandler;
        private readonly HistoryRateHandler _historyHandler;

        public UserInterface(ServerODataHandler serverHandler, HistoryRateHandler historyHandler)
        {
            _serverHandler = serverHandler;
            _historyHandler = historyHandler;
        }

        public void Start()
        {
            var outKey = -1;
            do
            {
                string key;
                do
                {
                    Console.WriteLine("If you want know current rate press - '1',\n" +
                                      "If you want view currency rate statistic press - '2',\n" +
                                      "For End press - '0'");
                    key = Console.ReadLine();

                    if(key != null && key.Equals("0")) return;

                } while (!key.Equals("1") && !key.Equals("2"));

                outKey = key.Equals("2") ? GetStatictic() : CurrentRate();
            } while (outKey != 0);
        }

        private int GetStatictic()
        {
            var currencyKey = GetCurrencyKey();
            var currencyId = GetCurrencyIdByKey(currencyKey);
            
            Console.WriteLine("Statistic from 2016-16-09 to 2017-15-09");

            var rates = _historyHandler.GetRateHistory(currencyId, new DateTime(2016, 09, 16), new DateTime(2017,09,15)).GetAwaiter().GetResult();

            foreach (var rate in rates)
            {
                Console.WriteLine("Rate: " + rate.Cur_OfficialRate + " ||| Date: " + rate.Date.ToString("u").Split(' ')[0]);
            }
            return -1;
        }

        private int CurrentRate()
        {
            string key = GetCurrencyKey();

            if (key.Equals("0")) return 0;

            if (key.Equals("4"))
            {
                GetStatictic();
                return 0;
            }
            string currency = GetCurrencyByKey(key);

            string cityKey = GetCityKey();

            if (key.Equals("0")) return 0;

            if (key.Equals("4"))
            {
                GetStatictic();
                return -1;
            }

            string city = GetCityByKey(cityKey);

            var allResult = _serverHandler.GetCurrencyRate(city, currency);
            WriteAllResult(allResult);

            _serverHandler.GetBestRate(out var sale, out var purchase, city, currency);
            WriteBestRate(sale, purchase);
            return -1;
        }

        private void WriteAllResult(IEnumerable<CurrencyRateByTime> results)
        {
            Console.WriteLine("Currency rate by all departments | minsk |  dollar: ");
            foreach (var rateByTime in results)
            {
                Console.WriteLine("Purchase:" + rateByTime.Purchase + " ||| Sale: " + rateByTime.Sale + 
                    " ||| Bank:" + rateByTime.BankDepartment.Bank.Name.Trim() + " ||| Department:" + rateByTime.BankDepartment.Name.Trim() + 
                    " ||| Address:" + rateByTime.BankDepartment.Address.Trim());
            }
        }

        private void WriteBestRate(double sale, double purchase)
        {
            Console.WriteLine("\n\n Best Sale: " + sale + " ||| Best purchase: " + purchase);
        }

        private string GetCityKey()
        {
            string key;

            do
            {
                Console.WriteLine("Select currency: \n" +
                                  "minsk - '1'\n" +
                                  "vitebsk - '2'\n" +
                                  "brest - '3' \n" +
                                  "If you want view currency rate statistic press - '4' \n" +
                                  "For out press - 0");
                key = Console.ReadLine();
            } while (!key.Equals("0") && !key.Equals("1") && !key.Equals("2") && !key.Equals("3") && !key.Equals("4"));

            return key;
        }

        private string GetCurrencyKey()
        {
            string key;

            do
            {
                Console.WriteLine("Select currency: \n" +
                                  "dollar - '1'\n" +
                                  "euro - '2'\n" +
                                  "russian ruble(by 100 rub) - '3' \n" +
                                  "For out press - 0");
                key = Console.ReadLine();
            } while (!key.Equals("0") && !key.Equals("1") && !key.Equals("2") && !key.Equals("3") && !key.Equals("4"));

            return key;
        }

        private static string GetCurrencyByKey(string key)
        {
            var currency = "";
            switch (key)
            {
                case "1":
                    currency = "dollar";
                    break;
                case "2":
                    currency = "euro";
                    break;
                case "3":
                    currency = "rubl";
                    break;
            }
            return currency;
        }

        private static int GetCurrencyIdByKey(string key)
        {
            var currencyId = 0;
            switch (key)
            {
                case "1":
                    currencyId = 145;
                    break;
                case "2":
                    currencyId = 292;
                    break;
                case "3":
                    currencyId = 298;
                    break;
            }
            return currencyId;
        }

        private static string GetCityByKey(string key)
        {
            var city = "";
            switch (key)
            {
                case "1":
                    city = "minsk";
                    break;
                case "2":
                    city = "vitebsk";
                    break;
                case "3":
                    city = "brest";
                    break;
            }
            return city;
        }
    }
}