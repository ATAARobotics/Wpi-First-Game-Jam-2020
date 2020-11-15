using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene_manage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Go_To_Game(){
        SceneManager.LoadScene("Level One");
    }

    public void Go_To_Controls() {
        SceneManager.LoadScene("Controls");
    }

    public void Go_To_Test() {
        SceneManager.LoadScene("player-movement-test");
    }
}
