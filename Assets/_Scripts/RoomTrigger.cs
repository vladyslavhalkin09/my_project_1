using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomEncounter encounter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            encounter.StartEncounter();
        }
    }
}
