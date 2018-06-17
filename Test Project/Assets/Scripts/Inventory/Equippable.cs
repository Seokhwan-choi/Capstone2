using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Chest,
    Weapon,
    Accessory,
}

[CreateAssetMenu]
public class Equippable : item {
    public int StrengthBonus;
    public int AgilityBonus;
    [Space]
    public float StrengthPercentBouns;
    public float AgilityPercentBonus;
    [Space]
    public EquipmentType EquipmentType;
}
