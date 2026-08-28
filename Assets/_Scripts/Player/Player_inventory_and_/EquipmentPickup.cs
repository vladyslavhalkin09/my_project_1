using UnityEngine;

public class EquipmentPickup : MonoBehaviour, IInteractable
{
    public ItemData data;

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