using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LootLocker.Requests;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Row prefab — có Rank, Name, Score TMP")]
    public GameObject rowPrefab;
    public Transform content;          // ScrollView Content
    public Button btnRefresh;
    public GameObject loading;

    void OnEnable()
    {
        btnRefresh.onClick.AddListener(Refresh);
        Refresh();
    }

    void OnDisable()
    {
        btnRefresh.onClick.RemoveListener(Refresh);
    }

    void Refresh()
    {
        loading.SetActive(true);
        // clear rows cũ
        foreach (Transform c in content) Destroy(c.gameObject);

        LeaderboardManager.Instance.GetTopScores(20, OnReceived);
    }

    void OnReceived(LootLockerLeaderboardMember[] items)
    {
        loading.SetActive(false);
        if (items == null) return;

        string myID = LeaderboardManager.Instance.GetPlayerID();

        foreach (var item in items)
        {
            GameObject row = Instantiate(rowPrefab, content);
            var txts = row.GetComponentsInChildren<TextMeshProUGUI>();

            txts[0].text = $"#{item.rank}";
            txts[1].text = string.IsNullOrEmpty(item.player.name) 
                ? $"Guest_{item.player.id}"
                : item.player.name;
            txts[2].text = item.score.ToString();

            bool isMe = item.player.id.ToString() == myID;
            txts[0].color = isMe ? Color.yellow : Color.white;
            txts[1].color = isMe ? Color.yellow : Color.white;
            txts[2].color = isMe ? Color.yellow : Color.white;
        }
    }
}