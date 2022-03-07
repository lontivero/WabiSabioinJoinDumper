using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NBitcoin;
using Newtonsoft.Json.Linq;

namespace CoinJoinWatcher;
class Program
{
	static async Task Main(string[] args)
	{
		var xpub = "xpub6C13JhXzjAhVRgeTcRSWqKEPe1vHi3Tmh2K9PN1cZaZFVjjSaj76y5NNyqYjc2bugj64LVDFYu8NZWtJsXNYKFb9J94nehLAPAKqKiXcebC";
		var coordinatorExtPubKey = ExtPubKey.Parse(xpub);
		var masterPubKey = coordinatorExtPubKey.Derive(0, false);
		var walletScripts = Enumerable.Range(0, 20_000)
			.Select(depth => masterPubKey.Derive(depth, false).PubKey.WitHash.ScriptPubKey)
			.ToHashSet();

		var coinjoins = File.ReadAllLines("coinjoins-testnet.txt");
		foreach (var line in coinjoins.Reverse())
		{
			var tx = await GetTransactionByIdAsync(line);
			var time = DateTimeOffset.FromUnixTimeSeconds(tx["status"].Value<long>("block_time"));
			foreach (var output in tx["vout"])
			{
				var outputScript = Script.FromHex(output.Value<string>("scriptpubkey"));
				var hash = tx.Value<string>("txid");
				var address = outputScript.GetDestinationAddress(Network.TestNet);
				var amount = Money.Satoshis(output.Value<long>("value"));

				Console.WriteLine($"{time.ToUniversalTime():dd/MM/yy HH:mm:ss}, {hash}, {address}, {amount.ToUnit(MoneyUnit.BTC)}");
			}
		}
	}

	private static async Task<JObject> GetTransactionByIdAsync(string txid)
	{
	again:
		try
		{
			var serializedTx = await GetBlockchainInfoAsync($"/tx/{txid}");
			return JObject.Parse(serializedTx);
		}
		catch
		{
			await Task.Delay(new Random().Next(0, 3000));
			goto again;
		}
	}

	private static async Task<string> GetBlockchainInfoAsync(string path)
	{
		using var httpClient = new HttpClient();

		using var response = await httpClient.GetAsync("https://blockstream.info/testnet/api" + path, HttpCompletionOption.ResponseContentRead);
		if (response.StatusCode != HttpStatusCode.OK)
			throw new HttpRequestException(response.StatusCode.ToString());
		string responseString = await response.Content.ReadAsStringAsync();
		return responseString;
	}
}
