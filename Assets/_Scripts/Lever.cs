using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public bool isLeverOn;
    public Door targetDoor;
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
            if (player != null)
            {
                player._nearbyInteractable = null;
            }
        }
    }
    public void UseLever()
    {
        if (isLeverOn) return;
        isLeverOn = true;
        targetDoor.OpenDoor();

    }
    public void Interact(Player player)
    {
        UseLever();
    }
}
