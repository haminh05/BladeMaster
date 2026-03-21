using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    const string SOUND_KEY = "SETTING_SOUND";
    const string VIBRATION_KEY = "SETTING_VIBRATION";
    const string LEFTHAND_KEY = "SETTING_LEFTHAND";

    public static event Action<bool> OnSoundChanged;
    public static event Action<bool> OnVibrationChanged;
    public static event Action<bool> OnLeftHandChanged;

    public bool Sound { get; private set; }
    public bool Vibration { get; private set; }
    public bool LeftHand { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Sound = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        Vibration = PlayerPrefs.GetInt(VIBRATION_KEY, 1) == 1;
        LeftHand = PlayerPrefs.GetInt(LEFTHAND_KEY, 0) == 1;
    }

    public void SetSound(bool value)
    {
        Sound = value;
        PlayerPrefs.SetInt(SOUND_KEY, value ? 1 : 0);
        PlayerPrefs.Save();
        AudioListener.pause = !value;
        OnSoundChanged?.Invoke(value);
    }

    public void SetVibration(bool value)
    {
        Vibration = value;
        PlayerPrefs.SetInt(VIBRATION_KEY, value ? 1 : 0);
        PlayerPrefs.Save();
        OnVibrationChanged?.Invoke(value);
    }

    public void SetLeftHand(bool value)
    {
        LeftHand = value;
        PlayerPrefs.SetInt(LEFTHAND_KEY, value ? 1 : 0);
        PlayerPrefs.Save();
        OnLeftHandChanged?.Invoke(value);
    }

    // Rung nếu vibration bật — gọi ở nơi cần
    public void Vibrate(long milliseconds = 50)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Vibration) Handheld.Vibrate();
#endif
    }
}