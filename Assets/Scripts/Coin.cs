using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerLife player = other.GetComponent<PlayerLife>();
            player.coins += value;
            Destroy(transform.parent.gameObject);
        }
    }
}
