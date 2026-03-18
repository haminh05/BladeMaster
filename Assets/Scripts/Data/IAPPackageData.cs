using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IAPPackage", menuName = "Shop/IAP Package")]
public class IAPPackageData : ScriptableObject
{
    public string productID;
    public string displayName;

    public PackageType packageType;

    public int appleAmount;
    public int livesAmount;

    public List<int> knifeUnlockIDs;

    public bool removeAds;

    public bool isVIP;
    public int vipDurationDays;
}

public enum PackageType
{
    KnifePackage,
    LivesPackage,
    RemoveAds,
    VIP
}