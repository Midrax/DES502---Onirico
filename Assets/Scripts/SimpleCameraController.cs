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
    void Update()
    {
        // Move camera to XZ-plane of target
        transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        // Adjust distance from player while preserving camera look angle
        transform.position = target.position + (transform.position - target.position).normalized * distance;
        // Face player
        transform.LookAt(target);
    }
}