using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenVFX : MonoBehaviour {

    public float vfxScale = 1f;

    Dictionary<GameObject, GameObject> vfxs = new Dictionary<GameObject, GameObject>();

    public void processEffects(List<EntityEffect> effects) {

        List<GameObject> vfxToRemove = new List<GameObject>(vfxs.Keys);

        foreach (var effect in effects) {
            if (effect.vfx == null) continue;
            vfxToRemove.Remove(effect.vfx.vfxObject);

            if (!vfxs.ContainsKey(effect.vfx.vfxObject)) {
                GameObject go = Instantiate(effect.vfx.vfxObject, transform.position, transform.rotation, transform);
                go.transform.localScale = Vector3.one * vfxScale;
                vfxs.Add(effect.vfx.vfxObject, go);
            }
        }

        foreach (var vfx in vfxToRemove) {
            GameObject.Destroy(vfxs[vfx]);
            vfxs.Remove(vfx);
        }

    }

    public void attackBlocked() {
        foreach (var vfx in vfxs) {
            ShakingObject sO = vfx.Value.GetComponent<ShakingObject>();
            if (sO != null) {
                sO.setShaking(1f);
            }
        }
    }

    private void Update() {

    }
}
