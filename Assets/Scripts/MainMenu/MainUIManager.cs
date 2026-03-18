using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Images")]
    [SerializeField] private Image currentKnifeImage; // kéo img vào đây
    private bool isReady = false;
    // MainUIManager.cs
    void Start()
    {
        UpdateAllUI();
        isReady = true;
        InventoryManager.OnEquipChanged += UpdateKnifeImage;
        SaveSystem.OnApplesChanged += UpdateAppleUI;
        SaveSystem.OnLivesChanged += UpdateLivesUI; // thêm dòng này
    }

    void OnDestroy()
    {
        InventoryManager.OnEquipChanged -= UpdateKnifeImage;
        SaveSystem.OnApplesChanged -= UpdateAppleUI;
        SaveSystem.OnLivesChanged -= UpdateLivesUI; // thêm dòng này
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
}