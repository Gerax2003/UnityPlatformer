using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    float rayLen = 0.05f;
    [SerializeField]
    float topCorrection = 0.075f;

    public GameObject heart;
    [SerializeField]
    int dropRate = 20; // Drop rate of hearts, between 0 and 100

    public float speed = 1f;
    public float deathForce = 5f; // Force that ejects upwards the player on death

    new Rigidbody rigidbody;

    Vector3 direction = new Vector3(-1f, 0f, 0f);

    [HideInInspector]
    public float height;
    [HideInInspector]
    public Vector3 verticalOffset;
    [HideInInspector]
    public Vector3 horizontalOffset;
    [HideInInspector]
    public Vector3 horizontalCorrection;
    [HideInInspector]
    public float radius;

    void Start()
    {
        radius = 0.5f * transform.localScale.x;
        height = transform.localScale.y;
        verticalOffset = new Vector3(0f, 0.7f * height, 0f);
        horizontalOffset = new Vector3(0.7f * radius, 0f, 0f);
        horizontalCorrection = new Vector3(topCorrection, 0f, 0f);
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (CheckCollisionInFront() || CheckGroundInFront())
            direction = -direction;

        CheckTopHitWithPlayer(); // Only checking top hit since a trigger is used for the other sides
                                 // (Probably more optimized and easier to implement this way)

        transform.Translate(direction * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private bool CheckGroundInFront()
    {
        RaycastHit hit;        // Those values are placeholder used to overload Physics.Raycast to 
        LayerMask mask = ~0;   // ignore triggers when raycasting
        bool ray;

        ray = Physics.Raycast(transform.position - verticalOffset + (direction.x * horizontalOffset), Vector3.Normalize(new Vector3(direction.x * 0.2f, -0.5f, 0f)), out hit, radius + rayLen, mask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(transform.position - verticalOffset + (direction.x * horizontalOffset), Vector3.Normalize(new Vector3(direction.x * 0.5f, -0.5f, 0f)) * (radius + rayLen), Color.blue);

        if (!ray)
            return true;
        else
            return false;
    }

    private bool CheckCollisionInFront()
    {
        RaycastHit hit1, hit2, hit3;
        LayerMask mask = ~0;

        bool ray1, ray2, ray3;

        ray1 = Physics.Raycast(transform.position, direction, out hit1, radius + rayLen, mask, QueryTriggerInteraction.Ignore);
        ray2 = Physics.Raycast(transform.position + verticalOffset, direction, out hit2, radius + rayLen, mask, QueryTriggerInteraction.Ignore);
        ray3 = Physics.Raycast(transform.position - verticalOffset, direction, out hit3, radius + rayLen, mask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(transform.position, direction * (radius + rayLen), Color.white);
        Debug.DrawRay(transform.position + verticalOffset, direction * (radius + rayLen), Color.white);
        Debug.DrawRay(transform.position - verticalOffset, direction * (radius + rayLen), Color.white);

        if (ray1 || ray2 || ray3)
            return true;
        else
            return false;
    }

    private bool CheckTopHitWithPlayer()
    {
        RaycastHit hit;      // Those values are placeholder used to overload Physics.Raycast to 
        LayerMask mask = ~0; // ignore triggers when raycasting over the character's head
        
        bool ray1, ray2, ray3;
        
        ray1 = Physics.Raycast(transform.position + (direction * speed * Time.deltaTime) + verticalOffset, 
            Vector3.up, out hit, 0.4f * height, mask, QueryTriggerInteraction.Ignore);

        if (ray1 && hit.transform.tag == "Player")
        {
            KillEnemy(hit.transform.GetComponent<PlayerMove>());
            return true;
        }
        
        ray2 = Physics.Raycast(transform.position + (direction * speed * Time.deltaTime) + verticalOffset + (horizontalOffset + horizontalCorrection), 
            Vector3.up, out hit, 0.4f * height, mask, QueryTriggerInteraction.Ignore);

        if (ray2 && hit.transform.tag == "Player")
        {
            KillEnemy(hit.transform.GetComponent<PlayerMove>());
            return true;
        }

        ray3 = Physics.Raycast(transform.position + (direction * speed * Time.deltaTime) + verticalOffset - (horizontalOffset + horizontalCorrection), 
            Vector3.up, out hit, 0.4f * height, mask, QueryTriggerInteraction.Ignore);

        if (ray3 && hit.transform.tag == "Player")
        {
            KillEnemy(hit.transform.GetComponent<PlayerMove>());
            return true;
        }

        Debug.DrawRay(transform.position + (direction * speed * Time.deltaTime) + verticalOffset, Vector3.up * 0.4f * height, Color.yellow);
        Debug.DrawRay(transform.position + (direction * speed * Time.deltaTime) + verticalOffset + (horizontalOffset + horizontalCorrection), Vector3.up * 0.4f * height, Color.yellow);
        Debug.DrawRay(transform.position + (direction * speed * Time.deltaTime) + verticalOffset - (horizontalOffset + horizontalCorrection), Vector3.up * 0.4f * height, Color.yellow);
        
        return false;
    }

    private void KillEnemy(PlayerMove player)
    {
        if (Random.Range(0, 100) <= dropRate)
            Instantiate(heart, transform.position, Quaternion.identity);

        Destroy(transform.gameObject);
        player.movement.y = deathForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!CheckTopHitWithPlayer())
            {
                PlayerLife player = other.GetComponent<PlayerLife>();
                player.Hit(1);
            }
        }
    }
}
