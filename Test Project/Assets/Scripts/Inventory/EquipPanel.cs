using UnityEngine;
using System;

public class EquipPanel : MonoBehaviour {

    [SerializeField] Transform equipSlotsParent;
    [SerializeField] EquipSlot[] equipSlots;

    public event Action<item> OnItemRightClickedEvent;

    private void Awake()
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
        }
    }

    private void OnValidate()
    {
        equipSlots = equipSlotsParent.GetComponentsInChildren<EquipSlot>();
    }

    public bool AddItem(Equippable item, out Equippable previousItem)
    {
        for(int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (Equippable)equipSlots[i].Item;
                equipSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(Equippable item)
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].Item == item)
            {
                equipSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}
