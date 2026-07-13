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
        if (isOpen) UpdateInventoryUI();
    }

}
