// Written By: Patrick Leonard
// 5/5/2025

using UnityEngine;
using HealthComponents;
using System;

[RequireComponent(typeof(Collider))]
public class DamageOnTrigger : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("How much damage should be dealt upon entering this trigger?")]
    [SerializeField]
    private float damageAmount = 10f;

    [Tooltip("Destroy on hit?")]
    private bool destroyOnHit = false;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col.isTrigger) Debug.LogWarning("DamageOnTrigger: Collider is marked as not a trigger, setting to true.");
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HealthSystem>(out HealthSystem hs) && !hs.IsDead)
        {
            hs.ModifyHealth(-damageAmount);

            if (destroyOnHit) Destroy(gameObject);
        }
    }
}
