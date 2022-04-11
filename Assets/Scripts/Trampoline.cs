using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    float force = 10f; // velocity added to the jump
    [SerializeField]
    float speedTransfer = 1f; // influence of the player's approach velocity on the bounce

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            player.movement.y = speedTransfer * Mathf.Abs(player.movement.y) + force;
        }
    }
}
