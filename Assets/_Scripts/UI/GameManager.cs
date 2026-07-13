using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI totalscore;
    public float survivaltime = 0;
    public TextMeshProUGUI totaltime;

    void Start()
    {
        totalscore.text = "Score " + score;
        totaltime.text = "time " + survivaltime;
    }
    void Update()
    {
        survivaltime += Time.deltaTime;
        totaltime.text = "time " + Mathf.Round(survivaltime).ToString();
    }
    public void AddScore(GameObject enemy)
    {
        score += 1;
        if (totalscore != null)
        {
            totalscore.text = "Score " + score;
        }

    }
    private void OnEnable()
    {
        Health.OnEnemyDied += AddScore;
    }
    private void OnDisable()
    {
        Health.OnEnemyDied -= AddScore;
    }
}
