using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiledAnim : MonoBehaviour {
    float angularSpeed = 270f;

    void Start() {

    }
    void Update() {
        transform.rotation *= Quaternion.AngleAxis(angularSpeed * Time.deltaTime, Vector3.up);
    }
}
