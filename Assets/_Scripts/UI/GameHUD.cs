using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Player player;
    private UIDocument _uidoc;
    private VisualElement hp_bar_fill;
    private VisualElement cast_bar_container;
    private VisualElement cast_bar_fill;
    private Label cast_bar_label;
    private Label hp_text;
    private VisualElement ability_1_overlay;
    private VisualElement ability_2_overlay;
    private VisualElement ability_3_overlay;
    private VisualElement ability_4_overlay;
    private Health _health;
    private Mana _mana;
    private VisualElement mana_bar_fill;
    private Label mana_text;
    private PlayerInventory _inventory;
    private VisualElement _inventoryContainer;
    private VisualElement _inventoryGrid;
    private PlayerEquipment _equipment;
    private VisualElement equip_slot_weapon;
    private VisualElement equip_slot_helmet;
    private VisualElement equip_slot_amulet;
    private VisualElement equip_slot_chest;
    private VisualElement equip_slot_boots;
    private VisualElement equip_slot_ring;
    private VisualElement interaction_prompt;
    private Label interaction_prompt_label;
    private IInteractable _lastInteractable;



    void Start()
    {
        _uidoc = GetComponent<UIDocument>();
        _health = player.GetComponent<Health>();
        _mana = player.GetComponent<Mana>();
        _inventory = player.GetComponent<PlayerInventory>();
        _inventoryContainer = _uidoc.rootVisualElement.Q<VisualElement>("inventory-container");
        _inventoryGrid = _uidoc.rootVisualElement.Q<VisualElement>("inventory-grid");
        _inventory.OnInventoryChanged += UpdateInventoryUI;
        _inventory.OnInventoryToggled += HandleInventoryToggle;
        _equipment = player.GetComponent<PlayerEquipment>();
        equip_slot_weapon = _uidoc.rootVisualElement.Q<VisualElement>("equip-slot-weapon");
        equip_slot_helmet = _uidoc.rootVisualElement.Q<VisualElement>("equip-slot-helmet");
        equip_slot_amulet = _uidoc.rootVisualElement.Q<VisualElement>("equip-slot-amulet");
        equip_slot_chest = _uidoc.rootVisualElement.Q<VisualElement>("equip-slot-chest");
        equip_slot_boots = _uidoc.rootVisualElement.Q<VisualElement>("equip-slot-boots");
        equip_slot_ring = _uidoc.rootVisualElement.Q<VisualElement>("equip-slot-ring");
        _equipment.OnEquipmentChanged += HandleEquipmentChanged;
        _health.OnResourceChanged += UpdateHPBar;
        _mana.OnResourceChanged += UpdateManaBar;
        hp_bar_fill = _uidoc.rootVisualElement.Q<VisualElement>("hp-bar-fill");
        cast_bar_container = _uidoc.rootVisualElement.Q<VisualElement>("cast-bar-container");
        cast_bar_fill = _uidoc.rootVisualElement.Q<VisualElement>("cast-bar-fill");
        cast_bar_label = _uidoc.rootVisualElement.Q<Label>("cast-bar-label");
        hp_text = _uidoc.rootVisualElement.Q<Label>("hp-text");
        mana_bar_fill = _uidoc.rootVisualElement.Q<VisualElement>("mana-bar-fill");
        mana_text = _uidoc.rootVisualElement.Q<Label>("mana-text");
        ability_1_overlay = _uidoc.rootVisualElement.Q<VisualElement>("ability-1-overlay");
        ability_2_overlay = _uidoc.rootVisualElement.Q<VisualElement>("ability-2-overlay");
        ability_3_overlay = _uidoc.rootVisualElement.Q<VisualElement>("ability-3-overlay");
        ability_4_overlay = _uidoc.rootVisualElement.Q<VisualElement>("ability-4-overlay");
        player._multishot.OnCooldownChanged += UpdateAbility1;
        player._multishot.OnCastChanged += UpdateCastBar;
        player._multishot.OnCastFinished += HideCastbar;
        player._trapability.OnCooldownChanged += UpdateAbility2;
        player._powershot.OnCooldownChanged += UpdateAbility3;
        player._powershot.OnCastChanged += UpdateCastBar;
        player._powershot.OnCastFinished += HideCastbar;
        player._dash.OnCooldownChanged += UpdateAbility4;
        ability_1_overlay.style.display = DisplayStyle.None;
        ability_2_overlay.style.display = DisplayStyle.None;
        ability_3_overlay.style.display = DisplayStyle.None;
        ability_4_overlay.style.display = DisplayStyle.None;
        Debug.Log($"Overlay 1 display: {ability_1_overlay.style.display.value}");
        interaction_prompt = _uidoc.rootVisualElement.Q<VisualElement>("interaction-prompt");
        interaction_prompt_label = _uidoc.rootVisualElement.Q<Label>("interaction-prompt-label");

    }
    void Update()
    {
        if (player._nearbyInteractable != _lastInteractable)
        {
            _lastInteractable = player._nearbyInteractable;
            if (_lastInteractable != null)
            {
                interaction_prompt.style.display = DisplayStyle.Flex;
                interaction_prompt_label.text = $"[F] {_lastInteractable.GetInteractionPrompt()}";
            }
            else
            {
                interaction_prompt.style.display = DisplayStyle.None;
            }
        }
    }
    void UpdateAbility1(float progress)
    {
        if (progress >= 1f)
        {
            ability_1_overlay.style.display = DisplayStyle.None;
        }
        else
        {
            ability_1_overlay.style.display = DisplayStyle.Flex;
            ability_1_overlay.style.height = Length.Percent(100f - progress * 100f);
        }
    }
    void UpdateAbility2(float progress)
    {
        if (progress >= 1f)
        {
            ability_2_overlay.style.display = DisplayStyle.None;
        }
        else
        {
            ability_2_overlay.style.display = DisplayStyle.Flex;
            ability_2_overlay.style.height = Length.Percent(100f - progress * 100f);
        }
    }
    void UpdateAbility3(float progress)
    {
        if (progress >= 1f)
        {
            ability_3_overlay.style.display = DisplayStyle.None;
        }
        else
        {
            ability_3_overlay.style.display = DisplayStyle.Flex;
            ability_3_overlay.style.height = Length.Percent(100f - progress * 100f);
        }
    }
    void UpdateAbility4(float progress)
    {
        if (progress >= 1f)
        {
            ability_4_overlay.style.display = DisplayStyle.None;
        }
        else
        {
            ability_4_overlay.style.display = DisplayStyle.Flex;
            ability_4_overlay.style.height = Length.Percent(100f - progress * 100f);
        }
    }
    void UpdateHPBar(float progress)
    {
        hp_bar_fill.style.width = Length.Percent(progress * 100f);
        hp_text.text = $"{Mathf.Round(_health.currentValue)} / {_health.maxValue}";
    }
    void UpdateManaBar(float progress)
    {
        mana_bar_fill.style.width = Length.Percent(progress * 100f);
        mana_text.text = $"{Mathf.Round(_mana.currentValue)} / {_mana.maxValue}";
    }
    void UpdateCastBar(float progress, string abilityName)
    {
        cast_bar_container.style.display = DisplayStyle.Flex;
        cast_bar_fill.style.width = Length.Percent(progress * 100f);
        cast_bar_label.text = abilityName;
    }
    void HideCastbar()
    {
        cast_bar_container.style.display = DisplayStyle.None;
    }
    void OnDestroy()
    {
        player._multishot.OnCooldownChanged -= UpdateAbility1;
        player._trapability.OnCooldownChanged -= UpdateAbility2;
        player._powershot.OnCooldownChanged -= UpdateAbility3;
        player._dash.OnCooldownChanged -= UpdateAbility4;
        _health.OnResourceChanged -= UpdateHPBar;
        _mana.OnResourceChanged -= UpdateManaBar;
        player._multishot.OnCastChanged -= UpdateCastBar;
        player._multishot.OnCastFinished -= HideCastbar;
        player._powershot.OnCastChanged -= UpdateCastBar;
        player._powershot.OnCastFinished -= HideCastbar;
        _inventory.OnInventoryChanged -= UpdateInventoryUI;
        _inventory.OnInventoryToggled -= HandleInventoryToggle;
        _equipment.OnEquipmentChanged -= HandleEquipmentChanged;
    }
    void UpdateInventoryUI()
    {
        _inventoryGrid.Clear();
        foreach (ItemData item in _inventory.GetItems())
        {
            VisualElement slot = new VisualElement();
            slot.AddToClassList("inventory-slot");
            slot.AddToClassList("inventory-slot-filled");
            slot.tooltip = item.ItemName;
            if (item.isEquppable)
            {
                slot.RegisterCallback<ClickEvent>(evt =>
                {
                    ItemData previousItem = _equipment.Equip(item);
                    _inventory.RemoveItem(item);
                    if (previousItem != null)
                    {
                        _inventory.AddItem(previousItem);
                    }
                });
            }
            _inventoryGrid.Add(slot);
        }
        int emptySlots = _inventory.SlotsAmount - _inventory.GetItems().Count;
        for (int i = 0; i < emptySlots; i++)
        {
            VisualElement slot = new VisualElement();
            slot.AddToClassList("inventory-slot");
            _inventoryGrid.Add(slot);
        }
    }

    void HandleInventoryToggle(bool isOpen)
    {
        _inventoryContainer.style.display = isOpen ? DisplayStyle.Flex : DisplayStyle.None;
        if (isOpen)
        {
            UpdateInventoryUI();
            UpdateEquipSlotsUI();
        }
    }
    void HandleEquipmentChanged(EquipmentSlot slot)
    {
        UpdateEquipSlotsUI();
    }

    void UpdateEquipSlotsUI()
    {
        UpdateEquipSlot(equip_slot_weapon, EquipmentSlot.Weapon);
        UpdateEquipSlot(equip_slot_helmet, EquipmentSlot.Helmet);
        UpdateEquipSlot(equip_slot_amulet, EquipmentSlot.Amulet);
        UpdateEquipSlot(equip_slot_chest, EquipmentSlot.Chest);
        UpdateEquipSlot(equip_slot_boots, EquipmentSlot.Boots);
        UpdateEquipSlot(equip_slot_ring, EquipmentSlot.Ring);
    }

    void UpdateEquipSlot(VisualElement slotElement, EquipmentSlot slot)
    {
        ItemData item = _equipment.GetEquippedItem(slot);
        if (item != null)
        {
            slotElement.AddToClassList("equip-slot-filled");
            slotElement.tooltip = item.ItemName;
        }
        else
        {
            slotElement.RemoveFromClassList("equip-slot-filled");
            slotElement.tooltip = "";
        }
    }
}
