// SaveSystem.cs
using UnityEngine;
using System;

public static class SaveSystem
{
    const string HIGH_SCORE = "HIGH_SCORE";
    const string MAX_STAGE = "MAX_STAGE";
    const string TOTAL_APPLES = "TOTAL_APPLES";
    const string LIVES = "LIVES";
    const string CHALLENGE_PROGRESS = "CHALLENGE_";


    public static event Action OnApplesChanged;
    public static event Action OnLivesChanged;
    public static void SaveHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt(HIGH_SCORE, 0))
            PlayerPrefs.SetInt(HIGH_SCORE, score);
    }

    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE, 0);
    }

    public static void SaveMaxStage(int stage)
    {
        if (stage > PlayerPrefs.GetInt(MAX_STAGE, 1))
            PlayerPrefs.SetInt(MAX_STAGE, stage);
    }

    public static int LoadMaxStage()
    {
        return PlayerPrefs.GetInt(MAX_STAGE, 1);
    }

    public static void AddApples(int apples)
    {
        int total = PlayerPrefs.GetInt(TOTAL_APPLES, 0);
        PlayerPrefs.SetInt(TOTAL_APPLES, total + apples);
        PlayerPrefs.Save();
        OnApplesChanged?.Invoke();
    }

    public static int LoadApples()
    {
        return PlayerPrefs.GetInt(TOTAL_APPLES, 0);
    }

    public static void SaveLives(int lives)
    {
        PlayerPrefs.SetInt(LIVES, lives);
        PlayerPrefs.Save();
        OnLivesChanged?.Invoke(); // thêm dòng này
    }

    public static int LoadLives()
    {
        return PlayerPrefs.GetInt(LIVES, 0);
    }

    public static void SaveChallengeProgress(string challengeId, int stageIndex)
    {
        PlayerPrefs.SetInt(CHALLENGE_PROGRESS + challengeId, stageIndex);
        PlayerPrefs.Save();
    }

    public static int LoadChallengeProgress(string challengeId)
    {
        return PlayerPrefs.GetInt(CHALLENGE_PROGRESS + challengeId, 0);
    }

    public static bool IsChallengeCompleted(string challengeId, int totalStages)
    {
        return LoadChallengeProgress(challengeId) >= totalStages;
    }

    // PowerUp global

    public static void SavePowerUpLevel(int level)
    {
        PlayerPrefs.SetInt("PU_LEVEL", level);
        PlayerPrefs.Save();
    }
    public static int LoadPowerUpLevel() => PlayerPrefs.GetInt("PU_LEVEL", 0);

    public static void SavePowerUpXP(int xp)
    {
        PlayerPrefs.SetInt("PU_XP", xp);
        PlayerPrefs.Save();
    }
    public static int LoadPowerUpXP() => PlayerPrefs.GetInt("PU_XP", 0);

    public static void SaveAdWatchCount(int knifeId, int count)
    {
        PlayerPrefs.SetInt($"AD_WATCH_{knifeId}", count);
        PlayerPrefs.Save();
    }

    public static int LoadAdWatchCount(int knifeId)
        => PlayerPrefs.GetInt($"AD_WATCH_{knifeId}", 0);
}
