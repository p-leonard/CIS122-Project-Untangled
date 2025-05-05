// Written By: Patrick Leonard
// 3/30/25

using TMPro;
using UnityEngine;

namespace HealthComponents
{
    [RequireComponent(typeof(HealthSystem))]
    public class DEBUG_HealthSystemMonitor : MonoBehaviour
    {
        [Header("Text Settings")]
        public TextMeshPro textPrefab;
        public Vector3 worldOffset = new Vector3(0, 2, 0);

        private HealthSystem hs;
        private TextMeshPro textInstance;

        public void Awake()
        {
            // Get a reference to the health system of the attached object
            hs = GetComponent<HealthSystem>();

            // Subscribe to events I care about
            hs.OnDeath += HandleDeath;
            hs.OnHealthChanged += HandleHealthChange;

            // Create a floating text
            if (textPrefab != null)
            {
                textInstance = Instantiate(textPrefab, transform);
                textInstance.alignment = TextAlignmentOptions.Center;
                textInstance.transform.localPosition = worldOffset;
                UpdateHealthText(hs.CurrentHealth, hs.MaxHealth); // Initialize the text
            }
            else
            {
                Debug.LogWarning("[DEBUG_HealthSystemMonitor] No TextMeshPro prefab assigned.");
            }
        }

        private void LateUpdate()
        {
            if (textInstance == null) return;

            // Make the text always face the camera
            Camera cam = Camera.main;
            if (cam != null)
            {
                textInstance.transform.rotation = Quaternion.LookRotation(
                    textInstance.transform.position - cam.transform.position,
                    Vector3.up
                );
            }
        }

        private void HandleDeath()
        {
            Debug.Log($"[HealthSystem] Event Fire (OnDie) {gameObject.name}");
            UpdateHealthText(0, hs.MaxHealth);
        }

        private void HandleHealthChange(float oldVal, float newVal)
        {
            Debug.Log($"[HealthSystem] Event Fire (OnHealthChange) (from {oldVal} to {newVal}) {gameObject.name}");
            UpdateHealthText(newVal, hs.MaxHealth);
        }

        private void UpdateHealthText(float currentHealth, float maxHealth)
        {
            if (textInstance != null)
            {
                textInstance.text = $"HP: {currentHealth}/{maxHealth}";
            }
        }

        private void OnDestroy()
        {
            if (hs != null)
            {
                hs.OnDeath -= HandleDeath;
                hs.OnHealthChanged -= HandleHealthChange;
            }

            if (textInstance != null)
            {
                Destroy(textInstance.gameObject);
            }
        }
    }
}