using UnityEngine;

public class Key : MonoBehaviour
{

    public KeyItemData data;
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory p = other.GetComponent<PlayerInventory>();
        if (p != null)
        {
            if (p.AddItem(data))
            {
                Destroy(gameObject);
            }
        }
    }
}
