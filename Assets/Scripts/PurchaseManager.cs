using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : Singleton<PurchaseManager>, IStoreListener
{
    private IStoreController storeController;

    private bool adsDisabled = false;

    // Set the product ID for the feature you want to check
    private string removeAdsId = "com.gregorii_k.flying_balls.remove_ads";

    void Start()
    {
        InitializePurchasing();
    }

    private void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(removeAdsId, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;

        // Check if the user has previously purchased the feature
        if (storeController != null)
        {
            Product product = storeController.products.WithID(removeAdsId);
            if (product != null && product.hasReceipt)
            {
                // Feature has been purchased
                adsDisabled = true;
            }
        }
        IsInitialized();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Failed to initialize Unity Purchasing: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == removeAdsId)
        {
            // Feature has been purchased
            adsDisabled = true;
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase of {product.definition.id} failed with reason {failureReason}");
    }

    public bool AreAdsDisabled()
    {
        return adsDisabled;
    }


    public void BuyRemoveAds()
    {
        PurchaseProduct(removeAdsId);
    }

    private bool IsInitialized()
    {
        return storeController != null;
    }

    private void PurchaseProduct(string id)
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(id);
            if (product != null && product.availableToPurchase)
            {
                Debug.LogFormat("Purchasing product asynchronously: {0}", product.definition.id);
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("PurchaseProduct FAIL. Product not found or not available for purchase.");
            }
        }
        else
        {
            Debug.Log("PurchaseProduct FAIL. Not initialized.");
        }

    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }


    //    private static IStoreController storeController;
    //    private static IExtensionProvider extensionProvider;

    //    private bool adsDisabled = false;
    //    private string removeAdsId = "com.gregorii_k.flying_balls.remove_ads";

    //    void Start()
    //    {
    //    InitializePurchasing();

    //    // Check if the user has previously purchased the feature
    //    if (storeController != null)
    //    {
    //        Product product = storeController.products.WithID(removeAdsId);
    //        if (product != null && product.hasReceipt)
    //        {
    //            // Feature has been purchased
    //            adsDisabled = true;
    //        }
    //    }
    //    }

    //    private bool IsInitialized()
    //    {
    //        return storeController != null && extensionProvider != null;
    //    }

    //    private void InitializePurchasing()
    //    {
    //        if (IsInitialized())
    //        {
    //            return;
    //        }
    //        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
    //        builder.AddProduct(removeAdsId, ProductType.NonConsumable);
    //    }


    //    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    //    {
    //        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, removeAdsId, System.StringComparison.Ordinal))
    //        {
    //            Debug.LogFormat("ProcessPurchase: PASS. Product : {0}", purchaseEvent.purchasedProduct.definition.id);
    //            //PlayerPrefs.SetInt("AdsRemoved", 1);
    //        }
    //        else
    //        {
    //            Debug.LogFormat("ProcessPurchase : FAIL. Unrecognized product : {0}", purchaseEvent.purchasedProduct.definition.id);
    //        }

    //        return PurchaseProcessingResult.Complete;
    //    }

    //    private void PurchaseProduct(string id)
    //    {
    //        if (IsInitialized())
    //        {
    //            Product product = storeController.products.WithID(id);
    //            if (product != null && product.availableToPurchase)
    //            {
    //                Debug.LogFormat("Purchasing product asynchronously: {0}", product.definition.id);
    //                storeController.InitiatePurchase(product);
    //            }
    //            else
    //            {
    //                Debug.Log("PurchaseProduct FAIL. Product not found or not available for purchase.");
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("PurchaseProduct FAIL. Not initialized.");
    //        }

    //    }

    //    public void OnPurchaseCompleted(Product product)
    //    {
    //        if (product.definition.id == removeAdsId)
    //        {
    //            adsDisabled = true;
    //            AdsManager.Instance.DisableAllAds();
    //        }
    //    }

    //    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    //    {
    //        Debug.LogFormat("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason);
    //    }

    //    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    //    {
    //        storeController = controller;
    //        extensionProvider = extensions;
    //    }

    //    public void BuyRemoveAds()
    //    {
    //        PurchaseProduct(removeAdsId);
    //    }

    //    public bool AreAdsDisabled()
    //    {
    //        return adsDisabled;
    //    }

    //    public void OnInitializeFailed(InitializationFailureReason error)
    //    {
    //        Debug.Log("OnInitializeFailed InitializationFailureReason: " + error);
    //    }

    //    public void OnInitializeFailed(InitializationFailureReason error, string message)
    //    {
    //        Debug.Log("OnInitializeFailed InitializationFailureReason: " + error);
    //    }
}
