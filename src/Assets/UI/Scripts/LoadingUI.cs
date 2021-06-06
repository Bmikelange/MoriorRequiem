using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingUI : MonoBehaviour {
    AudioSource source;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene() {
        source = GetComponent<AudioSource>();
        GameState gameState = GameManager.instance.GetGameState();
        string scene = gameState.ToString();
        if (gameState == GameState.COMBAT) {
            // Choose a random combat scene
            scene += Random.Range(0, GameManager.NB_COMBAT_ROOMS);
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        if (gameState != GameState.MAP && gameState != GameState.MAIN_MENU && gameState != GameState.WIN && gameState != GameState.BOSS) {
            source.Play();
            async.allowSceneActivation = false;
            yield return new WaitForSeconds(1f);
            async.allowSceneActivation = true;
        }
        while (!async.isDone) {
            yield return null;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
