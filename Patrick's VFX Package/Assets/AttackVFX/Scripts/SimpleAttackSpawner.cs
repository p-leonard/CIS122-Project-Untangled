// Written By: Patrick Leonard

using UnityEngine;

public class SimpleAttackSpawner : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("Prefab of the projectile to spawn (must have Rigidbody or Movement script)")]
    public GameObject projectilePrefab;
    [Tooltip("Y position (world space) at which all projectiles should be spawned")]
    public float fixedElevation = 1.5f;

    [Header("Spawn Timing")]
    [Tooltip("How often, in seconds, to fire a projectile")]
    public float spawnInterval = 2f;


    void Start()
    {
        if (projectilePrefab == null) Debug.LogWarning("Projectile Prefab is Null!");
        InvokeRepeating(nameof(SpawnProjectile), 0f, spawnInterval);
    }

    void SpawnProjectile()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            fixedElevation,
            transform.position.z
        );

        Quaternion spawnRot = Quaternion.LookRotation(transform.forward, Vector3.up);

        GameObject proj = Instantiate(projectilePrefab, spawnPos, transform.rotation);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(SpawnProjectile));    
    }
}
