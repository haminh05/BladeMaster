using UnityEngine;

public class LeftHandLayout : MonoBehaviour
{
    public GameObject panelRightHand;  // mặc định
    public GameObject panelLeftHand;   // khi bật left hand

    void OnEnable()
    {
        SettingsManager.OnLeftHandChanged += Apply;
        Apply(SettingsManager.Instance.LeftHand);
    }

    void OnDisable()
    {
        SettingsManager.OnLeftHandChanged -= Apply;
    }

    void Apply(bool isLeftHand)
    {
        panelRightHand.SetActive(!isLeftHand);
        panelLeftHand.SetActive(isLeftHand);
    }
}