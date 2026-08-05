using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public float baseCooldown;
    public GameObject bulletPrefab;
    public Transform attackPoint;
    public IAttackModifier[] _modifiers;
    private float _cooldownTimer;
    public LayerMask groundLayer;
    private CharacterStatsHolder _stats;
    private PlayerEquipment _equipment;
    private WeaponDamageCalculator _calculator;
    public void Awake()
    {
        _modifiers = GetComponents<IAttackModifier>();
        _stats = GetComponent<CharacterStatsHolder>();
        _equipment = GetComponent<PlayerEquipment>();
        _calculator = GetComponent<WeaponDamageCalculator>();
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
            bulletScript.bulletdamage = _calculator.GetAutoAttackDamage();
            foreach (var modifier in _modifiers)
            {
                modifier.Apply(bulletScript);
            }
        }
    }
    private void OnEnable()
    {
        if (_equipment != null)
        {
            _equipment.OnEquipmentChanged += HandleEquipmentChanged;
        }
    }
    private void OnDestroy()
    {
        if (_equipment != null)
        {
            _equipment.OnEquipmentChanged -= HandleEquipmentChanged;
        }
    }
    private void HandleEquipmentChanged(EquipmentSlot slot)
    {
        if (slot == EquipmentSlot.Weapon)
        {
            RefreshModifiers();
        }
    }
    void RefreshModifiers()
    {
        _modifiers = GetComponents<IAttackModifier>();
    }
}
