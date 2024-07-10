using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Update()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y; // block camera from rotation
        transform.position = newPosition;
    }
}
