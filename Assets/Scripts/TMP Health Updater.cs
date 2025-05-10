// Written By: Patrick Leonard
// 5/9/2025

using HealthComponents;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPHealthUpdater : MonoBehaviour
{
    // Fields
    private HealthSystem HS;
    private TextMeshProUGUI TEXT;

    // Properties
    [SerializeField]
    private GameObject trackedEntity;

    // Helper Methods
    private string HealthString(float current, float max) => $"{current:F0}/{max:F0}";

    private void HandleHealthChange(float oldValue, float newValue)
    {
        TEXT.text = HealthString(newValue, HS.MaxHealth);
    }

    // Unity Methods
    void Start()
    {
        HS = trackedEntity.GetComponent<HealthSystem>();
        TEXT = GetComponent<TextMeshProUGUI>();

        Debug.Assert(HS != null, "Couldn't find a health system.");

        HS.OnHealthChanged += HandleHealthChange;

        HandleHealthChange(HS.CurrentHealth, HS.MaxHealth);
    }
}
