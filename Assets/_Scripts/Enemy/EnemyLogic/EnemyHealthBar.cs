using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Health health;
    private Camera cam;
    void Start()
    {
        slider.value = health.currentValue / health.maxValue;
    }
    void Awake()
    {
        health = GetComponentInParent<Health>();
        cam = Camera.main;
    }

    void OnEnable()
    {
        health.OnResourceChanged += HandleHealthChanged;
    }

    void OnDisable()
    {
        health.OnResourceChanged -= HandleHealthChanged;
    }

    void LateUpdate()
    {
        transform.forward = cam.transform.forward;
    }

    void HandleHealthChanged(float ratio)
    {
        slider.value = ratio;
    }

}
