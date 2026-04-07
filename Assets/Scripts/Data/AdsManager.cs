using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

#if UNITY_ANDROID
    //const string BANNER_ID       = "ca-app-pub-1867189104185319/7333490959";
    //const string INTERSTITIAL_ID = "ca-app-pub-1867189104185319/5340217583";
    //const string REWARDED_ID     = "ca-app-pub-1867189104185319/5401729086";
    const string BANNER_ID = "ca-app-pub-3940256099942544/6300978111"; // test ID
    const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712"; // test ID
    const string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    const string BANNER_ID       = "ca-app-pub-3940256099942544/2934735716";
    const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/4411468910";
    const string REWARDED_ID     = "ca-app-pub-3940256099942544/1712485313";
#else
    const string BANNER_ID       = "unused";
    const string INTERSTITIAL_ID = "unused";
    const string REWARDED_ID     = "unused";
#endif

    private BannerView      bannerView;
    private InterstitialAd  interstitialAd;
    private RewardedAd      rewardedAd;

    private Action       onInterstitialClosed;
    private ScreenOrientation currentOrientation;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            if (initStatus == null) { Debug.LogError("[Ads] Init failed"); return; }
            Debug.Log("[Ads] Initialized");
            currentOrientation = Screen.orientation;
            LoadBanner();
            LoadInterstitial();
            LoadRewarded();
        });
    }

    void Update()
    {
        // Banner tự recreate khi xoay màn hình
        if (Screen.orientation != currentOrientation)
        {
            currentOrientation = Screen.orientation;
            LoadBanner();
        }
    }

    // ── BANNER ──

    void LoadBanner()
    {
        if (VIPSystem.IsAdsRemoved || BANNER_ID == "unused") return;

        bannerView?.Destroy();

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(BANNER_ID, adaptiveSize, AdPosition.Bottom);

        bannerView.OnBannerAdLoaded += () => StartCoroutine(ShowBannerDelayed());
        bannerView.OnBannerAdLoadFailed += err => Debug.LogError("[Ads] Banner failed: " + err.GetMessage());

        bannerView.LoadAd(new AdRequest());
    }

    System.Collections.IEnumerator ShowBannerDelayed()
    {
        yield return new WaitForEndOfFrame();
        bannerView?.Show();

#if UNITY_EDITOR
        var canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var c in canvases)
        {
            if (c.gameObject.name.Contains("ADAPTIVE") ||
                c.gameObject.name.Contains("Banner") ||
                c.gameObject.name.Contains("AdMob"))
            {
                c.sortingOrder = 999;
                Debug.Log("[Ads] Banner canvas sort order → 999");
            }
        }
#endif
    }
    // AdsManager.cs — thêm vào Awake
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene,
                   UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (VIPSystem.IsAdsRemoved) return;

        // Reload banner sau mỗi scene
        StartCoroutine(ReloadBannerDelayed());
    }

    System.Collections.IEnumerator ReloadBannerDelayed()
    {
        yield return new WaitForSeconds(0.3f);  // chờ scene load xong
        LoadBanner();
    }

#if UNITY_EDITOR
    System.Collections.IEnumerator FixBannerSortOrder()
    {
        yield return new WaitForEndOfFrame();
        var canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var c in canvases)
        {
            if (c.gameObject.name.Contains("ADAPTIVE") ||
                c.gameObject.name.Contains("Banner") ||
                c.gameObject.name.Contains("AdMob"))
            {
                c.sortingOrder = 999;
            }
        }
    }
#endif
    public void HideBanner() => bannerView?.Hide();

    // ── INTERSTITIAL ──

    void LoadInterstitial()
    {
        if (INTERSTITIAL_ID == "unused") return;

        InterstitialAd.Load(INTERSTITIAL_ID, new AdRequest(), (ad, err) =>
        {
            if (err != null) { Debug.LogWarning("[Ads] Interstitial load failed: " + err.GetMessage()); return; }

            interstitialAd = ad;

            ad.OnAdFullScreenContentClosed += () =>
            {
                onInterstitialClosed?.Invoke();
                onInterstitialClosed = null;
                LoadBanner();
                LoadInterstitial();
            };

            ad.OnAdFullScreenContentFailed += error =>
                Debug.LogError("[Ads] Interstitial show failed: " + error.GetMessage());

            Debug.Log("[Ads] Interstitial loaded");
        });
    }

    public void ShowInterstitial(Action onClosed = null)
    {
        if (VIPSystem.IsAdsRemoved) { onClosed?.Invoke(); return; }

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            onInterstitialClosed = onClosed;
            HideBanner();
            interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("[Ads] Interstitial not ready");
            onClosed?.Invoke();
            LoadInterstitial();
        }
    }

    // ── REWARDED ──

    void LoadRewarded()
    {
        if (REWARDED_ID == "unused") return;

        RewardedAd.Load(REWARDED_ID, new AdRequest(), (ad, err) =>
        {
            if (err != null) { Debug.LogWarning("[Ads] Rewarded load failed: " + err.GetMessage()); return; }

            rewardedAd = ad;

            ad.OnAdFullScreenContentClosed += () =>
            {
                LoadBanner();
                LoadRewarded();
            };

            ad.OnAdFullScreenContentFailed += error =>
                Debug.LogError("[Ads] Rewarded show failed: " + error.GetMessage());

            Debug.Log("[Ads] Rewarded loaded");
        });
    }

    public void ShowRewarded(Action<bool> onResult)
    {
        if (REWARDED_ID == "unused") { onResult?.Invoke(false); return; }

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            bool rewarded = false;
            HideBanner();

            rewardedAd.Show(reward =>
            {
                rewarded = true;
                Debug.Log($"[Ads] Rewarded earned: {reward.Amount} {reward.Type}");
            });

            rewardedAd.OnAdFullScreenContentClosed += () =>
                onResult?.Invoke(rewarded);
        }
        else
        {
            Debug.LogWarning("[Ads] Rewarded not ready");
            onResult?.Invoke(false);
            LoadRewarded();
        }
    }

    public bool IsRewardedReady() => rewardedAd != null && rewardedAd.CanShowAd();

    void OnDestroy()
    {
        bannerView?.Destroy();
        interstitialAd?.Destroy();
        rewardedAd?.Destroy();
    }
}