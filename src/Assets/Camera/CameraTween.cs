using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTween : MonoBehaviour {
    float time = 1f;
    float percent = 1f;

    Transform trSrc;
    Transform trDst;

    private void Start() {
        trSrc = transform;
        trDst = transform;
    }

    private void Update() {
        if (percent == 1f) return;

        percent = Mathf.Min(percent + Time.deltaTime / time, 1f);

        transform.position = Vector3.Lerp(trSrc.position, trDst.position, percent);
        transform.rotation = Quaternion.Slerp(trSrc.rotation, trDst.rotation, percent);
    }

    public void setDestination(Transform tr, float _time = 1f) {
        time = _time;
        trSrc = transform;
        trDst = tr;
        percent = 0f;
    }
}
