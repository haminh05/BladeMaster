
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppleRewardUIBridge : MonoBehaviour
{
    public Button[] rewardButtons;
    public TextMeshProUGUI[] txtTimer;
   
    public TextMeshProUGUI txtRewardPopup;
    public Transform effectSpawnPoint;

    void OnEnable()
    {
        if (AppleRewardSystem.Instance == null) return;

        // Gán UI của scene này vào system
        AppleRewardSystem.Instance.rewardButtons = rewardButtons;
        AppleRewardSystem.Instance.txtTimer = txtTimer;
       
        AppleRewardSystem.Instance.txtRewardPopup = txtRewardPopup;
        if (effectSpawnPoint != null)
            AppleRewardSystem.Instance.effectSpawnPoint = effectSpawnPoint;

        foreach (var btn in rewardButtons)
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(AppleRewardSystem.Instance.Claim);
            }
    }
}