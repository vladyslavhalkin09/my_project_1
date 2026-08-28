using System;
using UnityEngine;
using System.Collections;
public class Player : MonoBehaviour
{
    public string playerName = "test";
    public LayerMask groundLayer;
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
    private Vector3 _targetCrosshairPos;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 finalPoint;
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer);

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
        if (crosshairObject != null)
        {

            crosshairObject.position = Vector3.Lerp(crosshairObject.position, finalPoint, Time.deltaTime * 20f);

            if (hitSomething)
            {
                crosshairObject.rotation = Quaternion.LookRotation(hit.normal);
            }
            else
            {
                crosshairObject.rotation = Quaternion.Euler(90, 0, 0);
            }
            float dist = Vector3.Distance(transform.position, finalPoint);
            crosshairObject.GetComponent<MeshRenderer>().material.color = (dist <= maxRange) ? Color.blue : Color.grey;
        }
        Vector3 lookTarget = new Vector3(finalPoint.x, transform.position.y, finalPoint.z);
        transform.LookAt(lookTarget);

        if (Input.GetMouseButtonDown(0) && !_inventory.isInventoryopen)
        {
            if (Vector3.Distance(transform.position, hit.point) <= maxRange)
            {
                _autoAttack.TryShot(hit.point);
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