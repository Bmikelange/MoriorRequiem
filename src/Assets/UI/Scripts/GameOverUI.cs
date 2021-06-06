using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {
    Button button;
    // Start is called before the first frame update
    void Start() {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(returnToMainMenu);
    }

    void returnToMainMenu() {
        SceneManager.LoadScene("MAIN_MENU");
    }
}
