using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerLife : MonoBehaviour
{
    [SerializeField]
    int maxLife = 5;

    public Text lifeUI;
    public Text coinsUI;
    int startLife = 5;

    [HideInInspector]
    public int life;
    [HideInInspector]
    public int coins;

    [HideInInspector]
    public Vector3 spawn;

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        life = startLife;
        spawn = transform.position;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeUI.text = "x" + life.ToString();
        coinsUI.text = "x" + coins.ToString();

        if (coins >= 100)
        {
            coins -= 100;
            life++;
        }

        life = (int)Mathf.Clamp((float)life, -1f, (float)maxLife);

        if (life < 0)
            GetComponent<ChangeScene>().LoadScene();
    }

    public void Hit(int damage)
    {
        life -= damage;

        controller.enabled = false;
        transform.position = spawn;
        controller.enabled = true;
    }
}
