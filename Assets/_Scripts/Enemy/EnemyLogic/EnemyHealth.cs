using UnityEngine;
using TMPro;


public class EnemyHealth : Health
{
    public GameObject damagetextprefab;
    public float damageTextHeight = 1f;

    void Start()
    {
        EnemyBase enemyBase = GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            maxValue = enemyBase.data.maxHp;
            currentValue = enemyBase.data.maxHp;
        }
    }
    public override void TakeDamage(float dmg, DamageType type = DamageType.Normal)
    {

        base.TakeDamage(dmg);
        EnemyBase enemyBase = GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.Aggro();
            if (enemyBase.squad != null)
            {
                enemyBase.squad.OnMemberAggro();
            }
        }
        GameObject dmgText = Instantiate(damagetextprefab, transform.position + Vector3.up * damageTextHeight, Quaternion.identity);
        DamageText dmgScript = dmgText.GetComponent<DamageText>();
        dmgScript.Initialize(dmg, type);

    }
}
