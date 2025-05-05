// Written By: Patrick Leonard

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [Header("Appearance")]
    public float appearDuration = 0.2f;

    void Awake()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(GrowIn());
    }

    IEnumerator GrowIn()
    {
        float t = 0f;
        while (t < appearDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.SmoothStep(0f, 1f, t / appearDuration);
            transform.localScale = Vector3.one * s;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}