using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDelay : MonoBehaviour {

    [SerializeField]
    float time = 1f;

    [SerializeField]
    GameObject objToActive;

    // Start is called before the first frame update
    void Start() {

        StartCoroutine(waitActive());

    }

    IEnumerator waitActive() {
        yield return new WaitForSeconds(time);
        objToActive.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

    }
}
