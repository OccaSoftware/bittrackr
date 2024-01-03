using OccaSoftware.BitTrackr.Runtime;

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static OccaSoftware.BitTrackr.Runtime.Wrappers;

namespace OccaSoftware.BitTrackr.Demo
{
    public class GetCryptoData_Example : MonoBehaviour
    {
        [SerializeField]
        private Text globalMarketData;

        [SerializeField]
        private Text topCoinData;

        [SerializeField]
        private Text coinData;

        [SerializeField]
        private Text btcMarkets;

        public Client bitClient;

        private void Start()
        {
            bitClient = new Client();

            WriteGlobalMarketDataExample();
            WriteTopCoinsExample();
            WriteBtcAndEthDataExample();
            WriteBtcMarketData();
        }

        private async void LogGlobalMarketCap()
        {
            var globalMarket = await bitClient.GetGlobalMarketData();
            Debug.Log(globalMarket.total_mcap);
        }

        private async void WriteGlobalMarketDataExample()
        {
            while (true)
            {
                if (globalMarketData == null)
                    return;

                globalMarketData.text = "...";
                GlobalMarketWrapper.GlobalMarket globalMarket = await bitClient.GetGlobalMarketData();
                string total_mcap = globalMarket.total_mcap.ToString();
                string total_volume = globalMarket.total_volume.ToString();

                if (globalMarketData == null)
                    return;
                globalMarketData.text = "Total Market Cap: " + total_mcap + "\nTotal Volume: " + total_volume;
                await Task.Delay(Random.Range(4000, 6000));
            }
        }

        private async void WriteTopCoinsExample()
        {
            while (true)
            {
                if (topCoinData == null)
                    return;

                topCoinData.text = "...";
                AllCoins allCoins = await bitClient.GetCoinsByRank();
                string top_coin = allCoins.data[0].name;
                string top_coin_price_usd = allCoins.data[0].price_usd;

                if (topCoinData == null)
                    return;
                topCoinData.text = "Name: " + top_coin + "\nPrice: " + top_coin_price_usd;
                await Task.Delay(Random.Range(4000, 6000));
            }
        }

        /// <summary>
        /// "90" is the ID for Bitcoin, "80" is the ID for Eth. Coin ID information can be pulled from the coins list, which you can cache using something like LoadAllCoins(), demonstrated in SearchCryptoDB_Example.cs.
        /// </summary>
        private async void WriteBtcAndEthDataExample()
        {
            while (true)
            {
                if (coinData == null)
                    return;
                coinData.text = "...";
                CoinWrapper.Coin[] coins = await bitClient.GetCoinsById(new string[] { "90", "80" });
                string p0 = coins[0].price_usd;
                string p1 = coins[1].price_usd;

                if (coinData == null)
                    return;
                coinData.text = "BTC $" + p0;
                coinData.text += "\n" + "ETH $" + p1;
                await Task.Delay(Random.Range(4000, 6000));
            }
        }

        private async void WriteBtcMarketData()
        {
            while (true)
            {
                if (btcMarkets == null)
                    return;

                btcMarkets.text = "...";
                MarketWrapper.Market[] markets = await bitClient.GetMarketsForCoin("90");
                string m0 = markets[0].name + " " + markets[0].price_usd;
                string m1 = markets[1].name + " " + markets[1].price_usd;
                string m2 = markets[2].name + " " + markets[2].price_usd;

                if (btcMarkets == null)
                    return;

                btcMarkets.text = "1. " + m0;
                btcMarkets.text += "\n2. " + m1;
                btcMarkets.text += "\n3. " + m2;
                await Task.Delay(Random.Range(4000, 6000));
            }
        }
    }
}
