using System.Collections;
using UnityEngine;

public class DashAbility : BaseAbility
{
    public float dashDistance;
    public float dashSpeed;
    public float dashDamage;
    public bool isDashActive;
    public CharacterController cc;
    private Health _health;
    void Start()
    {
        _health = GetComponent<Health>();
        cc = GetComponent<CharacterController>();
    }
    public void TryUseDash(Vector3 targetpoint)
    {
        if (isoncooldown) return;
        if (!_mana.TryConsume(manaCost)) return;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = new Vector3(h, 0f, v);
        Vector3 direction = Vector3.zero;
        if (moveDir.magnitude > 0)
        {
            direction = moveDir.normalized;
        }
        else
        {
            moveDir = targetpoint - gameObject.transform.position;
            direction = moveDir.normalized;
        }
        StartCoroutine(DashRoutine(direction));


    }
    IEnumerator DashRoutine(Vector3 direction)
    {
        isDashActive = true;
        Physics.IgnoreLayerCollision(10, 3, true);
        _health.enabled = false;
        float distanceTravelled = 0f;
        while (distanceTravelled < dashDistance)
        {
            float step = dashSpeed * Time.deltaTime;
            cc.Move(direction * step);
            distanceTravelled = distanceTravelled + step;
            yield return null;
        }
        Physics.IgnoreLayerCollision(10, 3, false);
        _health.enabled = true;
        isDashActive = false;
        StartCoroutine(CooldownBase());
    }

}
