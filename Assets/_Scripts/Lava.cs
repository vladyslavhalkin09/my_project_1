using UnityEngine;

public class Lava : MonoBehaviour
{
    public float damagepersecond = 10f;
    private float _damageAccumulator;
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("TriggerStay: " + other.name);
        if (!other.CompareTag("Player")) return;

        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            _damageAccumulator += damagepersecond * Time.deltaTime;
            if (_damageAccumulator >= 5f)
            {
                p.TakeDamage(1);
                _damageAccumulator -= 5f;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        _damageAccumulator = 0f;
    }
}
