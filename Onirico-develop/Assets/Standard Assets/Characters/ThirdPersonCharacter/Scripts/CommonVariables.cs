using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonVariables : MonoBehaviour
{
    public Vector3 playerPosition = new Vector3(1000f, 1000f, 1000f);         // The last global sighting of the player.
    public Vector3 resetPlayerPosition = new Vector3(1000f, 1000f, 1000f);    // The default position if the player is not in sight.

    public float moveSpeed = 0f;
    
}
