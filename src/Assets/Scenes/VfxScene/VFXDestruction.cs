using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDestruction : MonoBehaviour {
    [SerializeField]
    float destructionTime = 1f;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, destructionTime);
    }

    // Update is called once per frame
    void Update() {

    }
}
