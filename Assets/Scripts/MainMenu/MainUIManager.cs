using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainUIManager : MonoBehaviour
{
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI[] apple;
    [SerializeField] private TextMeshProUGUI[] lives;
    [SerializeField] private TextMeshProUGUI highestScore;
    [SerializeField] private TextMeshProUGUI highestStage;

    [Header("Panel")]
    [SerializeField] private GameObject panelShop;
    [SerializeField] private GameObject panelCollection;
    [SerializeField] private GameObject panelSubVIP;
    [SerializeField] private GameObject panelChallenge;
    [SerializeField] private GameObject panelPU;
    [SerializeField] private GameObject panelSetting;
    [SerializeField] private GameObject panelLeaderBoard;

    [Header("Images")]
    [SerializeField] private Image currentKnifeImage; // kéo img vào đây

    [Header("VIP Button")]
    [SerializeField] private Button btnSubVIP;
    [SerializeField] private TextMeshProUGUI txtSubVIP;     // "VIP" / "BECOME VIP"
    
    private bool isReady = false;
    // MainUIManager.cs
    void Start()
    {
        UpdateAllUI();
        isReady = true;
        InventoryManager.OnEquipChanged += UpdateKnifeImage;
        SaveSystem.OnApplesChanged += UpdateAppleUI;
        SaveSystem.OnLivesChanged += UpdateLivesUI; 
        VIPSystem.OnVIPChanged += UpdateVIPButton;
        StartCoroutine(VIPTimerLoop());
    }

    void OnDestroy()
    {
        InventoryManager.OnEquipChanged -= UpdateKnifeImage;
        SaveSystem.OnApplesChanged -= UpdateAppleUI;
        SaveSystem.OnLivesChanged -= UpdateLivesUI;
        VIPSystem.OnVIPChanged -= UpdateVIPButton;
        StopAllCoroutines();
    }

    void UpdateAppleUI()
    {
        foreach (var a in apple)
            a.text = SaveSystem.LoadApples().ToString();
    }

    void UpdateLivesUI()
    {
        foreach (var l in lives)
            l.text = SaveSystem.LoadLives().ToString();
    }

    void OnEnable()
    {
        if (!isReady) return;
        UpdateAllUI();
    }

    void UpdateAllUI()
    {
        foreach (var a in apple)
            a.text = SaveSystem.LoadApples().ToString();
        foreach (var l in lives)
            l.text = SaveSystem.LoadLives().ToString();
        highestScore.text = SaveSystem.LoadHighScore().ToString();
        highestStage.text = SaveSystem.LoadMaxStage().ToString();
        UpdateKnifeImage();
        UpdateVIPButton();
    }
    void UpdateVIPButton()
    {
        if (btnSubVIP == null) return;
        bool isVIP = VIPSystem.IsVIP;
        btnSubVIP.interactable = !isVIP;

        if (txtSubVIP == null) return;

        if (!isVIP)
        {
            txtSubVIP.text = "BECOME VIP";
            return;
        }

        var remaining = VIPSystem.GetTimeRemaining();

        // Lifetime → TimeSpan.MaxValue
        if (remaining == TimeSpan.MaxValue)
        {
            txtSubVIP.text = "VIP LIFETIME";
            return;
        }

        // Có hạn → đếm ngược
        if ((int)remaining.TotalDays > 0)
            txtSubVIP.text = $"VIP {(int)remaining.TotalDays}d {remaining.Hours}h {remaining.Minutes}m {remaining.Seconds}s";
        else
            txtSubVIP.text = $"VIP {remaining.Hours}h {remaining.Minutes}m {remaining.Seconds}s";
    }
    System.Collections.IEnumerator VIPTimerLoop()
    {
        while (true)
        {
            UpdateVIPButton();
            yield return new WaitForSecondsRealtime(1f); 
        }
    }

    void UpdateKnifeImage()
    {
        if (currentKnifeImage == null) return;
        KnifeData data = InventoryManager.Instance.GetEquippedKnifeData();
        if (data != null)
            currentKnifeImage.sprite = data.unlockedSprite;
    }

    public void OnPlayClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Stage");
    }

    public void OnShopClicked()
    {
        panelShop.SetActive(true);
    }


    public void OnBackClicked(GameObject panel)
    {
        panel.SetActive(false);
        UpdateAllUI();
    }

    public void OnCollectionClicked()
    {
        panelCollection.SetActive(true);
    }

    public void OnSubVIPClicked()
    {
        panelSubVIP.SetActive(true);
    }

    public void OnChallengeClicked()
    {
        panelChallenge.SetActive(true);
    }

    public void OnPUClicked()
    {
        panelPU.SetActive(true);
    }

    public void OnSettingClicked()
    {
        panelSetting.SetActive(true);
    }

    public void OnLeaderBoardClicked()
    {
        panelLeaderBoard.SetActive(true);
    }
    public void OnLoadChallengelicked(int id)
    {
        AdsManager.Instance.ShowInterstitial(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene($"BossChallenge{id}");
        });

    }
}