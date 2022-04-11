using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickHit : MonoBehaviour
{
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            PlayerMove player = other.GetComponent<PlayerMove>();
            player.movement.y = 0f;
        }
    }
}
