using System;
using System.Collections.Generic;
using System.Globalization;
using Facebook.Unity;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaperStag;

namespace Dreamsim.Publishing
{
public class PurchaseValidationData
{
    public class PurchaseValidationDataData
    {
        public class PurchaseValidationDataDataFB
        {
            [JsonProperty("bundle_short_version")]
            public string BundleShortVersion { get; private set; }

            [JsonProperty("user_id")]
            public string UserId { get; private set; }

            [JsonProperty("advertiser_id")]
            public string AdvertiserId { get; private set; }

            [JsonProperty("advertiser_tracking_enabled")]
            public bool AdvertiserTrackingEnabled { get; private set; }

            [JsonProperty("application_tracking_enabled")]
            public bool ApplicationTrackingEnabled { get; private set; }

            [JsonProperty("product_id")]
            public string ProductId { get; private set; }

            [JsonProperty("product_quantity")]
            public double ProductQuantity { get; private set; }

            [JsonProperty("product_title")]
            public string ProductTitle { get; private set; }

            [JsonProperty("product_description")]
            public string ProductDescription { get; private set; }

            [JsonProperty("value_to_sum")]
            public decimal ValueToSum { get; private set; }

            [JsonProperty("log_time")]
            public int LogTime { get; private set; }

            [JsonProperty("num_items")]
            public int NumItems { get; private set; } = 1;

            [JsonProperty("currency")]
            public string Currency { get; private set; }

            [JsonProperty("transaction_date")]
            public string TransactionDate { get; private set; }

            [JsonProperty("extinfo")]
            public List<string> ExtInfo { get; private set; }

            public PurchaseValidationDataDataFB(UnityEngine.Purchasing.Product product)
            {
                BundleShortVersion = DeviceHelper.ShortVersionName;
                UserId = AccessToken.CurrentAccessToken?.UserId;
                AdvertiserId = Analytics.AdvertisingId;
                AdvertiserTrackingEnabled = Analytics.TrackingEnabled;
                ApplicationTrackingEnabled = true;

                ProductId = product.definition.id;
                ProductQuantity = 1;
                ProductTitle = product.metadata.localizedTitle;
                ProductDescription =
                    product.metadata.localizedDescription;
                ValueToSum = product.metadata.localizedPrice;
                LogTime = (int)DateTime.UtcNow
                    .Subtract(new DateTime(1970, 1, 1))
                    .TotalSeconds;
                Currency = product.metadata.isoCurrencyCode;
                TransactionDate =
                    DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss+ffff");

                #if UNITY_ANDROID || UNITY_IOS
                ExtInfo = new List<string>
                {
                    #if UNITY_ANDROID
                    "a2",
                    #elif UNITY_IOS
                                        "i2",
                    #endif
                    DeviceHelper.PackageName,
                    DeviceHelper.PackageVersionCode,
                    BundleShortVersion,
                    DeviceHelper.OSVersion,
                    DeviceHelper.DeviceModelName,
                    DeviceHelper.Locale,
                    DeviceHelper.TimeZoneAbbreviation,
                    DeviceHelper.CarrierName,
                    Screen.width.ToString(),
                    Screen.height.ToString(),
                    DeviceHelper.ScreenDensity,
                    DeviceHelper.CPUCores.ToString(),
                    DeviceHelper.TotalDiskSpace.ToString(CultureInfo.InvariantCulture),
                    DeviceHelper.RemainingDiskSpace.ToString(CultureInfo.InvariantCulture),
                    DeviceHelper.TimeZone
                };
                #endif
            }
        }

        public class ReceiptData
        {
            [JsonProperty("store")]
            public string Store { get; private set; }

            [JsonProperty("transaction_id")]
            public string TransactionId { get; private set; }

            [JsonProperty("payload")]
            public string Payload { get; private set; }

            public ReceiptData(string store,
                string transactionId,
                string payload)
            {
                Store = store;
                TransactionId = transactionId;
                Payload = payload;
            }
        }

        [JsonProperty("receipt_data")]
        public ReceiptData Receipt { get; private set; }

        [JsonProperty("adjust_device_id")]
        public string AdjustDeviceId { get; private set; }

        [JsonProperty("fb")]
        public PurchaseValidationDataDataFB Facebook { get; private set; }

        public PurchaseValidationDataData(UnityEngine.Purchasing.Product product)
        {
            var unityReceipt = JObject.Parse(product.receipt);
            Receipt = new ReceiptData(unityReceipt.GetValue("Store")?.ToString(),
                unityReceipt.GetValue("TransactionID")?.ToString(),
                unityReceipt.GetValue("Payload")?.ToString());
            Facebook = new PurchaseValidationDataDataFB(product);
            AdjustDeviceId = "none";
        }
    }

    [JsonProperty("platform")]
    public string Platform { get; private set; }

    [JsonProperty("game")]
    public string Game { get; private set; }

    [JsonProperty("is_sandbox")]
    public bool IsSandbox { get; private set; }

    [JsonProperty("data")]
    public PurchaseValidationDataData Data { get; private set; }

    public PurchaseValidationData(string slug, UnityEngine.Purchasing.Product product, bool isSandbox)
    {
        Platform = Application.platform switch
        {
            RuntimePlatform.Android => "android",
            RuntimePlatform.IPhonePlayer => "ios",
            _ => "unsupported"
        };

        IsSandbox = isSandbox;
        Game = slug;
        Data = new PurchaseValidationDataData(product);
    }
}
}