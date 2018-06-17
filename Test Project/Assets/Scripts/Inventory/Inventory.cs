using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {

    [SerializeField] List<item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlots[] itemSlots;

    public event Action<item> OnItemRightClickedEvent;

    private void Awake()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
        }
    }

    private void OnValidate()
    {
        if ( itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlots>();

        RefreshUI();
    }

    // 인벤토리 표시 해주는 메소드
    private void RefreshUI()
    {
        int i = 0;
        for(; i< items.Count && i < itemSlots.Length; i++)
        {
            // 인벤토리에 item 추가
            itemSlots[i].Item = items[i];
        }

        for(; i< itemSlots.Length; i++)
        {
            // 인벤토리 빈슬롯
            itemSlots[i].Item = null;
        }
    }

    // 아이템을 인벤토리에 추가하는 메소드
    public bool AddItem(item item)
    {
        // 인벤토리가 가득 찼는지 확인
        // 가득 차 있다면 추가하지않는다.
        if (IsFull()) 
            return false;

        // 가득 차 있지 않다면 아이템을 추가한다.
        items.Add(item);
        // 인벤토리에 표시해준다.
        RefreshUI();    
        return true;
    }

    // 아이템을 인벤토리에서 삭제하는 메소드
    public bool RemoveItem(item item)
    {
        if (items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
    }

    // 인벤토리 가득찼는지 확인하는 메소드
    public bool IsFull()
    {
        // item의 갯수가 인벤토리 Slot보다 많다면 false
        // Slot이 item 보다 많다면 true 
        return items.Count >= itemSlots.Length;
    }
}
