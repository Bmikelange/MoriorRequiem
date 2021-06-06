using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBossAnim : MonoBehaviour {

    [SerializeField]
    GameObject boss;

    // Start is called before the first frame update
    void Start() {
        Invoke("nextRoom", 9f);
        Invoke("playBossAnim", 3f);

    }

    void nextRoom() {
        GameManager.instance.SetGameState(GameState.BOSS);
    }

    void playBossAnim() {
        boss.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

    }
}
