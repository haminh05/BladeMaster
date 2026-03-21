using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpSystem : MonoBehaviour
{
    public static PowerUpSystem Instance;

    [Header("Data — kéo đúng thứ tự, index = unlock level - 1")]
    public PowerUpData[] allPowerUps;   // PU[0] unlock ở level 1, PU[1] ở level 2...

    [Header("XP cần để lên mỗi level — index 0 = lên level 1")]
    public int[] xpPerLevel;            // length phải == allPowerUps.length

    private int globalLevel;
    private int currentXP;

    public static event Action OnPowerUpChanged;

    void Awake()
    {
        Instance = this;
        globalLevel = SaveSystem.LoadPowerUpLevel();
        currentXP = SaveSystem.LoadPowerUpXP();
    }

    // ── Query ──

    public int GetGlobalLevel() => globalLevel;
    public int GetCurrentXP() => currentXP;
    public bool IsMaxLevel() => globalLevel >= allPowerUps.Length;

    public int GetXPRequired()
    {
        if (IsMaxLevel()) return 0;
        return xpPerLevel[Mathf.Clamp(globalLevel, 0, xpPerLevel.Length - 1)];
    }

    public bool IsUnlocked(PowerUpType type)
    {
        int idx = GetIndexOf(type);
        return idx >= 0 && globalLevel > idx;
    }

    public float GetValue(PowerUpType type)
    {
        if (!IsUnlocked(type)) return 0f;
        int idx = GetIndexOf(type);
        return allPowerUps[idx].value;
    }

    // ── Add XP ──

    public void AddXP(int amount)
    {
        if (IsMaxLevel()) return;

        amount = Mathf.RoundToInt(amount * GetXPMultiplier());
        currentXP += amount;

        // level up loop
        while (!IsMaxLevel() && currentXP >= GetXPRequired())
        {
            currentXP -= GetXPRequired();
            globalLevel++;
            Debug.Log($"[PowerUp] Global level up → {globalLevel}, unlocked: {allPowerUps[globalLevel - 1].type}");
        }

        if (IsMaxLevel()) currentXP = 0;

        SaveSystem.SavePowerUpLevel(globalLevel);
        SaveSystem.SavePowerUpXP(currentXP);
        OnPowerUpChanged?.Invoke();
    }

    // ── Buff getters ──

    public bool RollRevive()
    {
        float chance = GetValue(PowerUpType.ReviveChance) / 100f;
        return UnityEngine.Random.value < chance;
    }

    public float GetDailyAppleMultiplier()
    {
        return 1f + GetValue(PowerUpType.DailyAppleBonus) / 100f;
    }

    public int GetVideoAppleBonus()
    {
        return Mathf.RoundToInt(GetValue(PowerUpType.VideoAppleBonus));
    }

    public float GetXPMultiplier()
    {
        // XPBoost chưa unlock thì multiplier = 1
        float boost = GetValue(PowerUpType.XPBoost);
        return 1f + boost / 100f;
    }

    // ── Helper ──

    int GetIndexOf(PowerUpType type)
    {
        for (int i = 0; i < allPowerUps.Length; i++)
            if (allPowerUps[i].type == type) return i;
        return -1;
    }
}