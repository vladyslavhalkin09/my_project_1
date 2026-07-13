using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }
    public void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
