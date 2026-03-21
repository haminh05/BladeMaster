using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(
            () => SoundManager.Instance.PlayButton()
        );
    }
}