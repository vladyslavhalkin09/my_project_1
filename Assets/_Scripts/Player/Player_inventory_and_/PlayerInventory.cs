using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour
{
    List<ItemData> items = new List<ItemData>();
    public int SlotsAmount;
    private Player _player;
    private Health _playerHealth;
    public bool isInventoryopen;
    public TextMeshProUGUI inventoryText;
    public int totalItemsPickedUp;
    public event Action OnInventoryChanged;
    public event Action<bool> OnInventoryToggled;
    void Start()
    {
        _player = GetComponent<Player>();
        _playerHealth = GetComponent<Health>();
        UpdateInventoryUI();
        inventoryText.gameObject.SetActive(false);

    }
    public bool AddItem(ItemData item)
    {
        if (items.Count < SlotsAmount)
        {
            items.Add(item);
            totalItemsPickedUp++;
            UpdateInventoryUI();
            OnInventoryChanged?.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Inventory is full");
            return false;

        }
    }
    public void UseItem()
    {
        if (items.Count > 0)
        {
            ItemData item = items[0];
            if (item.isUsable)
            {
                // if (item is HealthItemData healthItem)
                // {
                //     if (_playerHealth.hp < _playerHealth.maxHp)
                //     {
                //         _player.HealAmount(healthItem.healthRestored);
                //         items.Remove(item);
                //     }
                //     else
                //     {
                //         Debug.Log("HP is full");
                //     }
                // }
            }
        }
        OnInventoryChanged?.Invoke();
        UpdateInventoryUI();
    }
    public void UseHealItem()
    {
        if (items.Count > 0)
        {
            foreach (ItemData item in items)
            {
                if (item is HealthItemData healthItem)
                {
                    if (_playerHealth.currentValue < _playerHealth.maxValue)
                    {
                        _player.HealAmount(healthItem.healthRestored);
                        items.Remove(item);
                        OnInventoryChanged?.Invoke();
                        UpdateInventoryUI();
                        return;
                    }
                    else
                    {
                        Debug.Log("HP is full");
                    }
                }
            }
        }
    }
    public void UseSpeedItem()
    {
        if (items.Count > 0)
        {
            foreach (ItemData item in items)
            {
                if (item is SpeedFlaskData speedFlask)
                {
                    _player.StartSpeedBoost(speedFlask.speedmultiplier, speedFlask.duration);
                    items.Remove(item);
                    OnInventoryChanged?.Invoke();
                    UpdateInventoryUI();
                    return;
                }
            }
        }
    }
    public void DeleteFromInventory()
    {
        if (items.Count > 0)
        {
            ItemData item = items[0];
            items.Remove(item);
        }
        OnInventoryChanged?.Invoke();
        UpdateInventoryUI();
    }
    public void UpdateInventoryUI()
    {
        if (inventoryText != null)
        {
            inventoryText.text = "Inventory \n";
            inventoryText.text += "Items count " + totalItemsPickedUp.ToString() + "\n";
            inventoryText.text += "Slots available " + items.Count + "/" + SlotsAmount + "\n";
            foreach (ItemData item in items)
            {
                inventoryText.text += item.ItemName + "\n";
            }
        }
    }
    public void ToggleInventory()
    {
        isInventoryopen = !isInventoryopen;
        OnInventoryToggled?.Invoke(isInventoryopen);
    }
    public List<ItemData> GetItems() => items;
    public bool HasKey()
    {
        foreach (ItemData item in items)
        {
            if (item is KeyItemData) return true;
        }
        return false;
    }
    public bool RemoveKey()
    {
        foreach (ItemData item in items)
        {
            if (item is KeyItemData)
            {
                items.Remove(item);
                OnInventoryChanged?.Invoke();
                UpdateInventoryUI();
                return true;
            }
        }
        return false;
    }
}
