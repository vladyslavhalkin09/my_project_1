using System;
using UnityEngine;
using System.Collections;
public class Player : MonoBehaviour
{
    public string playerName = "test";
    public LayerMask groundLayer;
    [Tooltip("Assign your Enemy layer here so the aim ray stops on enemies instead of passing through them to the ground behind.")]
    public LayerMask enemyLayer;
    public GameObject bulletprefab;
    public Transform attackpoint;
    public Transform cam;
    public Transform crosshairObject;
    public float maxRange = 10f;
    public float selfheal = 10f;
    public bool hasShield = false;
    private Health _health;
    public PlayerInventory _inventory;
    public MultishotAbility _multishot;
    public DashAbility _dash;
    public TrapAbility _trapability;
    public PowerShotAbility _powershot;
    public IInteractable _nearbyInteractable;
    public AutoAttack _autoAttack;
    public float movespeed = 5f;
    private Renderer _renderer;
    bool isSpeedBoosed;
    private CharacterController _cc;
    private Vector3 _direction;
    private float _verticalVelocity;
    public float crosshairSmoothSpeed = 15f;
    public float turnSpeed = 720f; // degrees per second, character facing rotation
    private Vector3 _targetCrosshairPos;
    private Camera _cam;
    private MeshRenderer _crosshairRenderer;
    void Awake()
    {
        _health = GetComponent<Health>();
        _renderer = GetComponent<Renderer>();
        _inventory = GetComponent<PlayerInventory>();
        _cc = GetComponent<CharacterController>();
        _multishot = GetComponent<MultishotAbility>();
        _trapability = GetComponent<TrapAbility>();
        _powershot = GetComponent<PowerShotAbility>();
        _dash = GetComponent<DashAbility>();
        _autoAttack = GetComponent<AutoAttack>();
        _cam = Camera.main;
        if (crosshairObject != null) _crosshairRenderer = crosshairObject.GetComponent<MeshRenderer>();
        Debug.Log($"Ready {playerName}");
    }
    void FixedUpdate()
    {
        if (_cc.isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
        }
        Vector3 moveDirection = _direction * movespeed;
        moveDirection.y = _verticalVelocity;
        if (_dash.isDashActive == false)
        {
            _cc.Move(moveDirection * Time.fixedDeltaTime);
        }

    }
    void Update()
    {
        if (IsDead()) return;
        if (_cam == null) _cam = Camera.main;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        Vector3 finalPoint;
        LayerMask aimMask = groundLayer | enemyLayer | _autoAttack.obstacleLayer;
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, 100f, aimMask);
        bool hitGround = hitSomething && (((1 << hit.collider.gameObject.layer) & groundLayer.value) != 0);

        if (hitSomething)
        {
            finalPoint = hit.point;
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                finalPoint = ray.GetPoint(rayDistance);
            }
            else
            {
                finalPoint = transform.position + transform.forward * 5f;
            }
        }
        bool withinRange = Vector3.Distance(transform.position, finalPoint) <= maxRange;
        if (crosshairObject != null)
        {
            crosshairObject.position = Vector3.Lerp(crosshairObject.position, finalPoint, Time.deltaTime * 20f);

            if (hitGround)
            {
                // keep it lying flat on the surface, tilted to match the normal,
                // instead of standing the crosshair up along the normal
                crosshairObject.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(90, 0, 0);
            }
            else
            {
                crosshairObject.rotation = Quaternion.Euler(90, 0, 0);
            }
            if (_crosshairRenderer != null)
            {
                _crosshairRenderer.material.color = withinRange ? Color.blue : Color.grey;
            }
        }
        Vector3 lookTarget = new Vector3(finalPoint.x, transform.position.y, finalPoint.z);
        Vector3 lookDir = lookTarget - transform.position;
        if (lookDir.sqrMagnitude > 0.0001f)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0) && !_inventory.isInventoryopen)
        {
            if (withinRange)
            {
                _autoAttack.TryShot(finalPoint);
            }
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(moveX, 0, moveY).normalized;
        HandleInput(finalPoint);
    }

    void HandleInput(Vector3 targetpoint)
    {
        if (Input.GetKeyDown(KeyCode.E)) _inventory.UseHealItem();
        if (Input.GetKeyDown(KeyCode.Q)) _inventory.UseSpeedItem();
        if (Input.GetKeyDown(KeyCode.H)) HealAmount(selfheal);
        if (Input.GetKeyDown(KeyCode.I)) _inventory.ToggleInventory();
        if (Input.GetKeyDown(KeyCode.O) && _inventory.isInventoryopen)
        {
            _inventory.DeleteFromInventory();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _nearbyInteractable?.Interact(this);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) _multishot.TryUseMultishot(targetpoint);
        if (Input.GetKeyDown(KeyCode.Alpha2)) _trapability.TryUseTrapAbility(targetpoint);
        if (Input.GetKey(KeyCode.Alpha3)) _powershot.PowerShotCharge(targetpoint);
        if (Input.GetKeyUp(KeyCode.Alpha3) && _powershot.ChargeTime > 0) _powershot.TryUsePowerShot(targetpoint);
        if (Input.GetKey(KeyCode.Alpha4)) _dash.TryUseDash(targetpoint);

    }

    public void TakeDamage(float damageamount)
    {
        if (IsDead()) return;
        if (_renderer != null)
        {
            StartCoroutine(FlashColor(Color.red));
        }
        if (hasShield)
        {
            damageamount /= 2;
            Debug.Log($"{playerName} shielded and got {damageamount} damage");
        }
        _health.TakeDamage(damageamount);
        Debug.Log($"{playerName} has {_health.currentValue} hp");
        if (IsDead())
        {
            Debug.Log("Died");
            if (_renderer != null)
            {
                _renderer.material.color = Color.black;
                transform.localScale = Vector3.zero;
            }

        }
    }

    public bool IsDead()
    {

        return _health.currentValue <= 0;

    }
    public void HealAmount(float healamount)
    {
        if (IsDead()) return;
        if (_renderer != null)
        {
            StartCoroutine(FlashColor(Color.yellow));
        }
        _health.Heal(healamount);
        Debug.Log($"Healed for {healamount}. Current hp is {_health.currentValue}");
    }
    IEnumerator FlashColor(Color color)
    {
        _renderer.material.color = color;
        yield return new WaitForSeconds(0.2f);
        if (!IsDead() && _renderer != null)
        {
            _renderer.material.color = Color.white;
        }
    }
    IEnumerator ApplySpeedBoost(float multiplier, float duration)
    {
        if (isSpeedBoosed) yield break;

        isSpeedBoosed = true;
        float originalspeed = movespeed;
        movespeed = movespeed * multiplier;
        yield return new WaitForSeconds(duration);
        movespeed = originalspeed;
        isSpeedBoosed = false;
    }

    public void StartSpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(ApplySpeedBoost(multiplier, duration));
    }
}