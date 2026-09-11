using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool ischestopen;
    public List<GameObject> drops;
    public float spawnitemradius = 2f;


    private void OnTriggerEnter(Collider other)
    {
        if (ischestopen) return;
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
    public string GetInteractionPrompt()
    {
        return "Open Chest";
    }

    public void OpenChest()
    {
        if (ischestopen) return;
        foreach (GameObject drop in drops)
        {
            Vector3 randomOffsetItem = new Vector3(Random.Range(-spawnitemradius, spawnitemradius), 0, Random.Range(-spawnitemradius, spawnitemradius));
            Vector3 finalSpawnPosItem = transform.position + randomOffsetItem;
            Instantiate(drop, finalSpawnPosItem, Quaternion.identity);
        }
        ischestopen = true;
    }
    public void Interact(Player player)
    {
        OpenChest();
        if (player._nearbyInteractable == (IInteractable)this)
        {
            player._nearbyInteractable = null;
        }
    }
}