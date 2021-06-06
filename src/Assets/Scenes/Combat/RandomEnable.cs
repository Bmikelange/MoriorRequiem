using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnable : MonoBehaviour {
    private void Awake() {
        gameObject.SetActive(Random.Range(0, 2) == 0 ? true : false);
    }
}
