using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevMode : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Input")]
    public TMP_InputField inputAmount;

    [Header("Info")]
    public TextMeshProUGUI txtInfo;

    // ── Toggle panel ──
    public void TogglePanel() => panel.SetActive(!panel.activeSelf);

    // ── APPLES ──
    public void AddApples()
    {
        int amount = GetAmount();
        SaveSystem.AddApples(amount);
        Log($"+{amount} táo → {SaveSystem.LoadApples()}");
    }

    public void ResetApples()
    {
        PlayerPrefs.SetInt("TOTAL_APPLES", 0);
        PlayerPrefs.Save();
        Log("Reset táo → 0");
    }

    // ── LIVES ──
    public void AddLives()
    {
        int amount = GetAmount();
        SaveSystem.SaveLives(SaveSystem.LoadLives() + amount);
        Log($"+{amount} mạng → {SaveSystem.LoadLives()}");
    }

    public void ResetLives()
    {
        SaveSystem.SaveLives(0);
        Log("Reset mạng → 0");
    }

    public void MaxLives()
    {
        SaveSystem.SaveLives(99);
        Log("Mạng → 99");
    }

    // ── VIP ──
    public void ActivateVIPLifetime()
    {
        VIPSystem.ActivateVIP(0);
        Log("VIP Lifetime ON");
    }

    public void ActivateVIP30Days()
    {
        VIPSystem.ActivateVIP(30);
        Log("VIP 30 ngày ON");
    }

    public void ResetVIP()
    {
#if UNITY_EDITOR
        VIPSystem.ResetVIP();
        Log("VIP reset");
#endif
    }

    public void ActivateVIPTest10s()
    {
#if UNITY_EDITOR
        VIPSystem.ActivateVIPTest(10);
        Log("VIP test 10s");
#endif
    }

    // ── REMOVE ADS ──
    public void RemoveAds()
    {
        PlayerPrefs.SetInt("REMOVE_ADS", 1);
        PlayerPrefs.Save();
        Log("Ads removed");
    }

    public void RestoreAds()
    {
        PlayerPrefs.SetInt("REMOVE_ADS", 0);
        PlayerPrefs.Save();
        Log("Ads restored");
    }

    // ── KNIVES ──
    public void UnlockAllKnives()
    {
        foreach (var knife in InventoryManager.Instance.knifeDatabase.knives)
            InventoryManager.Instance.UnlockKnife(knife.id);
        Log("Unlock tất cả dao");
    }

    public void LockAllKnives()
    {
        foreach (var knife in InventoryManager.Instance.knifeDatabase.knives)
            if (!knife.isDefault)
                PlayerPrefs.SetInt("KNIFE_" + knife.id, 0);
        PlayerPrefs.Save();
        Log("Lock tất cả dao (trừ default)");
    }

    // ── SCORE ──
    public void AddScore()
    {
        int amount = GetAmount();
        SaveSystem.SaveHighScore(SaveSystem.LoadHighScore() + amount);
        Log($"High score → {SaveSystem.LoadHighScore()}");
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt("HIGH_SCORE", 0);
        PlayerPrefs.Save();
        Log("Reset score → 0");
    }

    // ── STAGE ──
    public void SetMaxStage()
    {
        int amount = GetAmount();
        PlayerPrefs.SetInt("MAX_STAGE", amount);
        PlayerPrefs.Save();
        Log($"Max stage → {amount}");
    }

    public void ResetStage()
    {
        PlayerPrefs.SetInt("MAX_STAGE", 1);
        PlayerPrefs.Save();
        Log("Reset stage → 1");
    }

    // ── POWERUP ──
    public void AddPowerUpXP()
    {
        int amount = GetAmount();
        PowerUpSystem.Instance.AddXP(amount);
        Log($"+{amount} XP powerup → level {PowerUpSystem.Instance.GetGlobalLevel()}");
    }

    public void ResetPowerUp()
    {
        SaveSystem.SavePowerUpLevel(0);
        SaveSystem.SavePowerUpXP(0);
        Log("Reset PowerUp → level 0");
    }

    public void MaxPowerUp()
    {
        // Thêm XP nhiều để max hết
        PowerUpSystem.Instance.AddXP(999999);
        Log($"Max PowerUp → level {PowerUpSystem.Instance.GetGlobalLevel()}");
    }

    // ── CHALLENGE ──
    public void ResetAllChallenges()
    {
        // Xóa tất cả key CHALLENGE_
        foreach (var key in new string[] { "monster", "dragon", "ninja" }) // thêm id thật vào đây
            PlayerPrefs.DeleteKey("CHALLENGE_" + key);
        PlayerPrefs.Save();
        Log("Reset tất cả challenge");
    }

    // ── RESET ALL ──
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Log("⚠️ RESET TẤT CẢ DỮ LIỆU");
    }

    // ── PRINT STATE ──
    public void PrintState()
    {
        Log($"Táo: {SaveSystem.LoadApples()}\n" +
            $"Mạng: {SaveSystem.LoadLives()}\n" +
            $"Score: {SaveSystem.LoadHighScore()}\n" +
            $"Stage: {SaveSystem.LoadMaxStage()}\n" +
            $"VIP: {VIPSystem.IsVIP}\n" +
            $"Ads removed: {VIPSystem.IsAdsRemoved}\n" +
            $"PU Level: {PowerUpSystem.Instance.GetGlobalLevel()}\n" +
            $"PU XP: {PowerUpSystem.Instance.GetCurrentXP()}");
    }

    // ── HELPER ──
    int GetAmount()
    {
        if (inputAmount == null) return 100;
        return int.TryParse(inputAmount.text, out int val) ? val : 100;
    }

    public void OnDevModeClicked() { panel.SetActive(true); }
    void Log(string msg)
    {
        Debug.Log($"[DevMode] {msg}");
        if (txtInfo != null) txtInfo.text = msg;
    }
}