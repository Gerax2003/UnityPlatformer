using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    enum ToLoad
    {
        TestScene,
        Menu,
        Tutorial,
        Level1,
        Level2,
        GameOver,
        BossFight,
        Quit
    }

    [SerializeField]
    ToLoad scene = ToLoad.TestScene;

    // Start is called before the first frame update
    public void LoadScene()
    {
        Debug.Log(scene.ToString());
        if (scene.ToString() != "Quit")
            SceneManager.LoadScene(scene.ToString());
        else
            Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
