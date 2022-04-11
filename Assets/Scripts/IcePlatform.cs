using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    [SerializeField]
    float givenInertia = 2f;
    float baseInertia = 0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            baseInertia = player.inertia;
            player.inertia = givenInertia;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            player.inertia = baseInertia;
        }
    }
}
