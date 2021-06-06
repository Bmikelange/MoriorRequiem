using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    Button button;
    GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        panel = transform.GetChild(0).gameObject;
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(returnToMainMenu);
        panel.SetActive(false);
        Invoke("ActivatePanel", 2.5f);
    }

    void ActivatePanel()
    {
        panel.SetActive(true);
    }

    void returnToMainMenu()
    {
        SceneManager.LoadScene("MAIN_MENU");
    }
}
