using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gear", menuName = "Inventory/Gear")]
public class GearData : ItemData
{
    [SerializeField]
    public EnumGearSlot enumGearSlot;
    [SerializeField]
    public List<StatusGear> statusGears;

}

[Serializable]
public class StatusGear
{
    public EnumGearStatus enumStatus;
    public int value;
}

[Serializable]
public enum EnumGearStatus
{
    None,
    MaxHealth,
    DEF,
    ATK
}

[Serializable]
public enum EnumGearSlot
{
    Gear
}
