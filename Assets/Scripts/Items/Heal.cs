using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Item
{
    public override void UseItem()
    {
        if (!_owner.CanUseItem()) return;
        //_owner.SetItem(_itemName);
        _owner.Heal(StatsReader.Instance.GetStat(_itemName).HP);
        gameObject.SetActive(false);
    }
}
