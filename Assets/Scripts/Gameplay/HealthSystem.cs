using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    // số mạng dự trữ người chơi mua
    public int extraLives = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StageUIManager.Instance.SetLives(extraLives);
    }

    // khi player fail
    public void PlayerFailed()
    {
        if (extraLives > 0)
        {
            extraLives--;

            StageUIManager.Instance.SetLives(extraLives);

            Debug.Log("Use extra life. Remaining: " + extraLives);

            // cho chơi tiếp
            KnifeSpawner.Instance.SpawnKnife();
        }
        else
        {
            GameOver();
        }
    }
    public int GetLives()
    {
        return extraLives;
    }
    // khi người chơi mua thêm máu
    public void AddLives(int amount)
    {
        extraLives += amount;

        StageUIManager.Instance.SetLives(extraLives);
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0f;
    }
}