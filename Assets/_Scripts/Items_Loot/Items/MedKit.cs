using UnityEngine;


public class MedKit : MonoBehaviour
{
    public HealthItemData data;
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory p = other.GetComponent<PlayerInventory>(); // Receive player inventory script
        if (p != null) // if player inventory exist
        {
            if (p.AddItem(data))
            {
                Destroy(gameObject);
            }

        }

    }
}
