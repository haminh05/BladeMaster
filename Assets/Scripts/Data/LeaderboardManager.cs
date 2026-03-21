using UnityEngine;
using LootLocker.Requests;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    const string LEADERBOARD_KEY = "high_score";  // khớp với key trên dashboard
    const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        StartSession();
    }

    // ── Khởi động session (bắt buộc trước khi làm gì) ──
    void StartSession()
    {
        LootLockerSDKManager.StartGuestSession(response =>
        {
            if (response.success)
            {
                Debug.Log("[LootLocker] Session started: " + response.player_id);
                PlayerPrefs.SetString("LL_PLAYER_ID", response.player_id.ToString());
            }
            else
                Debug.LogWarning("[LootLocker] Session failed: " + response.errorData);
        });
    }
    public string GetPlayerID()
    {
        return PlayerPrefs.GetString("LL_PLAYER_ID", "");
    }
    // ── Submit điểm ──
    public void SubmitScore(int score)
    {
        string playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player");

        LootLockerSDKManager.SubmitScore(playerName, score, LEADERBOARD_KEY, response =>
        {
            if (response.success)
                Debug.Log($"[LootLocker] Score submitted: {score}");
            else
                Debug.LogWarning("[LootLocker] Submit failed: " + response.errorData);
        });
    }

    // ── Lấy top scores ──
    public void GetTopScores(int count, System.Action<LootLockerLeaderboardMember[]> onResult)
    {
        LootLockerSDKManager.GetScoreList(LEADERBOARD_KEY, count, response =>
        {
            if (response.success)
                onResult?.Invoke(response.items);
            else
            {
                Debug.LogWarning("[LootLocker] Get scores failed: " + response.errorData);
                onResult?.Invoke(null);
            }
        });
    }

    // ── Đặt tên player ──
    public void SetPlayerName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, name);
        PlayerPrefs.Save();

        LootLockerSDKManager.SetPlayerName(name, response =>
        {
            if (!response.success)
                Debug.LogWarning("[LootLocker] Set name failed: " + response.errorData);
        });
    }

    public string GetPlayerName()
        => PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player");
}