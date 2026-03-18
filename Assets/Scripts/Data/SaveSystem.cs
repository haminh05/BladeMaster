using UnityEngine;
//kho dao
public static class SaveSystem
{
    const string HIGH_SCORE = "HIGH_SCORE";
    const string MAX_STAGE = "MAX_STAGE";
    const string TOTAL_APPLES = "TOTAL_APPLES";
    const string LIVES = "LIVES";
    const string UNLOCKED_KNIVES = "UNLOCKED_KNIVES";

    public static void SaveHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt(HIGH_SCORE, 0))
        {
            PlayerPrefs.SetInt(HIGH_SCORE, score);
        }
    }

    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE, 0);
    }

    public static void SaveMaxStage(int stage)
    {
        if (stage > PlayerPrefs.GetInt(MAX_STAGE, 1))
        {
            PlayerPrefs.SetInt(MAX_STAGE, stage);
        }
    }

    public static int LoadMaxStage()
    {
        return PlayerPrefs.GetInt(MAX_STAGE, 1);
    }

    public static void AddApples(int apples)
    {
        int total = PlayerPrefs.GetInt(TOTAL_APPLES, 0);
        PlayerPrefs.SetInt(TOTAL_APPLES, total + apples);
    }

    public static int LoadApples()
    {
        return PlayerPrefs.GetInt(TOTAL_APPLES, 0);
    }

    public static void SaveLives(int lives)
    {
        PlayerPrefs.SetInt("LIVES", lives);
        PlayerPrefs.Save();
    }

    public static int LoadLives()
    {
        return PlayerPrefs.GetInt("LIVES", 0);
    }
}