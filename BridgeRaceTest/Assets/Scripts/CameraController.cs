using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Objects From Scene"), SerializeField]
    private Transform player;

    [Header("Variables To Control"), SerializeField]
    private Vector3 distance;

    private void FixedUpdate()
    {
        transform.position = player.position + distance;
    }
}