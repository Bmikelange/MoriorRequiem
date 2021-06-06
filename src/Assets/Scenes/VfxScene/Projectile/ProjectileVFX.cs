using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVFX : MonoBehaviour {
    public Vector3 srcPosition;
    public Vector3 targetPosition;
    public float time = 1f;
    public float percent = 0f;
    public bool reversed = false;

    Vector3 lastPosition = Vector3.zero;

    public void Init(Vector3 _srcPosition, Vector3 _targetPosition, GameObject _vfx, float _time, bool _reversed = false) {
        time = _time;
        reversed = _reversed;
        percent = 0f;

        if (!reversed) {
            srcPosition = _srcPosition;
            targetPosition = _targetPosition;
        } else {
            srcPosition = _targetPosition;
            targetPosition = _srcPosition;

        }

        GameObject obj = Instantiate(_vfx, transform.position, transform.rotation, transform);
        lastPosition = transform.position;
    }

    void Update() {
        percent += Time.deltaTime / time;
        if (percent >= 1) {
            GameObject.Destroy(gameObject, 0.125f);
        } else {
            lastPosition = transform.position;
            transform.position =
                Vector3.Lerp(srcPosition, targetPosition, percent) +
                new Vector3(0, 1f - 4f * (0.5f - percent) * (0.5f - percent), 0) * 2f;

            // Rotation
            Vector3 diff = transform.position - lastPosition;
            transform.rotation = Quaternion.LookRotation(diff);
        }

    }
}
