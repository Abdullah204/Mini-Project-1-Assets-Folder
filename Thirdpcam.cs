using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirdpcam : MonoBehaviour
{
    public Transform player; 

    void Update()
    {
        Vector3 offset = new(0, 10, -10);
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 500f);
        transform.LookAt(player.position);
    }
}
