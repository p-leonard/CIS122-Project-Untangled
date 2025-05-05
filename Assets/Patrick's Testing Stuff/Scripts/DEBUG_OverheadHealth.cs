using TMPro;
using UnityEngine;

public class DEBUG_OverheadHealth : MonoBehaviour
{
    [Tooltip("Offset above the object, world space")]
    public Vector3 worldOffset = new(0, 2, 0);

    public TextMeshPro textPrefab;

    private TextMeshPro _instance;
    [SerializeField]
    private string _text = "";

    void Start()
    {
        _instance = Instantiate(textPrefab, transform);
        _instance.text = _text;
        _instance.alignment = TextAlignmentOptions.Center;
        _instance.transform.localPosition = worldOffset;
    }

    private void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        _instance.transform.rotation = Quaternion.LookRotation(
            _instance.transform.position - cam.transform.position,
            Vector3.up

        );
    }

    void OnDestroy()
    {
        if (_instance != null) Destroy(_instance.gameObject);
    }
}
