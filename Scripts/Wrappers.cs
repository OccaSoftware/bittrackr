namespace OccaSoftware.BitTrackr.Runtime
{
    public static class Wrappers
    {
        #region Global Crypto Data
        [System.Serializable]
        public class GlobalMarketWrapper
        {
            public GlobalMarket[] data;

            [System.Serializable]
            public class GlobalMarket
            {
                public int coins_count;
                public int active_markets;
                public double total_mcap;
                public double total_volume;
                public string btc_d;
                public string eth_d;
                public string mcap_change;
                public string volume_change;
                public string avg_change_percent;
                public double volume_ath;
                public double mcap_ath;
            }
        }

        #endregion Global Crypto Data


        #region Tickers (All coins)
        [System.Serializable]
        public class AllCoins
        {
            public Data[] data;
            public Info info;

            [System.Serializable]
            public class Data
            {
                public string id;
                public string symbol;
                public string name;
                public string nameid;
                public int rank;
                public string price_usd;
                public string percent_change_24h;
                public string percent_change_1h;
                public string percent_change_7d;
                public string price_btc;
                public string market_cap_usd;
                public double volume24;
                public double volume24a;
                public string csupply;
                public string tsupply;
                public string msupply;
            }

            [System.Serializable]
            public class Info
            {
                public int coins_num;
                public int time;
            }
        }

        #endregion Tickers (All coins)


        #region Ticker (Specific)
        [System.Serializable]
        public class CoinWrapper
        {
            public Coin[] tickers;

            [System.Serializable]
            public class Coin
            {
                public string id;
                public string symbol;
                public string name;
                public string nameid;
                public int rank;
                public string price_usd;
                public string percent_change_24h;
                public string percent_change_1h;
                public string percent_change_7d;
                public string market_cap_usd;
                public string volume24;
                public string volume24_native;
                public string csupply;
                public string price_btc;
                public string tsupply;
                public string msupply;
            }
        }
        #endregion Ticker (Specific)


        #region Markets
        [System.Serializable]
        public class MarketWrapper
        {
            public Market[] markets;

            [System.Serializable]
            public class Market
            {
                public string name;
                public string @base;
                public string quote;
                public double price;
                public double price_usd;
                public double volume;
                public double volume_usd;
                public int time;
            }
        }
        #endregion Markets
    }
}
