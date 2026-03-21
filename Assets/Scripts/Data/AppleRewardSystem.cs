// AppleRewardSystem.cs
using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class AppleRewardSystem : MonoBehaviour
{
    public static AppleRewardSystem Instance;

    [Header("Settings")]
    public int applesPerReward = 50;
    public int maxApples = 200;
    public float intervalHours = 3f;

    [Header("Effect")]
    public GameObject appleEffectPrefab;
    public Transform effectSpawnPoint;
    public TextMeshProUGUI txtRewardPopup;
    public float popupDuration = 2f;

    [Header("UI — tự động cập nhật")]
    public UnityEngine.UI.Button[] rewardButtons;
    public TMPro.TextMeshProUGUI[] txtTimer;
   

    const string LAST_CLAIM_KEY = "LAST_APPLE_REWARD";
    const string ACCUMULATED_KEY = "ACCUMULATED_APPLES";

    void Awake()
    {
        // Tồn tại xuyên scene
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable() => StartCoroutine(TimerLoop());
    void OnDisable() => StopAllCoroutines();

    IEnumerator TimerLoop()
    {
        while (true)
        {
            RefreshUI();
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    // ── Gọi từ bất kỳ button nào ──
    public void Claim()
    {
        if (!CanClaim()) return;
        int amount = DoClain();
        StartCoroutine(ShowEffect(amount));
    }

    // ── Logic ──
    public bool CanClaim() => GetAccumulatedApples() > 0;

    public int GetAccumulatedApples()
    {
        UpdateAccumulated();
        return Mathf.Min(PlayerPrefs.GetInt(ACCUMULATED_KEY, 0), maxApples);
    }

    public TimeSpan GetTimeUntilNext()
    {
        DateTime next = GetLastClaim().AddHours(intervalHours);
        TimeSpan remaining = next - DateTime.UtcNow;
        return remaining.TotalSeconds > 0 ? remaining : TimeSpan.Zero;
    }

    int DoClain()
    {
        int baseAmount = GetAccumulatedApples();
        float multiplier = VIPSystem.RewardMultiplier
                         * PowerUpSystem.Instance.GetDailyAppleMultiplier();
        int amount = Mathf.RoundToInt(baseAmount * multiplier);
        SaveSystem.AddApples(amount);
        PlayerPrefs.SetInt(ACCUMULATED_KEY, 0);
        PlayerPrefs.SetString(LAST_CLAIM_KEY, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
        SoundManager.Instance.PlayDailyReward();
        return amount;
    }

    void UpdateAccumulated()
    {
        DateTime last = GetLastClaim();
        int intervals = Mathf.FloorToInt((float)(DateTime.UtcNow - last).TotalHours / intervalHours);
        if (intervals <= 0) return;

        int current = PlayerPrefs.GetInt(ACCUMULATED_KEY, 0);
        PlayerPrefs.SetInt(ACCUMULATED_KEY, Mathf.Min(current + intervals * applesPerReward, maxApples));
        PlayerPrefs.SetString(LAST_CLAIM_KEY, last.AddHours(intervals * intervalHours).ToString());
        PlayerPrefs.Save();
    }

    DateTime GetLastClaim()
    {
        string saved = PlayerPrefs.GetString(LAST_CLAIM_KEY, "");
        if (DateTime.TryParse(saved, out DateTime dt)) return dt;
        DateTime now = DateTime.UtcNow;
        PlayerPrefs.SetString(LAST_CLAIM_KEY, now.ToString());
        PlayerPrefs.Save();
        return now;
    }

    // ── UI ──
    void RefreshUI()
    {
        bool canClaim = CanClaim();
        string timerStr = canClaim
            ? "READY!"
            : $"{GetTimeUntilNext().Hours:00}:{GetTimeUntilNext().Minutes:00}:{GetTimeUntilNext().Seconds:00}";

        foreach (var btn in rewardButtons)
            if (btn != null) btn.interactable = canClaim;

        foreach (var txt in txtTimer)
            if (txt != null) txt.text = timerStr;
    }

    // ── Effect ──
    IEnumerator ShowEffect(int amount)
    {
        if (appleEffectPrefab != null && effectSpawnPoint != null)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 offset = new Vector3(
                    UnityEngine.Random.Range(-100f, 100f),
                    UnityEngine.Random.Range(50f, 150f), 0);

                GameObject apple = Instantiate(appleEffectPrefab,
                    effectSpawnPoint.position + offset,
                    Quaternion.identity, effectSpawnPoint);

                Destroy(apple, 1f); 
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        if (txtRewardPopup != null)
        {
            txtRewardPopup.gameObject.SetActive(true);
            txtRewardPopup.text = $"+{amount} Apples!";
            yield return new WaitForSecondsRealtime(popupDuration);
            txtRewardPopup.gameObject.SetActive(false);
        }
    }
}