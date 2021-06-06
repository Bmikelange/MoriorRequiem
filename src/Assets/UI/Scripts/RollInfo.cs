using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollInfo : MonoBehaviour {
    // Start is called before the first frame update
    TMP_Text effectName, result;

    private void Awake() {
        effectName = transform.GetChild(0).GetComponent<TMP_Text>();
        result = transform.GetChild(1).GetComponentInChildren<TMP_Text>();
    }

    public void SetEffect(DiceEffect effect, Vector2Int res) {
        effectName.text = effect.name + " : ";
        result.text = res.x + "+" + res.y;
    }
}
