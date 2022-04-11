using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    int lives = 5;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    Vector3 ejectForce = new Vector3(15f, 15f, 0f);

    Vector3 direction = new Vector3(1f, 0f, 0f);
    PlayerMove player;
    public GameObject baseSpawner;
    public GameObject smartSpawner;
    public GameObject wall;

    void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * direction * speed);

        if(lives <= 0)
        {
            Destroy(baseSpawner);
            Destroy(smartSpawner);
            Destroy(wall);
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("TriggerEntered");
            lives--;
            player = other.GetComponent<PlayerMove>();
            player.movement.y = ejectForce.y;
            StartCoroutine(ForceHorizontalForce());
        }
    }

    IEnumerator ChangeDirection()
    {
        float timeBetweenChange = 3f;
        for (float i = timeBetweenChange; i <= timeBetweenChange + 1f; i -= Time.deltaTime)
        {
            if (i <= 0f)
            {
                direction *= -1f;
                i = timeBetweenChange;
            }
            yield return null;
        }
    }

    IEnumerator ForceHorizontalForce()
    {
        float timeLeft = 2f;

        for (float i = 0; i < timeLeft; i += Time.deltaTime)
        {
            player.movement.x = ejectForce.x;
            yield return null;
        }
    }
}
