using UnityEngine;

[CreateAssetMenu(fileName = "KnifeDatabase", menuName = "Game/KnifeDatabase")]
public class KnifeDatabase : ScriptableObject
{
    public KnifeData[] knives;

    public KnifeData GetKnife(int id)
    {
        foreach (var knife in knives)
        {
            if (knife.id == id)
                return knife;
        }
        return null;
    }
}