using UnityEngine;

public class IVManager : MonoBehaviour {

    [SerializeField] Inventory inventory;
    [SerializeField] EquipPanel equippanel;

    private void Awake()
    {
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equippanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
    }

    private void EquipFromInventory(item item)
    {
        if( item is Equippable)
        {
            Equip((Equippable)item);
        }
    }

    private void UnequipFromEquipPanel(item item)
    {
        if( item is Equippable)
        {
            Unequip((Equippable)item);
        }
    }

    public void Equip(Equippable item)
    {
        if (inventory.RemoveItem(item))
        {
            Equippable previousItem;
            if(equippanel.AddItem(item, out previousItem))
            {
                if(previousItem != null)
                {
                    inventory.AddItem(previousItem);
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(Equippable item)
    {
        if(!inventory.IsFull() && equippanel.RemoveItem(item))
        {
            inventory.AddItem(item);
        }

    }
}
