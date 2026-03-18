using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance;

    public IAPPackageData[] packages;

    private IStoreController controller;

    void Awake()
    {
        Instance = this;
    }

    public void BuyProduct(string id)
    {
        controller.InitiatePurchase(id);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        ShopManager.Instance.OnPurchaseSuccess(args.purchasedProduct.definition.id);

        return PurchaseProcessingResult.Complete;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message = null)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new System.NotImplementedException();
    }
}