using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Toggle Images — dùng chung")]
    public Sprite spriteOn;
    public Sprite spriteOff;

    [Header("Toggle Buttons")]
    public Button btnSound;
    public Button btnVibration;
    public Button btnLeftHand;

    [Header("Restore Purchase")]
    public Button btnRestore;
    public GameObject panelRestoreResult;
    public TMPro.TextMeshProUGUI txtRestoreResult;

    void OnEnable()
    {
        var s = SettingsManager.Instance;

        RefreshAll(s.Sound, s.Vibration, s.LeftHand);

        btnSound.onClick.AddListener(OnSoundClick);
        btnVibration.onClick.AddListener(OnVibrationClick);
        btnLeftHand.onClick.AddListener(OnLeftHandClick);
        btnRestore.onClick.AddListener(OnRestorePurchase);
    }

    void OnDisable()
    {
        btnSound.onClick.RemoveListener(OnSoundClick);
        btnVibration.onClick.RemoveListener(OnVibrationClick);
        btnLeftHand.onClick.RemoveListener(OnLeftHandClick);
        btnRestore.onClick.RemoveListener(OnRestorePurchase);
    }

    void OnSoundClick()
    {
        bool next = !SettingsManager.Instance.Sound;
        SettingsManager.Instance.SetSound(next);
        SetButtonSprite(btnSound, next);
    }

    void OnVibrationClick()
    {
        bool next = !SettingsManager.Instance.Vibration;
        SettingsManager.Instance.SetVibration(next);
        SetButtonSprite(btnVibration, next);
    }

    void OnLeftHandClick()
    {
        bool next = !SettingsManager.Instance.LeftHand;
        SettingsManager.Instance.SetLeftHand(next);
        SetButtonSprite(btnLeftHand, next);
    }

    void RefreshAll(bool sound, bool vibration, bool leftHand)
    {
        SetButtonSprite(btnSound, sound);
        SetButtonSprite(btnVibration, vibration);
        SetButtonSprite(btnLeftHand, leftHand);
    }
    void SetButtonSprite(Button btn, bool isOn)
    {
        btn.GetComponent<Image>().sprite = isOn ? spriteOn : spriteOff;
    }
    // ── Restore Purchase ──
    void OnRestorePurchase()
    {
        btnRestore.interactable = false;
        IAPManager.Instance.RestorePurchases(OnRestoreResult);
    }

    void OnRestoreResult(bool success, string message)
    {
        btnRestore.interactable = true;
        if (panelRestoreResult != null)
        {
            panelRestoreResult.SetActive(true);
            if (txtRestoreResult != null)
                txtRestoreResult.text = message;
        }
    }
}