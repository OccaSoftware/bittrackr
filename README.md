# README

## Contents

- About
- Install
- Use
- API

## About

Query and present live cryptocurrency information in your game or app.

BitTrackr acts as a Unity-native SDK for Coinlore's public API. Coinlore provides free access to live price, volume, and change data on 8000+ coins, including Bitcoin, Ethereum, Binance, Shiba, and more.

Asynchronous API calls enable your application to continue to perform smoothly without hitches while BitTrarckr does the heavy lifting of downloading and parsing coin data for you.

BitTrackr gives you detailed coin-level data, including:

- Current Price (USD, BTC)
- Percent Change (1hr, 24hr, 7d)
- Market Cap (USD, BTC)
- Transaction Volumes (24hr)
- Supply Volumes (Coin, Total, Market)
- Market-level pricing and volumes

CryptoData also gives you Global Market-level data, including:

- Number of Coins
- Number of Active Markets
- Total Market Cap (USD)
- Total Volumes
- Bitcoin and ETH Dominance
- Market Cap and Volume Change Percentages

## Install

1. git clone to your /packages/ folder.

## Use

1. Create a new Client
2. Call Client methods using async operators.

```cs
using OccaSoftware.BitTrackr.Runtime;

public Client bitClient;

public void Start(){
    bitClient = new Client();
    WriteMarketCapToConsole();
}

public async void WriteMarketCapToConsole(){
    var globalMarket = await bitClient.GetGlobalMarketData();
    Debug.Log(globalMarket.total_mcap);
}
```

## Public API

The CryptoAPIHelper class includes the following public methods. These can be viewed directly in source in the CryptoAPIHelper.cs file.

### GetAllCoinData()

```cs
public async static Task GetAllCoinData(int numRecordsLowerLimit = 1000);
```

Fetches and caches a set of coin data. Primary use case is for identifying a coin's ID. CacheAllCoinData operates in batches of 100. If limited, we will return the lesser of total coins on the market and the limit rounded to the upper hundred. For example, if you submit a limit of 150, we will return 200 results. Limited to 1000 records by default.

### GetGlobalMarketData()

```cs
public async static Task<GlobalMarketWrapper.GlobalMarket> GetGlobalMarketData();
```

Gets the current global market data, such as global market volume, market cap, market cap change, etc.

### GetCoinsByRank()

```cs
public async static Task<AllCoins> GetCoinsByRank(int startRank = 0, int numberOfResults = 100);
```

Gets a list of coins with associated price, market cap, change, and volume information. Coins are ranked in order of Market Cap, so the default will return the first 100-ranked coins by market cap.

### GetCoinsById()

```cs
public async static Task<CoinWrapper.Coin[]> GetCoinsById(string[] coinIds);
```

Gets a list of coins with associated price, market cap, change, and volume information. Coins must be specified using the coinIds which can be identified from GetCoinsByRank() or from the cache.

### GetCoinById()

```cs
public async static Task<CoinWrapper.Coin> GetCoinById(string coinId);
```

Gets details on a specific coin identified by coinId, which can be identified from GetCoinsByRank or from the cache.

### GetMarketsForCoin()

```cs
public async static Task<MarketWrapper.Market[]> GetMarketsForCoin(string coinId);
```

Returns an array of markets consisting of market-level coin data, such as coin price, quote, volume, etc.

### GetBTC()

```cs
public async static Task<CoinWrapper.Coin> GetBTC();
```

Returns Coin-level details for Bitcoin.

### GetETH()

```cs
public async static Task<CoinWrapper.Coin> GetETH();
```

Returns Coin-level details for Bitcoin.
