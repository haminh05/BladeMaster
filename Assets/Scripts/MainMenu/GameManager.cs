using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }





    public void OnFBKetClicked()
    {
        Application.OpenURL("https://www.facebook.com/ketchappgames");
    }
    public void OnIGKetClicked()
    {
        Application.OpenURL("https://www.instagram.com/ketchapp");
    }

    public void OnFBEsClicked()
    {
        Application.OpenURL("https://www.facebook.com/estoty");
    }
    public void OnIGEsClicked()
    {
        Application.OpenURL("https://www.instagram.com/estoty_games");
    }
 
}
