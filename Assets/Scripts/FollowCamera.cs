using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public static FollowCamera Instance { get; private set; } 
    public Transform target;  
    public Vector3 offset = new Vector3(-15f, 20f, -15f); 
    public float smoothSpeed = 0.125f; 
    public Transform cameraHolder; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        cameraHolder.rotation = Quaternion.Euler(45f, 45f, 0f);
        Camera.main.orthographic = true;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(cameraHolder.position, desiredPosition, smoothSpeed);
            cameraHolder.position = smoothedPosition;
        }
    }
}
