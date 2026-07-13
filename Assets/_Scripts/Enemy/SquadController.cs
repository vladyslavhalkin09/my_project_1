using UnityEngine;

public class SquadController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float waitTime;
    public EnemyBase[] members;
    public Vector3[] memberOffsets;
    void Awake()
    {
        for (int i = 0; i < members.Length; i++)
        {
            members[i].squadOffset = memberOffsets[i];
            members[i].squad = this;
        }
    }
    public void OnMemberAggro()
    {
        foreach (EnemyBase member in members)
        {
            if (member == null) continue;
            member.Aggro();
        }
    }
}
