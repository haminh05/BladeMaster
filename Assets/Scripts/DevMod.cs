
using UnityEngine;

public class DevMod : MonoBehaviour
{
    public void UnlockAll()
    {
        foreach (var knife in InventoryManager.Instance.knifeDatabase.knives)
        {
            InventoryManager.Instance.UnlockKnife(knife.id);
        }
        Debug.Log("Unlocked all knives!");
    }

    public void LockAll()
    {
        foreach (var knife in InventoryManager.Instance.knifeDatabase.knives)
        {
            if (!knife.isDefault)
                PlayerPrefs.SetInt("KNIFE_" + knife.id, 0);
        }
        PlayerPrefs.Save();
        Debug.Log("Locked all knives (except default)!");
    }
}
