using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected PlayerStateMachine _owner;
    [SerializeField] protected string _itemName;
    public virtual void UseItem()
    {
        if (!_owner.CanUseItem()) return;
        _owner.SetItem(_itemName);
        gameObject.SetActive(false);
    }
}
