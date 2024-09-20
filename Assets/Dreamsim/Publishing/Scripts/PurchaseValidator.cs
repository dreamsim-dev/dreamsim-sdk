using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Dreamsim.Publishing
{
public class PurchaseValidator
{
    private class Response
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    private const string Url = "https://dreamsim.dev/api/chargeverify/";

    private readonly string _slug;

    public PurchaseValidator(string slug)
    {
        _slug = slug;
    }

    public async UniTask<bool> ValidateAsync(PurchaseEventArgs args)
    {
        if (Application.isEditor) return true;

        var product = args.purchasedProduct;
        var data = new PurchaseValidationData(_slug, product, DreamsimApp.DebugManager.IsConsoleActive);
        var requestContents = JsonConvert.SerializeObject(data);
        var response = await Client.SendPostAsync(Url, requestContents);
        var responseContents = response.Item2;
        var responseObject = JsonConvert.DeserializeObject<Response>(responseContents);
        var result = responseObject.Status == "ok";

        DreamsimLogger.Log($"Purchase validation status: {result}");

        return result;
    }
}
}