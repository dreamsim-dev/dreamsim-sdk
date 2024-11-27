using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Ump.Api;

namespace Dreamsim.Publishing
{
public class GoogleConsentFlow
{
    public string AdvertisingId { get; private set; }
    public bool TrackingEnabled { get; private set; }

    private ConsentForm _form;

    public async UniTask ProcessAsync(List<string> testDeviceHashedIds)
    {
        var done = false;
        
        var debugSettings = new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = testDeviceHashedIds
        };
        
        var requestParameters = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings
        };
        
        void ShowForm() { _form.Show(FormShown); }
        
        void FormShown(FormError error)
        {
            if (error != null)
            {
                DreamsimLogger.LogError("UMP: Consent form shown error");
                DreamsimLogger.LogError(error.Message);
                done = true;
                return;
            }
            
            DreamsimLogger.Log("UMP: Consent form shown");
            done = true;
        }
        
        void ConsentFormLoaded(ConsentForm form, FormError error)
        {
            if (error != null)
            {
                DreamsimLogger.LogError("UMP: Consent form loaded error");
                DreamsimLogger.LogError(error.Message);
                done = true;
                return;
            }
        
            _form = form;
            DreamsimLogger.Log("UMP: Consent form loaded");
        
            if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
            {
                ShowForm();
            }
            else
            {
                DreamsimLogger.Log("UMP: Consent not required");
                TrackingEnabled = true;
                done = true;
            }
        }
        
        void ConsentInfoUpdated(FormError error)
        {
            if (error != null)
            {
                DreamsimLogger.LogError("UMP: Consent info updated error");
                DreamsimLogger.LogError(error.Message);
                done = true;
                return;
            }
            
            DreamsimLogger.Log("UMP: Consent info updated");
        
            if (ConsentInformation.IsConsentFormAvailable())
            {
                DreamsimLogger.Log("UMP: Consent form available");
                ConsentForm.Load(ConsentFormLoaded);
            }
            else
            {
                DreamsimLogger.Log("UMP: Consent form not available");
                TrackingEnabled = ConsentInformation.CanRequestAds();
                done = true;
            }
        }
        
        try
        {
            ConsentInformation.Update(requestParameters, ConsentInfoUpdated);
        }
        catch (Exception e)
        {
            DreamsimLogger.LogError("UMP: Failed to update Google Consent information");
            DreamsimLogger.LogException(e);
            done = true;
            throw;
        }
        
        await UniTask.WaitUntil(() => done);
        
        AdvertisingId = IronSource.Agent.getAdvertiserId();
        DreamsimLogger.Log("UMP: Consent flow process finished");
    }
}
}