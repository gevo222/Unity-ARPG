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

    // Update is called once per frame
    void Update ()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, smoothingFactor);
    }
}
