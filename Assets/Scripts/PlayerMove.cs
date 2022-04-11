using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    CharacterController controller;
    
    [HideInInspector]
    public Vector3 movement = Vector3.zero;

    Vector3 sidesOffset;
    float raycastUpLength;

    [SerializeField]
    float speed = 10f;
    [SerializeField]
    float jumpForce = 25f;
    [SerializeField]
    float maxSpeed = 25f; // used to clamp movement speed
    [SerializeField]
    float maxFall = 15f;  // used to clamp fall speed
    [SerializeField]
    public float inertia = 5f; // simulates inertia, used to smoothly stop horizontal movement
    [SerializeField]
    float mass = 1f;

    int jumps = 1;
    int maxJumps = 2;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        sidesOffset = new Vector3(transform.localScale.x / 2f, 0, 0);
        raycastUpLength = transform.localScale.y / 2f + 0.1f;
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            jumps = maxJumps;
            if (Input.GetKey(KeyCode.RightArrow))
                movement.x -= speed;
            if (Input.GetKey(KeyCode.LeftArrow))
                movement.x += speed;
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
                movement.x -= speed / 20;
            if (Input.GetKey(KeyCode.LeftArrow))
                movement.x += speed / 20;

            if (CheckTopHit() && movement.y > 0)
                movement.y = 0f;

            movement.y += Physics.gravity.y * Time.deltaTime * mass;
            jumps = Mathf.Clamp(jumps, 0, maxJumps - 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        { 
            movement.y = jumpForce;
            jumps--;
        }

        movement.y = Mathf.Clamp(movement.y, -maxFall, 250f);
        movement.x = Mathf.Clamp(movement.x, -maxSpeed, maxSpeed);

        if (controller.isGrounded)
            movement.x -= (movement.x * Time.deltaTime) * inertia;
        else
            movement.x -= (movement.x * Time.deltaTime) * (inertia / 2f);

        controller.Move(movement * Time.deltaTime);

        if (transform.position.z != 0f)
        {
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            controller.enabled = true;
        }
    }

    private bool CheckTopHit()
    {
        RaycastHit hit1, hit2, hit3; // Those values are placeholder used to overload Physics.Raycast to 
        LayerMask mask = ~0;         // ignore triggers when raycasting over the character's head

        bool ray1, ray2, ray3;

        ray1 = Physics.Raycast(transform.position, Vector3.up, out hit1, raycastUpLength, mask, QueryTriggerInteraction.Ignore);
        ray2 = Physics.Raycast(transform.position + sidesOffset, Vector3.up, out hit2, raycastUpLength, mask, QueryTriggerInteraction.Ignore);
        ray3 = Physics.Raycast(transform.position - sidesOffset, Vector3.up, out hit3, raycastUpLength, mask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(transform.position, Vector3.up * raycastUpLength, Color.white);
        Debug.DrawRay(transform.position + sidesOffset, Vector3.up * raycastUpLength, Color.white);
        Debug.DrawRay(transform.position - sidesOffset, Vector3.up * raycastUpLength, Color.white);

        if (ray1 || ray2 || ray3)
            return true;
        else
            return false;
    }
}
