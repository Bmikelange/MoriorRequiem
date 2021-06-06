using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingObject : MonoBehaviour {
    float shaking = 0f;
    [SerializeField]
    float shakingMaxOffset = 1f;
    [SerializeField]
    float time = 1f;

    public Vector3 initPos;

    void Start() {
        initPos = transform.localPosition;
    }

    void Update() {
        if (shaking == 0f) return;

        shaking = Mathf.Max(shaking - Time.deltaTime / time, 0f);

        transform.localPosition = initPos + new Vector3(
            shakingMaxOffset * shaking * shaking * (Random.Range(0f, 1f) * 2.0f - 1.0f),
            0f,
            shakingMaxOffset * shaking * shaking * (Random.Range(0f, 1f) * 2.0f - 1.0f)
        );

    }

    public void setShaking(float value) { shaking = Mathf.Clamp(value, 0f, 1f); }

    public bool isShaking() { return shaking > 0f; }
}
