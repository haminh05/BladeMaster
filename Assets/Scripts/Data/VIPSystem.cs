// VIPSystem.cs
using UnityEngine;
using System;

public static class VIPSystem
{
    const string VIP_KEY = "VIP";
    const string VIP_EXPIRY_KEY = "VIP_EXPIRY";

    // ── Check trạng thái ──
    public static bool IsVIP
    {
        get
        {
            if (PlayerPrefs.GetInt(VIP_KEY, 0) == 0) return false;

            // Check hết hạn nếu có expiry
            string expiryStr = PlayerPrefs.GetString(VIP_EXPIRY_KEY, "");
            if (string.IsNullOrEmpty(expiryStr)) return true; // lifetime

            if (DateTime.TryParse(expiryStr, out DateTime expiry))
                return DateTime.UtcNow < expiry;

            return false;
        }
    }

    public static bool IsAdsRemoved => IsVIP ||
        PlayerPrefs.GetInt("REMOVE_ADS", 0) == 1;

    // Multiplier
    public static float RewardMultiplier => IsVIP ? 2f : 1f;  // 200%
    public static float XPMultiplier => IsVIP ? 2f : 1f;  // 200%

    // ── Kích hoạt VIP ──
    public static void ActivateVIP(int durationDays = 0)
    {
        PlayerPrefs.SetInt(VIP_KEY, 1);

        if (durationDays > 0)
        {
            DateTime expiry = DateTime.UtcNow.AddDays(durationDays);
            PlayerPrefs.SetString(VIP_EXPIRY_KEY, expiry.ToString());
        }
        else
        {
            // Lifetime — xóa expiry
            PlayerPrefs.DeleteKey(VIP_EXPIRY_KEY);
        }

        PlayerPrefs.Save();
        OnVIPChanged?.Invoke();
    }

    public static void DeactivateVIP()
    {
        PlayerPrefs.SetInt(VIP_KEY, 0);
        PlayerPrefs.DeleteKey(VIP_EXPIRY_KEY);
        PlayerPrefs.Save();
        OnVIPChanged?.Invoke();
    }

    // Thời gian còn lại
#if UNITY_EDITOR
    public static void ActivateVIPTest(int seconds)
    {
        // Xóa key cũ trước
        PlayerPrefs.DeleteKey(VIP_EXPIRY_KEY);

        PlayerPrefs.SetInt(VIP_KEY, 1);
        DateTime expiry = DateTime.UtcNow.AddSeconds(seconds);
        PlayerPrefs.SetString(VIP_EXPIRY_KEY, expiry.ToString());
        PlayerPrefs.Save();

        // Verify
        string saved = PlayerPrefs.GetString(VIP_EXPIRY_KEY, "");
        Debug.Log($"VIP TEST saved: '{saved}', expires: {expiry}");

        OnVIPChanged?.Invoke();
    }

    public static void ResetVIP()
    {
        PlayerPrefs.DeleteKey(VIP_KEY);
        PlayerPrefs.DeleteKey(VIP_EXPIRY_KEY);
        PlayerPrefs.Save();
        OnVIPChanged?.Invoke();
        Debug.Log("VIP Reset!");
    }
#endif
    public static TimeSpan GetTimeRemaining()
    {
        string expiryStr = PlayerPrefs.GetString(VIP_EXPIRY_KEY, "");
        if (string.IsNullOrEmpty(expiryStr)) return TimeSpan.MaxValue; // lifetime

        if (DateTime.TryParse(expiryStr, out DateTime expiry))
        {
            TimeSpan remaining = expiry - DateTime.UtcNow;
            return remaining.TotalSeconds > 0 ? remaining : TimeSpan.Zero;
        }
        return TimeSpan.Zero;
    }

    public static event Action OnVIPChanged;
}