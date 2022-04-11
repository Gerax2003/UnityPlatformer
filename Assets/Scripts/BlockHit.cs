using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BlockHit : MonoBehaviour
{
    public GameObject storedObject;
    Vector3 objectPos;

    [SerializeField]
    int objectQuantity = 0;

    public Material emptyMat; // Material to use when the block is empty
    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objectPos = transform.position;
        objectPos.y += transform.localScale.y;
    }

    private void Update()
    {
        if (objectQuantity <= 0)
        {
            meshRenderer.material = emptyMat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (objectQuantity > 0)
            {
                Instantiate(storedObject, objectPos, transform.rotation);
                objectQuantity--;
            }
        }
    }
}
