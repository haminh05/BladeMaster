using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PowerUpPanelController : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpSlot
    {
        public Image icon;
        public TextMeshProUGUI label;
        [HideInInspector] public int index;
    }

    [Header("Data")]
    public PowerUpData[] powerUps;   // kéo đúng thứ tự: PU 1,2,3,4

    [Header("Shared XP Bar")]
    public Slider xpSlider;
  
    public TextMeshProUGUI levelText; // "Level 2" hoặc "MAX"

    [Header("Slots — kéo đúng thứ tự với powerUps[]")]
    public PowerUpSlot[] slots;

    void OnEnable()
    {
        PowerUpSystem.OnPowerUpChanged += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        PowerUpSystem.OnPowerUpChanged -= Refresh;
    }

    void Refresh()
    {
        bool isMax = PowerUpSystem.Instance.IsMaxLevel();

        // --- XP Bar ---
        if (isMax)
        {
            xpSlider.value = 1f;          
            levelText.text = "MAX LEVEL";
        }
        else
        {
            int cur = PowerUpSystem.Instance.GetCurrentXP();
            int required = PowerUpSystem.Instance.GetXPRequired();
            xpSlider.value = required > 0 ? (float)cur / required : 0f;
      
            levelText.text = $"Level {PowerUpSystem.Instance.GetGlobalLevel() + 1}";
        }

        // --- Slots ---
        for (int i = 0; i < slots.Length && i < powerUps.Length; i++)
        {
            var data = powerUps[i];
            var slot = slots[i];
            bool unlocked = PowerUpSystem.Instance.IsUnlocked(data.type);

            slot.icon.sprite = unlocked ? data.iconUnlocked : data.iconLocked;
            slot.icon.color = unlocked ? Color.white : new Color(1, 1, 1, 0.4f);
            slot.label.text = unlocked ? data.GetLabel() : $"Power-up {i + 1}";
        }
    }
}