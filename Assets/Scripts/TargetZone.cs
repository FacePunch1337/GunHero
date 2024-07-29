using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public static TargetZone Instance { get; private set; }
    [SerializeField] public GameObject zone;
    [SerializeField] private SphereCollider zoneTrigger;
    private float radius = 10f;
    private bool active = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        switch (active)
        {
            case true:
                zone.gameObject.SetActive(true);
                break;
            case false:
                zone.gameObject.SetActive(false);
                break;
        }
        zoneTrigger.radius = radius;
        float scale = zoneTrigger.radius;
        zone.transform.localScale = new Vector3(scale, scale, scale);
        zone.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    public void SetActiveZone(bool value) { active = value; }

    public void SetRadius(float value) { radius = value; }
    public float GetRadius() { return radius; }


}
