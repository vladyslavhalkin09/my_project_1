using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public float damage;
    public float baseCooldown;
    public GameObject bulletPrefab;
    public Transform attackPoint;
    public IAttackModifier[] _modifiers;
    private float _cooldownTimer;
    public LayerMask groundLayer;
    public void Awake()
    {
        _modifiers = GetComponents<IAttackModifier>();
    }
    public void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        _cooldownTimer = Mathf.Max(0f, _cooldownTimer);
    }
    public void TryShot(Vector3 targetPoint)
    {
        if (_cooldownTimer > 0) return;
        _cooldownTimer = baseCooldown;
        Shoot(targetPoint);

    }
    void Shoot(Vector3 targetPoint)
    {
        float distanceToPoint = Vector3.Distance(attackPoint.position, targetPoint);
        if (Physics.Linecast(attackPoint.position, targetPoint, out RaycastHit wallHit, groundLayer))
        {
            targetPoint = wallHit.point;
            distanceToPoint = wallHit.distance;
        }
        GameObject newBullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.bullethitpoint = targetPoint;
            bulletScript.maxDistance = distanceToPoint;
            bulletScript.bulletdamage = damage;
            foreach (var modifier in _modifiers)
            {
                modifier.Apply(bulletScript);
            }
        }
    }
    void RefreshModifiers()
    {
        _modifiers = GetComponents<IAttackModifier>();
    }
}
