using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Simple camera following player with a certain distance
public class SimpleCameraController : MonoBehaviour
{
    // The target to follow
    public Transform target;
    // The distance to keep
    public float distance = 4.0f;

    // camera rotation script
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float speedX = 4.0f;
    private float speedY = 1.0f;

    void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0,0,-distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * dir;
        transform.LookAt(target.position);

    }
}