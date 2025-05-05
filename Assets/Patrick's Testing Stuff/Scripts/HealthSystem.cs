// Written By: Patrick Leonard
// 3/29/25

// HEY YOU!
// YES YOU. DO. NOT. EDIT. THIS. SCRIPT. THIS SCRIPT IS RESPONSIBLE FOR TRACKING HEALTH ONLY.
// IF YOU NEED SOMETHING SPECIAL TO HAPPEN WHEN YOUR OBJECT GETS DAMAGED/DIES, YOU MUST SUBSCRIBE TO THE CORRESPONDING EVENT.
// Please. Pretty please. With sugar on top. If you'd like more information, google "Single Responsibility Principle".

// If, for example, you want your thing to do a specific animation when it dies, subscribe to the OnDeath event created below.
// Do not shove animation logic in here. It makes debugging stupid hard. Don't even think about it. Not even once.

using System;
using UnityEditor;
using UnityEngine;

namespace HealthComponents
{
    public class HealthSystem : MonoBehaviour
    {
        // Unity-Exposed Properties
        [Header("Initial Values:")]
        [SerializeField]
        [Tooltip("What health should this object start with?")]
        private float InitialHealth = 100f;

        [SerializeField]
        [Tooltip("What's the maximum health this object should start with?")]
        private float InitialMaxHealth = 100f;

        // Properties
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        // Calculated Properties
        public bool IsDead { get => CurrentHealth <= 0.0f; }

        // Events
        public event Action OnDeath;
        public event Action<float, float> OnHealthChanged; // Old Health, New Health

        // Unity Method Implementations
        private void Awake()
        {
            MaxHealth = InitialMaxHealth;
            CurrentHealth = InitialHealth;
        }

        private void OnValidate()
        {
            if (InitialHealth > InitialMaxHealth) Debug.LogWarning("InitialHealth is greater than InitialMaxHealth. Snapping values...");

            InitialHealth = Mathf.Max(0, InitialHealth);
            InitialMaxHealth = Mathf.Max(InitialHealth, 1, InitialMaxHealth);
        }

        // Public Methods
        public void SetHealth(float value)
        {
            if (IsDead) return;

            float old_value = CurrentHealth;
            float new_value = Mathf.Clamp(value, 0, MaxHealth);

            if (Mathf.Approximately(old_value, new_value)) return;

            CurrentHealth = new_value;

            if (CurrentHealth <= 0f) InvokeOnDie();
            InvokeOnHealthChange(old_value, CurrentHealth);
        }

        public void ModifyHealth(float delta) => SetHealth(CurrentHealth + delta);

        public void SetMaxHealth(float value)
        {
            if (IsDead) return;

            MaxHealth = Mathf.Max(0f, value);

            if (CurrentHealth > MaxHealth) SetHealth(MaxHealth);
        }

        public void ModifyMaxHealth(float delta) => SetMaxHealth(MaxHealth + delta);


        // Event Invokers
        private void InvokeOnDie()
        {
            OnDeath?.Invoke();
        }

        private void InvokeOnHealthChange(float old_amount, float new_amount)
        {
            OnHealthChanged?.Invoke(old_amount, new_amount);
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(HealthSystem))]
    public class MyComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HealthSystem comp = (HealthSystem) target;

            EditorGUILayout.LabelField("Current Values:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Max Health", comp.MaxHealth.ToString());
            EditorGUILayout.LabelField("Current Health", comp.CurrentHealth.ToString());
        }
    }
#endif
}