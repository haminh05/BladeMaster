using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public IAPPackageData[] packages;

    void Awake() { Instance = this; }

    // Hàm duy nhất gọi từ UI button
    public void BuyProduct(string productID)
    {
        // Luôn gọi qua IAPManager — Editor sẽ tự hiện fake store popup
        IAPManager.Instance.BuyProduct(productID);
    }

    // Gọi tự động sau khi store (thật hoặc fake) xác nhận
    public void OnPurchaseSuccess(string productID)
    {
        IAPPackageData pack = FindPackage(productID);
        if (pack == null)
        {
            Debug.LogWarning($"Không tìm thấy package: {productID}");
            return;
        }
        ApplyPackage(pack);
    }

    IAPPackageData FindPackage(string productID)
    {
        foreach (var pack in packages)
            if (pack.productID == productID) return pack;
        return null;
    }

    void ApplyPackage(IAPPackageData pack)
    {
        if (pack.appleAmount > 0)
            SaveSystem.AddApples(pack.appleAmount);

        if (pack.livesAmount > 0)
            SaveSystem.SaveLives(SaveSystem.LoadLives() + pack.livesAmount);

        if (pack.removeAds)
        {
            PlayerPrefs.SetInt("REMOVE_ADS", 1);
            PlayerPrefs.Save();
        }

        if (pack.isVIP)
        {
            VIPSystem.ActivateVIP(pack.vipDurationDays);
            PlayerPrefs.Save();
        }

        foreach (var knifeID in pack.knifeUnlockIDs)
            InventoryManager.Instance.UnlockKnife(knifeID);

        Debug.Log($"✅ Đã áp dụng: {pack.displayName}");
    }

    public static bool IsVIP => PlayerPrefs.GetInt("VIP", 0) == 1;
    public static bool IsAdsRemoved => PlayerPrefs.GetInt("REMOVE_ADS", 0) == 1;
}