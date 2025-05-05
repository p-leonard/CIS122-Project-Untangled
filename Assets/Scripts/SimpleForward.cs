// Written By: Patrick Leonard

using UnityEngine;

public class SimpleForward : MonoBehaviour
{
    public float speed = 1.0f;
    public float lifetime = 10.2f;

    void Update()
    {
        lifetime -= Time.deltaTime;

        transform.Translate(Vector3.forward * speed, Space.Self);

        if (lifetime <= 0) Destroy(gameObject);
    }
}
