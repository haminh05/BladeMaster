using UnityEngine;

[CreateAssetMenu(fileName = "KnifeData", menuName = "Game/Knife")]
public class KnifeData : ScriptableObject
{
    public int id;
    public bool isDefault;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    public GameObject knifePrefab;

    public KnifeUnlockType unlockType;

    [Header("Apple Unlock")]
    public int appleCost;

    [Header("Ad Unlock")]
    public int adCount; 

    [Header("Boss Unlock")]
    public string bossName;
}

public enum KnifeUnlockType
{
    Apple,
    Ads,
    Boss,
    BossChallenge
}