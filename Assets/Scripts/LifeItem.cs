using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerLife player = other.GetComponent<PlayerLife>();
            player.life++;
            Destroy(transform.parent.gameObject);
        }
    }
}
