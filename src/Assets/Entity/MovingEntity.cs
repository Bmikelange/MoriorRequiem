using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : MonoBehaviour {
    float percent = 0f;
    [SerializeField]
    float maxOffset = 2f;
    [SerializeField]
    float time = 1f;
    float timeSign = -1f;

    Vector3 initPos;

    void Start() {
        initPos = transform.position;
    }

    void Update() {
        if (timeSign < 0f && percent == 0f) return;
        if (timeSign > 0f && percent == 1f) return;

        percent = Mathf.Clamp(percent + timeSign * Time.deltaTime / time, 0f, 1f);

        float fPart = (0.5f - percent);
        transform.position = initPos + transform.forward * maxOffset * percent + Vector3.up * (1f - 4f * fPart * fPart) * 0.5f;

        // float fPart = 2f * (percent - 0.5f);
        // transform.position = initPos + transform.forward * (maxOffset * (1 - fPart * fPart));

        ShakingObject shObj = GetComponent<ShakingObject>();
        if (shObj != null) {
            shObj.initPos = transform.position;
        }

    }

    public void startMove() { percent = 0f; }

    public void setTimeSign(float _timeSign) { timeSign = _timeSign; }
}
