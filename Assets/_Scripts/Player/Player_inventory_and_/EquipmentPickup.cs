using UnityEngine;

public class EquipmentPickup : MonoBehaviour, IInteractable
{
    public ItemData data;
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private LootBeam lootBeam; // перетягни дочірній Beam сюди в інспекторі

    private void Start()
    {
        RefreshVisuals();
    }

    public void SetData(ItemData newData)
    {
        data = newData;
        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        if (data == null) return;

        if (iconRenderer != null)
        {
            iconRenderer.sprite = data.icon;
        }
        if (lootBeam != null)
        {
            lootBeam.ApplyRarity(data.rarity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player._nearbyInteractable = this;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player._nearbyInteractable == (IInteractable)this)
            {
                player._nearbyInteractable = null;
            }
        }
    }
    public string GetInteractionPrompt()
    {
        return $"Pick up {data.ItemName}";
    }

    public void Interact(Player player)
    {
        if (player._inventory.AddItem(data))
        {
            player._nearbyInteractable = null;
            Destroy(gameObject);
        }
    }
}