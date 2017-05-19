using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TWallet.Models
{
    public static class CurrencyManager
    {
        private static Dictionary<string, Currency> Currencies = new Dictionary<string, Currency>();

        public static Currency GetCurrency(string key)
        {
            return Currencies.ContainsKey(key) ? Currencies[key] : null;
        }

        public static void SetCurrency(string key, Currency currency)
        {
            Currencies.Add(key, currency);
        }

		public static Currency MoveNext(string key)
		{
			IEnumerator<KeyValuePair<string, Currency>> enumerator = Currencies.OrderBy(x => x.Key).GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key.Equals(key))
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current.Value;
					}
					else
					{
                        return Currencies.First().Value;
					}
				}
			}
            return null;
		}

        public static List<Currency> GetCurrencies() 
        {
            return Currencies.Values.ToList();    
        }

        public static async Task Init()
        {
            ClearCurrencies();
			List<Currency> cachedCurrencies = new List<Currency>();
			cachedCurrencies = await App.Database.GetCurrencies();
			foreach (var item in cachedCurrencies)
			{
				SetCurrency(item.CurrencyKey, item);
			}
        }

        public static void ClearCurrencies()
        {
            Currencies.Clear();
        }
    }
}
