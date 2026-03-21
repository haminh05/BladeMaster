using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public KnifeDatabase knifeDatabase;

    public static event Action OnEquipChanged; 

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(this);
        UnlockDefaultKnife();
    }

    void UnlockDefaultKnife()
    {
        foreach (var knife in knifeDatabase.knives)
        {
            if (knife.isDefault)
            {
                if (!IsKnifeUnlocked(knife.id))
                {
                    PlayerPrefs.SetInt("KNIFE_" + knife.id, 1);
                    PlayerPrefs.Save();
                }
                if (GetEquippedKnife() == 0)
                {
                    PlayerPrefs.SetInt("EQUIP_KNIFE", knife.id);
                    PlayerPrefs.Save();
                }
                break;
            }
        }
    }

    public bool IsKnifeUnlocked(int id)
    {
        return PlayerPrefs.GetInt("KNIFE_" + id, 0) == 1;
    }

    public void UnlockKnife(int id)
    {
        PlayerPrefs.SetInt("KNIFE_" + id, 1);
        PlayerPrefs.Save();
    }

    public void EquipKnife(int id)
    {
        if (!IsKnifeUnlocked(id)) return;
        PlayerPrefs.SetInt("EQUIP_KNIFE", id);
        PlayerPrefs.Save();
        SoundManager.Instance.PlayEquipKnife();
        OnEquipChanged?.Invoke(); // thông báo cho UI
    }

    public int GetEquippedKnife()
    {
        return PlayerPrefs.GetInt("EQUIP_KNIFE", 0);
    }

    public KnifeData GetEquippedKnifeData()
    {
        int id = GetEquippedKnife();
        return knifeDatabase.GetKnife(id);
    }
    
    public void NotifyEquipChanged()
    {
        OnEquipChanged?.Invoke();
    }
}