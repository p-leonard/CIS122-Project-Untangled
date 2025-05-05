// Written By: Patrick Leonard

using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject attackPrefab;

    public float spawnInterval = 1f;

    public float yAdjust = -1f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnAttack), 0f, spawnInterval);
    }

    void SpawnAttack()
    {
        float yaw = transform.eulerAngles.y;

        Quaternion axisOnlyRot = Quaternion.Euler(0f, yaw, 0f);

        Vector3 spawnPos = transform.position + Vector3.down * yAdjust;

        Instantiate(attackPrefab, spawnPos, axisOnlyRot);
        
    }
}
