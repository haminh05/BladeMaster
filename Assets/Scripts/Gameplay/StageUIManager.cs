using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageUIManager : MonoBehaviour
{
    public static StageUIManager Instance;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI numberStageText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI appleText;
    public TextMeshProUGUI livesText;

    [Header("Knife Count Display")] 
    [SerializeField] private GameObject panelKnivesRight;
    [SerializeField] private GameObject panelKnivesLeft;
    [SerializeField]
    private GameObject iconKnife;
    [SerializeField]
    private Color usedKnifeIconColor;
    private GameObject panelKnives => SettingsManager.Instance.LeftHand
    ? panelKnivesLeft
    : panelKnivesRight;
    [Header("Progress UI")]
    [SerializeField] private Transform bossPanel;
    [SerializeField] private GameObject dotIcon;
    [SerializeField] private GameObject bossIcon;
    [SerializeField] private Color completedColor; // màu xám cho các dot đã hoàn thành

    [Header("Boss Banner")]
    [SerializeField] private GameObject bossBanner;
    [SerializeField] private TextMeshProUGUI bossText;
    [SerializeField] private Transform bossBannerIcon;
    [SerializeField] private Color bossUIColor;

    private int score;
    private int stage;
    private int apples;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        apples = SaveSystem.LoadApples();
        SetApple(apples);

        int lives = SaveSystem.LoadLives();
        SetLives(lives);

        UpdateAllUI();
    }
    void OnEnable()
    {
        UpdateAllUI();
    }
    public void SetStage(int value)
    {
        stage = value;
        numberStageText.text = stage.ToString();
    }

    public void AddScore(int value)
    {
        score += value;
        SaveSystem.SaveHighScore(score);
        scoreText.text = score.ToString();
    }

    public void SetApple(int value)
    {
        apples = value;
        appleText.text = apples.ToString();
    }

    public void AddApple(int value)
    {
        apples += value;
        SaveSystem.AddApples(value);
        appleText.text = apples.ToString();
    }

    // chỉ hiển thị từ HealthSystem
    public void SetLives(int value)
    {
        livesText.text = value.ToString();
    }

    //KNIFE COUNT FUNC
    public void SetInitialDisplayedKnifeCount(int count)
    {
        // xóa icon cũ
        foreach (Transform child in panelKnives.transform)
            Destroy(child.gameObject);

        knifeIconIndexToChange = 0;

        // tạo icon mới
        for (int i = 0; i < count; i++)
            Instantiate(iconKnife, panelKnives.transform);
    }

  
    private int knifeIconIndexToChange = 0;

    public void DecrementDisplayedKnifeCount()
    {
        if (knifeIconIndexToChange >= panelKnives.transform.childCount)
            return;

        panelKnives.transform
            .GetChild(knifeIconIndexToChange++)
            .GetComponent<Image>()
            .color = usedKnifeIconColor;
    }

    //PROGRESS FUNC

    public void SetupBossProgress(StageConfig[] stages, int currentStage)
    {
        foreach (Transform c in bossPanel)
            Destroy(c.gameObject);

        int bossIndex = FindNextBossIndex(stages, currentStage);

        if (bossIndex == -1) return;

        int stagesToBoss = bossIndex - currentStage;

        for (int i = 0; i < stagesToBoss; i++)
            Instantiate(dotIcon, bossPanel);

        Instantiate(bossIcon, bossPanel);
        bossIconIndex = 0;

        // vì đã bắt đầu stage nên mark dot đầu tiên
        AdvanceBossProgress();
    }
    int FindNextBossIndex(StageConfig[] stages, int start)
    {
        for (int i = start; i < stages.Length; i++)
        {
            if (stages[i].isBoss)
                return i;
        }

        return -1;
    }
    private int bossIconIndex = 0;

    public void AdvanceBossProgress()
    {
        if (bossIconIndex >= bossPanel.childCount - 1)
            return;

        bossPanel.GetChild(bossIconIndex)
            .GetComponent<Image>().color = completedColor;

        bossIconIndex++;
    }

    // BOSS BANNER FUNC
    public void ShowBossBanner(string bossName, GameObject icon)
    {
        numberStageText.gameObject.SetActive(false);
        stageText.gameObject.SetActive(false);
        bossBanner.SetActive(true);

        bossText.text = "BOSS: " + bossName;
        bossText.color = bossUIColor;
        //foreach (Transform child in bossBannerIcon)
        //    Destroy(child.gameObject);
        //GameObject iconObj = Instantiate(icon, bossBannerIcon);
        Image img = icon.GetComponent<Image>();
        if (img != null)
            img.color = bossUIColor;
        
    }
    public void HideBossBanner()
    {
        numberStageText.gameObject.SetActive(true);
        stageText.gameObject.SetActive(true);
        bossBanner.SetActive(false);
    }
    public void ClearBossProgress()
    {
        foreach (Transform c in bossPanel)
            Destroy(c.gameObject);
    }
    //UPFATE UI FUNC
    public void UpdateAllUI()
    {
        scoreText.text = score.ToString();
        numberStageText.text = stage.ToString();
        appleText.text = SaveSystem.LoadApples().ToString();
        livesText.text = SaveSystem.LoadLives().ToString();
    }

    public void OnBackClicked(GameObject panel)
    {
        panel.SetActive(false);
        UpdateAllUI();
    }
}