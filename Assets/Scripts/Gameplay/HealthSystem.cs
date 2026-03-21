using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;
    public GameObject panelOver;
    private int extraLives = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Load lives từ SaveSystem thay vì dùng giá trị hardcode
        extraLives = SaveSystem.LoadLives();
        StageUIManager.Instance.SetLives(extraLives);
    }

    public void PlayerFailed()
    {
        if (extraLives > 0 || PowerUpSystem.Instance.RollRevive())
        {
            if (extraLives > 0)
            {
                extraLives--;
                SaveSystem.SaveLives(extraLives);
                StageUIManager.Instance.SetLives(extraLives);
            }
            KnifeSpawner.Instance.SpawnKnife();
        }
        else GameOver();
    }

    public int GetLives() => extraLives;

    public void AddLives(int amount)
    {
        extraLives += amount;
        SaveSystem.SaveLives(extraLives);
        StageUIManager.Instance.SetLives(extraLives);
    }

    void GameOver()
    {
        SoundManager.Instance.PlayGameOver();
        Debug.Log("GAME OVER");
        LeaderboardManager.Instance.SubmitScore(SaveSystem.LoadHighScore());
        Time.timeScale = 0f;
        panelOver.SetActive(true);
    }
}