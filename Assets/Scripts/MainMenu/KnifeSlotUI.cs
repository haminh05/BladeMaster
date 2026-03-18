// KnifeSlotUI.cs
using UnityEngine;
using UnityEngine.UI;

public class KnifeSlotUI : MonoBehaviour
{
    public Image imgUnlocked;
    public Image imgLocked;
    public Image imgFrame;

    [Header("Frame Colors")]
    public Color equippedColor = Color.white;
    public Color selectedLockedColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    private KnifeData knife;
    private bool unlocked;

    public void Setup(KnifeData data)
    {
        knife = data;
        imgUnlocked.sprite = knife.unlockedSprite;
        imgLocked.sprite = knife.lockedSprite;
        Refresh();
        InventoryManager.OnEquipChanged += Refresh;
    }

    void OnDestroy()
    {
        InventoryManager.OnEquipChanged -= Refresh;
    }

    public void Refresh()
    {
        if (knife == null) return;

        unlocked = InventoryManager.Instance.IsKnifeUnlocked(knife.id);
        imgUnlocked.gameObject.SetActive(unlocked);
        imgLocked.gameObject.SetActive(!unlocked);

        if (imgFrame == null) return;

        KnifeData selected = KnifeCollectionUI.Instance?.GetSelectedKnife();
        bool isEquipped = InventoryManager.Instance.GetEquippedKnife() == knife.id;
        bool isSelected = selected == knife;
        bool hasSelection = selected != null;

        if (isSelected)
        {
            // Dao đang được chọn — luôn hiện frame
            imgFrame.gameObject.SetActive(true);
            imgFrame.color = unlocked ? equippedColor : selectedLockedColor;
        }
        else if (isEquipped && !hasSelection)
        {
            // Không có dao nào được chọn → hiện frame equipped
            imgFrame.gameObject.SetActive(true);
            imgFrame.color = equippedColor;
        }
        else
        {
            imgFrame.gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        KnifeCollectionUI.Instance.SelectKnife(knife);

        if (unlocked && knife.unlockType != KnifeUnlockType.Boss
                     && knife.unlockType != KnifeUnlockType.BossChallenge)
        {
            InventoryManager.Instance.EquipKnife(knife.id);
        }

        RefreshAllSlots();
    }

    public static void RefreshAllSlots()
    {
        foreach (var slot in FindObjectsByType<KnifeSlotUI>(FindObjectsSortMode.None))
            slot.Refresh();
    }
}