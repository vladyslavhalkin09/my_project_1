using System.Collections;
using TMPro;
using UnityEngine;
public enum DamageType
{
    Normal,
    Ability,
    Critical
}

public class DamageText : MonoBehaviour
{
    public float lifetime;
    TMP_Text tmp;

    void Start()
    {
        StartCoroutine(DamageTextRoutine());
    }

    public void Initialize(float damage, DamageType type)
    {
        tmp = GetComponent<TMP_Text>();
        tmp.text = Mathf.Round(damage).ToString();
        switch (type)
        {
            case DamageType.Normal:
                tmp.color = Color.white;
                break;
            case DamageType.Ability:
                tmp.color = Color.yellow;
                break;
            case DamageType.Critical:
                tmp.fontSize *= 1.5f;
                break;
        }
    }

    IEnumerator DamageTextRoutine()
    {
        while (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            transform.position += Vector3.up * 1f * Time.deltaTime;
            Color c = tmp.color;
            c.a -= Time.deltaTime / lifetime;
            tmp.color = c;
            yield return null;
        }
        Destroy(gameObject);
    }
}
