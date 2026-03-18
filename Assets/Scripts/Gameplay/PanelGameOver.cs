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


    public static PanelGameOver Instance;
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
        // mua mạng

        SaveSystem.SaveLives(9);

        Time.timeScale = 1f;

        panelGameOver1.SetActive(false);

        StageUIManager.Instance.UpdateAllUI();

        KnifeSpawner.Instance.SpawnKnife();
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
        Debug.Log("Share game to social media!");
    }

    public void OnAddAppleClicked() 
    {
        //xử lý ads

        SceneManager.LoadScene("MainMenu");
    }
}
