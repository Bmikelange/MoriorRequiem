using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyRoomUI : MonoBehaviour {
    //Button button;

    // Start is called before the first frame update
    void Start() {
        //button = GetComponentInChildren<Button>();
        GameManager.instance.HealPlayer();
        //button.onClick.AddListener(() => { GameManager.instance.SetGameState(GameState.MAP); });
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameManager.instance.SetGameState(GameState.MAP);
        }
    }
}
