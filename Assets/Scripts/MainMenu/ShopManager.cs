using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public IAPPackageData[] packages;

    void Awake()
    {
        Instance = this;
    }

    // gọi khi IAP mua thành công
    public void OnPurchaseSuccess(string productID)
    {
        foreach (var pack in packages)
        {
            if (pack.productID == productID)
            {
                ApplyPackage(pack);
                break;
            }
        }
    }

    void ApplyPackage(IAPPackageData pack)
    {
        // thêm apples
        if (pack.appleAmount > 0)
        {
            SaveSystem.AddApples(pack.appleAmount);
        }

        // thêm lives
        if (pack.livesAmount > 0)
        {
            int lives = SaveSystem.LoadLives();
            SaveSystem.SaveLives(lives + pack.livesAmount);
        }

        // remove ads
        if (pack.removeAds)
        {
            PlayerPrefs.SetInt("REMOVE_ADS", 1);
        }

        // vip
        if (pack.isVIP)
        {
            PlayerPrefs.SetInt("VIP", 1);
        }

        // unlock knives
        foreach (var knifeID in pack.knifeUnlockIDs)
        {
            UnlockKnife(knifeID);
        }

        PlayerPrefs.Save();
    }

    void UnlockKnife(int knifeID)
    {
        string key = "KNIFE_" + knifeID;

        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 1);
        }
    }
    // mua pack lives
    public void BuyLivesPack()
    {
        IAPManager.Instance.BuyProduct("lives_pack");
    }

    // mua pack knives
    public void BuyKnifePack()
    {
        IAPManager.Instance.BuyProduct("knife_pack");
    }

    // remove ads
    public void BuyRemoveAds()
    {
        IAPManager.Instance.BuyProduct("remove_ads");
    }

    // vip
    public void BuyVIP()
    {
        IAPManager.Instance.BuyProduct("vip_sub");
    }
}