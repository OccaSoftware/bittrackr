using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;
using OccaSoftware.BitTrackr.Runtime;
using System;
using static OccaSoftware.BitTrackr.Runtime.Wrappers;

namespace OccaSoftware.BitTrackr.Demo
{
    /// <summary>
    /// This component demonstrates one way in which you may query cached ticker data.
    /// Pulling down the ticker data can take some time.
    /// It is recommended that you cache the ticker ID and matching data offline with periodic realtime updates and pull the ticker price and market data on request.
    /// </summary>
    public class SearchCryptoDB_Example : MonoBehaviour
    {
        [SerializeField]
        private Text progress;

        [SerializeField]
        private Text symbol;

        [SerializeField]
        private Text coinName;

        [SerializeField]
        private Text price;

        [SerializeField]
        private Text marketCap;

        [SerializeField]
        private Text rank;

        [SerializeField]
        private Text id;

        private bool requestedTickerData;

        public Client bitClient;
        private List<AllCoins.Data> coinData;

        private void Start()
        {
            bitClient = new Client();
            GetSearchResults("");
        }

        public async void GetSearchResults(string searchQuery)
        {
            if (coinData == null && !requestedTickerData)
            {
                await LoadAllCoins();
                requestedTickerData = true;
            }

            List<Runtime.Wrappers.AllCoins.Data> searchResults = coinData.FindAll(
                x => x.nameid.IndexOf(searchQuery, System.StringComparison.OrdinalIgnoreCase) >= 0
            );

            symbol.text = "";
            coinName.text = "";
            price.text = "";
            marketCap.text = "";
            rank.text = "";
            id.text = "";

            Debug.Log($"{searchResults.Count}, {searchResults[0].name}");
            for (int i = 0; i < searchResults.Count && i < 25; i++)
            {
                symbol.text += searchResults[i].symbol + "\n";
                coinName.text += searchResults[i].name + "\n";

                string priceString = System.Convert
                    .ToDouble(searchResults[i].price_usd)
                    .ToString("N8", CultureInfo.CreateSpecificCulture("en-US"));
                price.text += priceString + "\n";

                string marketCapString = System.Convert
                    .ToDouble(searchResults[i].market_cap_usd)
                    .ToString("N2", CultureInfo.CreateSpecificCulture("en-US"));
                marketCap.text += marketCapString + "\n";

                rank.text += searchResults[i].rank + "\n";
                id.text += searchResults[i].id + "\n";
            }
        }

        private async Task LoadAllCoins()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            Task<List<AllCoins.Data>> task = bitClient.GetAllCoinData();

            if (progress == null)
                return;

            progress.text = "";
            progress.enabled = true;
            while (!task.IsCompleted)
            {
                progress.text = "Loading all tickers... " + sw.Elapsed.TotalSeconds.ToString("N2");
                await Task.Delay(100);
            }

            sw.Stop();

            coinData = await task;

            progress.text =
                "All Ticker Data Loaded, accessible via CryptoAPIHelper.GetCachedCoinData().";
        }
    }
}
