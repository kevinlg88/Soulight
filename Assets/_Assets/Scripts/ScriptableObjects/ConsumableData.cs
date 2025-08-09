using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class ConsumableData : ItemData
{
    [SerializeField]
    public List<StatusConsumable> status;
}

[Serializable]
public class StatusConsumable
{
    public EnumConsumable enumStatus;
    public int value;
}

[Serializable]
public enum EnumConsumable
{
    None,
    IncreaseHealth,
    IncreaseDefense,
    IncreaseAttack,
}

