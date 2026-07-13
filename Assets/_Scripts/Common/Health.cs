using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;


public class Health : Resource
{
    // public float hp = 100f;
    // public float maxHp = 100f;
    public TextMeshProUGUI healthtext;
    public GameObject gameOverScreen;
    public static event Action<GameObject> OnEnemyDied;
    // public event Action<float> OnHealthChanged;

    void Start()
    {
        UpdateUi();
    }
    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (currentValue <= 0 && Input.GetKeyDown(KeyCode.R))
            {

                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

    }
    public virtual void TakeDamage(float dmg, DamageType type = DamageType.Normal)
    {
        Debug.Log(gameObject.name + "get hit" + dmg);
        base.Spend(dmg);
        Debug.Log("hp left " + gameObject.name + ":" + currentValue);

        if (currentValue <= 0)
        {
            currentValue = 0;
            Die();
        }

    }
    public void Heal(float healamount)
    {
        if (currentValue <= 0) return;
        base.Restore(healamount);
        UpdateUi();
        Debug.Log(gameObject.name + "healed for " + healamount);


    }
    void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
                Time.timeScale = 0f;
                Debug.Log("stop");
            }
        }
        else
        {
            Debug.Log("Destroyed " + gameObject.name);
            OnEnemyDied?.Invoke(gameObject);
            Destroy(gameObject);
        }

    }
    protected override void UpdateUi()
    {
        base.UpdateUi();
        if (healthtext != null)
        {
            healthtext.text = "Health " + Mathf.Round(currentValue).ToString();
        }
    }
    public void InstantKill()
    {
        TakeDamage(currentValue);
    }
}
