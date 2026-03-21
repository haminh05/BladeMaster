using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelWin : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelSetting;
    public GameObject panelLeaderboard;
    public GameObject panelCollection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnSettingClicked() => panelSetting.SetActive(true);
    public void OnLeaderboardClicked() => panelLeaderboard.SetActive(true);
    public void OnCollectionClicked() => panelCollection.SetActive(true);
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

