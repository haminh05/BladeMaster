using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("AudioSource")]
    [SerializeField] private AudioSource sfxSource;

    [Header("UI")]
    public AudioClip sfxButton;
    public AudioClip sfxEquipKnife;
    public AudioClip sfxDailyReward;

    [Header("Gameplay")]
    public AudioClip sfxKnifeHitTarget;
    public AudioClip sfxKnifeHitKnife;
    public AudioClip sfxKnifeHitApple;
    public AudioClip sfxTargetClear;
    public AudioClip sfxGameOver;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Play(AudioClip clip)
    {
        if (clip == null || !SettingsManager.Instance.Sound) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButton() => Play(sfxButton);
    public void PlayEquipKnife() => Play(sfxEquipKnife);
    public void PlayDailyReward() => Play(sfxDailyReward);
    public void PlayHitTarget() => Play(sfxKnifeHitTarget);
    public void PlayHitKnife() => Play(sfxKnifeHitKnife);
    public void PlayHitApple() => Play(sfxKnifeHitApple);
    public void PlayTargetClear() => Play(sfxTargetClear);
    public void PlayGameOver() => Play(sfxGameOver);
}