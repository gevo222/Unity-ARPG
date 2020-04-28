using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [SerializeField]
    [Range(0.01f,1.0f)]
    private float smoothingFactor = 1.0f;

    void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, smoothingFactor);
    }
}
