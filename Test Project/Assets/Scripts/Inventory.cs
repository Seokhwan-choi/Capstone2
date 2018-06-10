using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> Allslot;

    private string name;
    GameObject slot;

    public GameObject a, b, c;

    private Items item;
    private float EmptySlot;
    
    void Start()
    {
        slot = transform.Find("Imange").gameObject;
        Allslot.Add(slot);
        for (int i = 1; i < 8; i++)
        {
            name = "Image(" + i.ToString() + ")";
            slot = transform.Find(name).gameObject;
            Allslot.Add(slot);
        }
        EmptySlot = Allslot.Count;

        item = a.GetComponent<Items>();
        AddItem(item);
        item = b.GetComponent<Items>();
        AddItem(item);
        item = c.GetComponent<Items>();
        AddItem(item);

    }

    // 아이템을 넣기위해 모든 슬롯을 검사.
    public bool AddItem(Items item)
    {
        // 슬롯에 총 개수.
        int slotCount = Allslot.Count;

        // 넣기위한 아이템이 슬롯에 존재하는지 검사.
        for (int i = 0; i < slotCount; i++)
        {
            // 그 슬롯의 스크립트를 가져온다.
            Slot slot = Allslot[i].GetComponent<Slot>();

            // 슬롯이 비어있으면 통과.
            if (!slot.isSlots())
                continue;

            // 슬롯에 존재하는 아이템의 타입과 넣을려는 아이템의 타입이 같고.
            // 슬롯에 존재하는 아이템의 겹칠수 있는 최대치가 넘지않았을 때. (true일 때)
            if (slot.ItemReturn().type == item.type && slot.ItemMax(item))
            {
                // 슬롯에 아이템을 넣는다.
                slot.AddItem(item);
                return true;
            }
        }

        // 빈 슬롯에 아이템을 넣기위한 검사.
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Allslot[i].GetComponent<Slot>();

            // 슬롯이 비어있지 않으면 통과
            if (slot.isSlots())
                continue;

            slot.AddItem(item);
            return true;
        }

        // 위에 조건에 해당되는 것이 없을 때 아이템을 먹지 못함.
        return false;
    }
}
