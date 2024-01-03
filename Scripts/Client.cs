using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using static OccaSoftware.BitTrackr.Runtime.Wrappers;

namespace OccaSoftware.BitTrackr.Runtime
{
    /// <summary>
    /// Data provided by public API, https://www.coinlore.com/cryptocurrency-data-api
    /// Coinlore provides a free cryptocurrency api that doesn't need api keys and is available publicly.
    /// OccaSoftware makes no guarantees regarding availability or accuracy of the data provided through the Coinlore API
    /// In the event that Coinlore becomes permanently or temporarily unavailable, OccaSoftware makes no guarantee that this software will be updated to an alternative API provider
    /// </summary>
    public class Client
    {
        private const string globalMarketUrl = "https://api.coinlore.net/api/global/";
        private const string allTickersUrl = "https://api.coinlore.net/api/tickers/?start=";
        private const string limitDeclarationPrefix = "&limit=";
        private const string specificTickersUrl = "https://api.coinlore.net/api/ticker/?id=";
        private const string marketSearchUrl = "https://api.coinlore.net/api/coin/markets/?id=";
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// CacheAllCoinData operates in batches of 100. If limited, we will return the lesser of total coins on the market and the limit rounded to the upper hundred.
        /// For example, if you submit a limit of 150, we will return 200 results.
        /// Limited to 1000 records by default.
        /// </summary>
        /// <param name="numRecordsLowerLimit">Lower limit of records to return</param>
        public async Task<List<AllCoins.Data>> GetAllCoinData(int numRecordsLowerLimit = 1000)
        {
            List<AllCoins.Data> coinData = new List<AllCoins.Data>();
            GlobalMarketWrapper.GlobalMarket globalMarket = await GetGlobalMarketData();
            int numRecords = Mathf.Min(globalMarket.coins_count, numRecordsLowerLimit);

            int iterations = Mathf.CeilToInt(numRecords / 100.0f);

            List<Task<AllCoins>> tasks = new List<Task<AllCoins>>();

            for (int i = 0; i < iterations; i++)
            {
                tasks.Add(GetCoinsByRank(i * 100));
            }
            await Task.WhenAll(tasks);

            for (int i = 0; i < iterations; i++)
            {
                coinData.AddRange(tasks[i].Result.data);
            }

            return coinData;
        }

        public async Task<GlobalMarketWrapper.GlobalMarket> GetGlobalMarketData()
        {
            string json = await GetJsonAsync(globalMarketUrl);
            return JsonUtility.FromJson<GlobalMarketWrapper>("{\"data\":" + json + "}").data[0];
        }

        public async Task<AllCoins> GetCoinsByRank(int startRank = 0, int numberOfResults = 100)
        {
            if (numberOfResults > 100)
            {
                Debug.Log(
                    "GetAllTickers() has a maximum of 100 coins per request. Returning first 100 coins from start rank."
                );
            }

            string json = await GetJsonAsync(
                allTickersUrl + startRank + limitDeclarationPrefix + numberOfResults
            );
            return JsonUtility.FromJson<AllCoins>(json);
        }

        /// <summary>
        /// Gets the information about specific coin(s) from coin id(s)
        /// You can get the Coin ID from the GetCoinsByRank() method.
        /// Note that some coins share the same symbol, hence the ID method.
        /// </summary>
        /// <param name="coinIds">Array of coin IDs</param>
        public async Task<CoinWrapper.Coin[]> GetCoinsById(string[] coinIds)
        {
            string idAppend = coinIds[0];
            for (int i = 1; i < coinIds.Length; i++)
            {
                idAppend += "," + coinIds[i];
            }

            string json = await GetJsonAsync(specificTickersUrl + idAppend);
            return JsonUtility.FromJson<CoinWrapper>("{\"tickers\":" + json + "}").tickers;
        }

        public async Task<CoinWrapper.Coin> GetCoinById(string coinId)
        {
            string json = await GetJsonAsync(specificTickersUrl + coinId);
            return JsonUtility.FromJson<CoinWrapper>("{\"tickers\":" + json + "}").tickers[0];
        }

        public async Task<MarketWrapper.Market[]> GetMarketsForCoin(string coinId)
        {
            string json = await GetJsonAsync(marketSearchUrl + coinId);
            return JsonUtility.FromJson<MarketWrapper>("{\"markets\":" + json + "}").markets;
        }

        public async Task<CoinWrapper.Coin> GetBTC()
        {
            return await GetCoinById("90");
        }

        public async Task<CoinWrapper.Coin> GetETH()
        {
            return await GetCoinById("80");
        }

        private async Task<string> GetJsonAsync(string endpoint)
        {
            try
            {
                return await client.GetStringAsync(endpoint);
            }
            catch (HttpRequestException e)
            {
                Debug.Log("Exception Caught. Message: " + e.InnerException.Message);
                throw;
            }
        }
    }
}
