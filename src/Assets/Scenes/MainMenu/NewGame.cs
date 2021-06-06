using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour {
    
    void GMNewGame()
    {
        GameManager.instance.NewGame();
    }

    public void StartNewGame() {
        List<Animation> animations = new List<Animation>(GameObject.FindObjectsOfType<Animation>());
        foreach(var anim in animations)
        {
            anim.Play();
        }
        Invoke("GMNewGame", 2f);
    }
}
