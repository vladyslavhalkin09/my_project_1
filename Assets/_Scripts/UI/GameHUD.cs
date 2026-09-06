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
    private VisualElement item_tooltip;
    private Label item_tooltip_name;
    private Label item_tooltip_stats;



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
        item_tooltip = _uidoc.rootVisualElement.Q<VisualElement>("item-tooltip");
        item_tooltip_name = _uidoc.rootVisualElement.Q<Label>("item-tooltip-name");
        item_tooltip_stats = _uidoc.rootVisualElement.Q<Label>("item-tooltip-stats");
        RegisterEquipSlotTooltip(equip_slot_weapon, EquipmentSlot.Weapon);
        RegisterEquipSlotTooltip(equip_slot_helmet, EquipmentSlot.Helmet);
        RegisterEquipSlotTooltip(equip_slot_amulet, EquipmentSlot.Amulet);
        RegisterEquipSlotTooltip(equip_slot_chest, EquipmentSlot.Chest);
        RegisterEquipSlotTooltip(equip_slot_boots, EquipmentSlot.Boots);
        RegisterEquipSlotTooltip(equip_slot_ring, EquipmentSlot.Ring);
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
            AddItemIcon(slot, item);
            RegisterTooltip(slot, item);
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
        else
        {
            HideItemTooltip();
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
        VisualElement existingIcon = slotElement.Q<VisualElement>(className: "item-icon");
        if (existingIcon != null) slotElement.Remove(existingIcon);

        ItemData item = _equipment.GetEquippedItem(slot);
        if (item != null)
        {
            slotElement.AddToClassList("equip-slot-filled");
            AddItemIcon(slotElement, item);
        }
        else
        {
            slotElement.RemoveFromClassList("equip-slot-filled");
        }
    }

    // Equip slots are persistent elements reused across updates (never recreated),
    // so their hover handlers are registered ONCE in Start() and always look up the
    // CURRENT item live via _equipment.GetEquippedItem(slot) — never capture the item
    // itself at registration time, or re-equipping would stack duplicate handlers.
    void RegisterEquipSlotTooltip(VisualElement slotElement, EquipmentSlot slot)
    {
        slotElement.RegisterCallback<PointerEnterEvent>(evt =>
        {
            ItemData item = _equipment.GetEquippedItem(slot);
            if (item != null) ShowItemTooltip(item, evt.position);
        });
        slotElement.RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (_equipment.GetEquippedItem(slot) != null) PositionTooltip(evt.position);
        });
        slotElement.RegisterCallback<PointerLeaveEvent>(evt => HideItemTooltip());
    }

    void AddItemIcon(VisualElement slot, ItemData item)
    {
        if (item.icon == null) return;
        VisualElement icon = new VisualElement();
        icon.AddToClassList("item-icon");
        icon.style.backgroundImage = new StyleBackground(item.icon);
        icon.pickingMode = PickingMode.Ignore;
        slot.Insert(0, icon);
    }

    // Inventory slots are fully recreated on every UpdateInventoryUI (via _inventoryGrid.Clear()),
    // so a fresh closure per slot here is safe — the old VisualElements and their handlers are discarded.
    void RegisterTooltip(VisualElement slot, ItemData item)
    {
        slot.RegisterCallback<PointerEnterEvent>(evt => ShowItemTooltip(item, evt.position));
        slot.RegisterCallback<PointerMoveEvent>(evt => PositionTooltip(evt.position));
        slot.RegisterCallback<PointerLeaveEvent>(evt => HideItemTooltip());
    }

    void ShowItemTooltip(ItemData item, Vector2 pointerPosition)
    {
        item_tooltip_name.text = item.ItemName;
        item_tooltip_stats.text = BuildStatsText(item);
        item_tooltip.style.display = DisplayStyle.Flex;
        PositionTooltip(pointerPosition);
    }

    void PositionTooltip(Vector2 pointerPosition)
    {
        Vector2 local = _uidoc.rootVisualElement.WorldToLocal(pointerPosition);
        item_tooltip.style.left = local.x + 16;
        item_tooltip.style.top = local.y + 16;
    }

    void HideItemTooltip()
    {
        item_tooltip.style.display = DisplayStyle.None;
    }

    string BuildStatsText(ItemData item)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (item is WeaponData weapon)
        {
            sb.AppendLine($"Damage: {weapon.minDamage:0} - {weapon.maxDamage:0}");
        }
        if (item.statModifiers != null)
        {
            foreach (StatModifier mod in item.statModifiers)
            {
                string sign = mod.Value >= 0 ? "+" : "";
                sb.AppendLine($"{mod.AffectedStat}: {sign}{mod.Value:0.#}");
            }
        }
        return sb.ToString().TrimEnd();
    }
}
