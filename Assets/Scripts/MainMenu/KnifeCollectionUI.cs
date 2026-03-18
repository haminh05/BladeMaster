using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class KnifeCollectionUI : MonoBehaviour
{
    public static KnifeCollectionUI Instance;
    public Image previewImage;

    [Header("Unlock Buttons")]
    public GameObject btnApple;
    public GameObject btnAd;
    public GameObject btnBoss;

    [Header("Boss Button States")]
    public GameObject bossLocked;
    public GameObject bossUnlocked;

    [Header("Texts")]
    public TextMeshProUGUI txtAppleCost;
    public TextMeshProUGUI txtAdCount;
    public TextMeshProUGUI txtBossName;

    [Header("Random & Ad Apple")]
    public KnifeDatabase knifeDatabase;
    public TextMeshProUGUI txtRandomCost;
    public TextMeshProUGUI txtRewardApple;
    public int randomCost = 50;
    public int adAppleReward = 10;

    [Header("Footer")]
    public GameObject footer1;
    public GameObject footer2;          
    public TextMeshProUGUI labels;  // KNIVES UNLOCKED"
    public TextMeshProUGUI xy;  // "X/Y"


    private KnifeData selectedKnife;
    private KnifeUnlockType currentPageType = KnifeUnlockType.Apple;

    void Awake() { Instance = this; }

    // Cho KnifeSlotUI check
    public KnifeData GetSelectedKnife() => selectedKnife;

    void OnEnable()
    {
        // Mở panel → không có dao nào selected → frame về equipped
        selectedKnife = null;
        RefreshPreview();
        KnifeSlotUI.RefreshAllSlots();
        InventoryManager.OnEquipChanged += RefreshPreview;
        if (txtRandomCost != null) txtRandomCost.text = randomCost.ToString();
        if (txtRewardApple != null) txtRewardApple.text = "+"+adAppleReward.ToString();

        UpdateFooter(currentPageType);
    }

    void OnDisable()
    {
        // Đóng panel → clear selection
        selectedKnife = null;
        InventoryManager.OnEquipChanged -= RefreshPreview;
    }
    void RefreshPreview()
    {
        if (InventoryManager.Instance == null) return;
        KnifeData equipped = InventoryManager.Instance.GetEquippedKnifeData();
        if (equipped == null) return;

        previewImage.sprite = equipped.unlockedSprite;

        if (btnApple != null) btnApple.SetActive(false);
        if (btnAd != null) btnAd.SetActive(false);

        if (selectedKnife == null ||
            (selectedKnife.unlockType != KnifeUnlockType.Boss &&
             selectedKnife.unlockType != KnifeUnlockType.BossChallenge))
        {
            if (btnBoss != null) btnBoss.SetActive(false);
        }
    }

    public void SelectKnife(KnifeData knife)
    {
        selectedKnife = knife;
        bool unlocked = InventoryManager.Instance.IsKnifeUnlocked(knife.id);
        previewImage.sprite = unlocked ? knife.unlockedSprite : knife.lockedSprite;
        ShowUnlockButton();
    }

    void ShowUnlockButton()
    {
        if (btnApple != null) btnApple.SetActive(false);
        if (btnAd != null) btnAd.SetActive(false);
        if (btnBoss != null) btnBoss.SetActive(false);

        bool unlocked = InventoryManager.Instance.IsKnifeUnlocked(selectedKnife.id);

        switch (selectedKnife.unlockType)
        {
            case KnifeUnlockType.Apple:
                if (!unlocked)
                {
                    btnApple.SetActive(true);
                    txtAppleCost.text = selectedKnife.appleCost.ToString();
                }
                break;

            case KnifeUnlockType.Ads:
                if (!unlocked)
                {
                    btnAd.SetActive(true);
                    txtAdCount.text = selectedKnife.adCount.ToString();
                }
                break;

            case KnifeUnlockType.Boss:
            case KnifeUnlockType.BossChallenge:
                btnBoss.SetActive(true);
                if (bossLocked != null) bossLocked.SetActive(!unlocked);
                if (bossUnlocked != null) bossUnlocked.SetActive(unlocked);
                txtBossName.text = selectedKnife.unlockType == KnifeUnlockType.Boss
                    ? selectedKnife.bossName
                    : "BOSS CHALLENGE";

                if (unlocked)
                {
                    PlayerPrefs.SetInt("EQUIP_KNIFE", selectedKnife.id);
                    PlayerPrefs.Save();
                    InventoryManager.Instance.NotifyEquipChanged();
                }
                break;
        }
    }

    // ── Mua bằng táo ──
    public void OnBuyWithApple()
    {
        if (selectedKnife == null) return;
        int apples = SaveSystem.LoadApples();
        if (apples >= selectedKnife.appleCost)
        {
            SaveSystem.AddApples(-selectedKnife.appleCost);
            InventoryManager.Instance.UnlockKnife(selectedKnife.id);
            InventoryManager.Instance.EquipKnife(selectedKnife.id);
            SelectKnife(selectedKnife);
        }
    }

    // ── Unlock dao bằng ad ──
    public void OnWatchAd()
    {
        if (selectedKnife == null) return;
        // Thay bằng AdManager.Instance.ShowAd(...) khi có SDK
        Debug.Log("Show Ad → Unlock knife");

        // Giả lập xem ad xong:
        // InventoryManager.Instance.UnlockKnife(selectedKnife.id);
        // InventoryManager.Instance.EquipKnife(selectedKnife.id);
        // SelectKnife(selectedKnife);
    }

    // ── Random 1 dao Apple chưa unlock ──
    public void OnRandomKnife()
    {
        if (knifeDatabase == null) return;

        int apples = SaveSystem.LoadApples();
        if (apples < randomCost) { Debug.Log("Không đủ táo!"); return; }

        var locked = knifeDatabase.knives
            .Where(k => k.unlockType == KnifeUnlockType.Apple
                     && !InventoryManager.Instance.IsKnifeUnlocked(k.id))
            .ToList();

        if (locked.Count == 0) { Debug.Log("Đã unlock hết dao Apple!"); return; }

        KnifeData randomKnife = locked[Random.Range(0, locked.Count)];

        SaveSystem.AddApples(-randomCost); 
        InventoryManager.Instance.UnlockKnife(randomKnife.id);
        InventoryManager.Instance.EquipKnife(randomKnife.id);

        selectedKnife = null;
        SelectKnife(randomKnife);
        KnifeSlotUI.RefreshAllSlots();
    }

    // ── Xem ad để nhận táo ──
    public void OnWatchAdForApples()
    {
        
        Debug.Log($"Watch Ad → +{adAppleReward} apples");

        // Giả lập xem ad xong:
        // SaveSystem.AddApples(adAppleReward);
    }

    public void OnPageChanged(KnifeUnlockType pageType)
    {
        currentPageType = pageType;
        UpdateFooter(pageType);
    }

    void UpdateFooter(KnifeUnlockType pageType)
    {
        if (knifeDatabase == null) return;

        int total = knifeDatabase.knives.Count(k => k.unlockType == pageType);
        int unlocked = knifeDatabase.knives.Count(k => k.unlockType == pageType
                         && InventoryManager.Instance.IsKnifeUnlocked(k.id));

        if (pageType == KnifeUnlockType.Apple)
        {
            if (footer1 != null) footer1.SetActive(true);
            if (footer2 != null) footer2.SetActive(false);
          
        }
        else
        {
            if (footer2 != null) footer2.SetActive(true);
            if (footer1 != null) footer1.SetActive(false);
            string label = pageType switch
            {
                KnifeUnlockType.Ads => "KNIVES UNLOCKED",
                KnifeUnlockType.Boss => "BOSS DEFEATED",
                KnifeUnlockType.BossChallenge => "BOSS DEFEATED",
                _ => "KNIVES UNLOCKED"
            };

            if (xy != null) xy.text = $"{unlocked}/{total}";
            if (labels != null) labels.text = label;
        }
    }
}