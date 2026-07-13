using UnityEngine;
using UnityEngine.UI;

public class AbilityUiSlot : MonoBehaviour
{
    [SerializeField] private BaseAbility ability;
    [SerializeField] private Image cooldownOverlay;

    public void Start()
    {
        ability.OnCooldownChanged += UpdateCooldown;
    }
    public void UpdateCooldown(float progress)
    {
        cooldownOverlay.fillAmount = progress;
    }
    void OnDestroy()
    {
        ability.OnCooldownChanged -= UpdateCooldown;
    }
}
