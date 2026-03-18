// AppleRewardUI.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AppleRewardUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button[] rewardButtons; // gán cả main menu lẫn game over

    [Header("UI")]
    public TextMeshProUGUI[] txtTimer;      // đếm ngược thời gian
    public TextMeshProUGUI[] txtAccumulated; // táo đang tích lũy

    [Header("Reward Effect")]
    public GameObject appleEffectPrefab;    // prefab táo rơi
    public Transform effectSpawnPoint;      // vị trí spawn táo
    public TextMeshProUGUI txtRewardPopup;  // "+50 Apples!"
    public float popupDuration = 2f;

    void OnEnable()
    {
        UpdateUI();
        StartCoroutine(TimerLoop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator TimerLoop()
    {
        while (true)
        {
            UpdateUI();
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    void UpdateUI()
    {
        if (AppleRewardSystem.Instance == null) return;

        bool canClaim = AppleRewardSystem.Instance.CanClaim();
        int accumulated = AppleRewardSystem.Instance.GetAccumulatedApples();
        var timeLeft = AppleRewardSystem.Instance.GetTimeUntilNext();

        // Cập nhật buttons
        foreach (var btn in rewardButtons)
            if (btn != null) btn.interactable = canClaim;

        // Cập nhật txt tích lũy
        foreach (var txt in txtAccumulated)
            if (txt != null) txt.text = $"+{accumulated} 🍎";

        // Cập nhật timer
        string timerStr = canClaim
            ? "READY!"
            : $"{timeLeft.Hours:00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";

        foreach (var txt in txtTimer)
            if (txt != null) txt.text = timerStr;
    }

    // Gắn vào tất cả reward buttons
    //public void OnClaimReward()
    //{
    //    if (!AppleRewardSystem.Instance.CanClaim()) return;

    //   // int amount = AppleRewardSystem.Instance.Claim();
    //    UpdateUI();
    //    StartCoroutine(ShowRewardEffect(amount));
    //}

    IEnumerator ShowRewardEffect(int amount)
    {
        // Spawn táo rơi
        if (appleEffectPrefab != null && effectSpawnPoint != null)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-100f, 100f),
                    Random.Range(50f, 150f), 0);
                Instantiate(appleEffectPrefab,
                    effectSpawnPoint.position + offset,
                    Quaternion.identity,
                    effectSpawnPoint);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        // Hiện popup text
        if (txtRewardPopup != null)
        {
            txtRewardPopup.gameObject.SetActive(true);
            txtRewardPopup.text = $"+{amount} Apples!";
            yield return new WaitForSecondsRealtime(popupDuration);
            txtRewardPopup.gameObject.SetActive(false);
        }
    }
}