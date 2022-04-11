using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    float radius = 15f;
    [SerializeField]
    float speedBoost = 10f; // Smart enemies go faster when they get a target

    GameObject player;
    BaseEnemy enemyController;
    Vector3 direction = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        enemyController = GetComponent<BaseEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();

        if (!enemyController.enabled)
        {
            direction = Vector3.right;
            if (Vector3.Dot(direction, player.transform.position - transform.position) <= 0)
                direction *= -1;

            CheckTopHitWithPlayer();
            transform.Translate(direction * (enemyController.speed + speedBoost) * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
    }

    private bool CheckTopHitWithPlayer()
    {
        RaycastHit hit;      // Those values are placeholder used to overload Physics.Raycast to 
        LayerMask mask = ~0; // ignore triggers when raycasting over the character's head

        bool ray1, ray2, ray3;

        ray1 = Physics.Raycast(transform.position + (direction * (enemyController.speed + speedBoost)* Time.deltaTime) + enemyController.verticalOffset,
            Vector3.up, out hit, 0.4f * enemyController.height, mask, QueryTriggerInteraction.Ignore);

        if (ray1 && hit.transform.tag == "Player")
        {
            PlayerMove player = hit.transform.GetComponent<PlayerMove>();
            Destroy(transform.gameObject);
            player.movement.y = enemyController.deathForce;
            return true;
        }

        ray2 = Physics.Raycast(transform.position + (direction * (enemyController.speed + speedBoost)* Time.deltaTime) + enemyController.verticalOffset 
            + (enemyController.horizontalOffset + enemyController.horizontalCorrection),
            Vector3.up, out hit, 0.4f * enemyController.height, mask, QueryTriggerInteraction.Ignore);

        if (ray2 && hit.transform.tag == "Player")
        {
            PlayerMove player = hit.transform.GetComponent<PlayerMove>();
            Destroy(transform.gameObject);
            player.movement.y = enemyController.deathForce;
            return true;
        }

        ray3 = Physics.Raycast(transform.position + (direction * (enemyController.speed + speedBoost)* Time.deltaTime) + enemyController.verticalOffset 
            - (enemyController.horizontalOffset + enemyController.horizontalCorrection),
            Vector3.up, out hit, 0.4f * enemyController.height, mask, QueryTriggerInteraction.Ignore);

        if (ray3 && hit.transform.tag == "Player")
        {
            PlayerMove player = hit.transform.GetComponent<PlayerMove>();
            Destroy(transform.gameObject);
            player.movement.y = enemyController.deathForce;
            return true;
        }

        Debug.DrawRay(transform.position + (direction * (enemyController.speed + speedBoost)* Time.deltaTime) + enemyController.verticalOffset, 
            Vector3.up * 0.4f * enemyController.height, Color.yellow);
        Debug.DrawRay(transform.position + (direction * (enemyController.speed + speedBoost)* Time.deltaTime) + enemyController.verticalOffset
            + (enemyController.horizontalOffset + enemyController.horizontalCorrection),
            Vector3.up * 0.4f * enemyController.height, Color.yellow);
        Debug.DrawRay(transform.position + (direction * (enemyController.speed + speedBoost)* Time.deltaTime) + enemyController.verticalOffset 
            - (enemyController.horizontalOffset + enemyController.horizontalCorrection), 
            Vector3.up * 0.4f * enemyController.height, Color.yellow);

        return false;
    }

    private void CheckTarget()
    {
        float playerDist = Vector3.Distance(player.transform.position, transform.position);

        if (playerDist <= radius)
            enemyController.enabled = false;
        else
            enemyController.enabled = true;
    }
}
