using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool isDoorOpen;
    public bool isRequiredLever;
    public bool isRequiredKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if (isRequiredLever) return;
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
            // if (isRequiredLever) return;
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player._nearbyInteractable = null;
            }
        }
    }
    public string GetInteractionPrompt()
    {
        return "Open Door";
    }
    public void OpenDoor()
    {
        if (isDoorOpen) return;
        isDoorOpen = true;
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            if (col.isTrigger == false)
            {
                col.enabled = false;
            }
        }
        gameObject.SetActive(false);

    }
    public void Interact(Player player)
    {
        if (isRequiredLever) return;
        if (isRequiredKey)
        {
            if (player._inventory.HasKey())
            {
                OpenDoor();
                player._inventory.RemoveKey();
            }
        }
        else
        {
            OpenDoor();
        }
    }
}

