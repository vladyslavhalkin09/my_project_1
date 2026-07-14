using UnityEngine;

public class CharacterStatsHolder : MonoBehaviour
{
    [SerializeField]
    private CharacterStats stats;
    public CharacterStats Stats => stats;
}
