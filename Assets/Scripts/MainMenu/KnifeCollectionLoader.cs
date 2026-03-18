using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class KnifeCollectionLoader : MonoBehaviour
{
    [Header("Database")]
    public KnifeDatabase database;

    [Header("Prefabs")]
    public GameObject slotPrefab;
    public GameObject comingSoonPrefab; // Prefab ô "Coming Soon"

    [Header("Pages Content ")]
    public Transform contentApple;
    public Transform contentAds;
    public Transform contentBoss;
    public Transform contentBossChallenge;

   

    [Header("Slots mỗi trang")]
    public int slotsPerPage = 16;

    void Start()
    {
       

        // Nhóm dao theo UnlockType
        var groups = new Dictionary<KnifeUnlockType, Transform>
        {
            { KnifeUnlockType.Apple,         contentApple },
            { KnifeUnlockType.Ads,           contentAds },
            { KnifeUnlockType.Boss,          contentBoss },
            { KnifeUnlockType.BossChallenge, contentBossChallenge },
        };
        
        foreach (var kvp in groups)
        {
            
            var knivesInGroup = database.knives
                .Where(k => k.unlockType == kvp.Key)
                .ToList();

            // Spawn slot thật
            foreach (var knife in knivesInGroup)
            {
                GameObject slot = Instantiate(slotPrefab, kvp.Value);
                slot.GetComponent<KnifeSlotUI>().Setup(knife);
            }

            // Lấp đầy Coming Soon
            int remaining = slotsPerPage - knivesInGroup.Count;
            for (int i = 0; i < remaining; i++)
            {
                Instantiate(comingSoonPrefab, kvp.Value);
            }
        }
        RefreshAllSlots();
    }
    void RefreshAllSlots()
    {
        var allSlots = GetComponentsInChildren<KnifeSlotUI>();
        foreach (var slot in allSlots)
            slot.Refresh();
    }
}