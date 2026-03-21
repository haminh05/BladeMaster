using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public static IAPManager Instance;
    public IAPPackageData[] packages;

    private IStoreController controller;
    private IExtensionProvider extensions;
    private bool isInitialized = false;

    void Awake()
    {

        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var pack in packages)
        {
            ProductType type = pack.isVIP
                ? ProductType.Subscription
                : ProductType.Consumable;
            builder.AddProduct(pack.productID, type);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProduct(string productID)
    {
        if (isInitialized)
            controller.InitiatePurchase(productID);
        else
            Debug.LogWarning("IAP chưa khởi tạo!");
    }

    // ── Fake purchase để test không cần store thật ──
    public void FakePurchase(string productID)
    {
        Debug.Log($"[FAKE IAP] Mua: {productID}");
        ShopManager.Instance.OnPurchaseSuccess(productID);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
        isInitialized = true;
        Debug.Log("IAP khởi tạo thành công!");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogWarning($"IAP khởi tạo thất bại: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogWarning($"IAP khởi tạo thất bại: {error} - {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        ShopManager.Instance.OnPurchaseSuccess(args.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning($"Mua thất bại: {product.definition.id} - {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogWarning($"Mua thất bại: {product.definition.id} - {failureDescription.message}");
    }

    // Thêm vào IAPManager.cs
    public void RestorePurchases(System.Action<bool, string> onResult)
    {
#if UNITY_IOS
    var apple = extensions.GetExtension<IAppleExtensions>();
    apple.RestoreTransactions(result =>
    {
        if (result)
        {
            // ProcessPurchase sẽ tự được gọi cho từng product đã mua
            onResult?.Invoke(true, "Khôi phục thành công!");
        }
        else
        {
            onResult?.Invoke(false, "Không tìm thấy giao dịch nào.");
        }
    });
#elif UNITY_ANDROID
        // Android tự restore khi IAP init, không cần gọi thêm
        onResult?.Invoke(true, "RESTORED SUCCESSFULY! \n In-App Purchases restored.");
#else
    // Editor / fake
    onResult?.Invoke(false, "Restore không khả dụng trên nền tảng này.");
#endif
    }
}