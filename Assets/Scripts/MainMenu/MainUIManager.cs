using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainUIManager : MonoBehaviour
{
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI apple;
    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI highestScore;
    [SerializeField] private TextMeshProUGUI highestStage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateAllUI();
    }
    void OnEnable()
    {
        UpdateAllUI();
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    public void OnPlayClicked()
    {
        // Load the gameplay scene
        SceneManager.LoadScene("Stage");
    }

    void UpdateAllUI()
    {
        apple.text = SaveSystem.LoadApples().ToString();
        lives.text = SaveSystem.LoadLives().ToString();
        highestScore.text = SaveSystem.LoadHighScore().ToString();
        highestStage.text = SaveSystem.LoadMaxStage().ToString();
    }
}
