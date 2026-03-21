using UnityEngine;

public enum PowerUpType
{
    ReviveChance,        // % cơ hội chơi tiếp khi chết
    DailyAppleBonus,     // % cộng thêm apple từ daily gift
    VideoAppleBonus,     // +apple cố định khi xem video
    XPBoost,             // % XP nhận được
    // thêm type mới ở đây sau
}

[CreateAssetMenu(menuName = "Game/PowerUpData")]
public class PowerUpData : ScriptableObject
{
    [Header("Info")]
    public PowerUpType type;
    public string description;    // "{value}% cơ hội sống sót"
    public Sprite iconLocked;
    public Sprite iconUnlocked;

    [Header("Buff Value")]
    public float value;

    public string GetLabel() => $"{value} {description}";
}