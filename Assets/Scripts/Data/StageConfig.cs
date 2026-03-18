using UnityEngine;

[System.Serializable]
public class StageConfig
{
    [Header("Gameplay")]
    public int knivesToHit;

    [Header("Target")]
    public GameObject targetPrefab;
    //public Transform targetSpawnPoint;

    [Header("Rotation")]
    public float startRotationSpeed;

    [Header("Dynamic Rotation")]
    public bool randomRotation;
    public float rotationChangeInterval;
    public float minRotationSpeed;
    public float maxRotationSpeed;
    public bool allowReverse;

    [Header("Obstacles")]
    public int startingKnives;
    public GameObject knifePrefab;

    [Header("Bonus")]
    public int appleCount;
    public GameObject applePrefab;

    [Header("Boss")]
    public bool isBoss;
    public string bossName;
    public GameObject bossIconPrefab;

    [Header("Boss Reward")]
    public KnifeData rewardKnife;
    //[Header("Pattern")]
    //public RotationPattern[] rotationPattern;
}