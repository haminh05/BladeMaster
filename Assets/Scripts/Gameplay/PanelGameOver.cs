using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelGameOver : MonoBehaviour
{
    [Header("Panel Game Over 1")]
    [SerializeField] private GameObject panelGameOver1;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI stage;

    [Header("Panel Game Over 2")]
    [SerializeField] private GameObject panelGameOver2;
    [SerializeField] private TextMeshProUGUI score1;
    [SerializeField] private TextMeshProUGUI stage1;

    [Header("Panels")]
    public GameObject panelSetting;
    public GameObject panelLeaderboard;
    public GameObject panelCollection;
    public static PanelGameOver Instance;

    public void OnSettingClicked() => panelSetting.SetActive(true);
    public void OnLeaderboardClicked() => panelLeaderboard.SetActive(true);
    public void OnCollectionClicked() => panelCollection.SetActive(true);
    void Start()
    {
        
    }
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf)
        {
            Over1();
        }
        if (panelGameOver2.activeSelf)
        {
            Over2();
        }
    }

    public void Over1()
    {
        score.text = StageUIManager.Instance.scoreText.text;
        stage.text = StageUIManager.Instance.numberStageText.text;
    }

    public void Over2()
    {
        score1.text = StageUIManager.Instance.scoreText.text;
        stage1.text = StageUIManager.Instance.numberStageText.text;
    }
    public void OnContinuesClicked()
    {
        // quảng cáo

        Time.timeScale = 1f;

        panelGameOver1.SetActive(false);
  
        StageUIManager.Instance.UpdateAllUI();

        KnifeSpawner.Instance.SpawnKnife();
    }

    public void OnExtraLivesClicked()
    {
        IAPManager.Instance.BuyProduct("4");  // productID trong IAPPackageData
    }

    public void OnNoThanksClicked()
    {
        
        panelGameOver2.SetActive(true);
        panelGameOver1.SetActive(false);
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnHomeClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnShareClicked()
    {
#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", "android.intent.action.SEND");
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", "android.intent.extra.TEXT",
            $"Tôi đạt {SaveSystem.LoadHighScore()} điểm trong Knife Hit! Bạn có thể vượt qua không?");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Chia sẻ điểm số");
        currentActivity.Call("startActivity", chooser);
#else
    Debug.Log("Share không khả dụng trên nền tảng này.");
#endif
    }

    public void OnAddAppleClicked() 
    {
        //xử lý ads

        SceneManager.LoadScene("MainMenu");
    }
}
