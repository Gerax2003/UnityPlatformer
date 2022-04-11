using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool triggered = false;
    public Material flagMat; // Material to use when the checkpoint is passed
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = transform.Find("Flag").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.gameObject.tag == "Player")
        {
            PlayerLife player = other.GetComponent<PlayerLife>();
            player.spawn = transform.position;
            triggered = true;
            meshRenderer.material = flagMat;
        }
    }
}
