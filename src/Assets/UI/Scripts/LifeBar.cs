using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LifeBar : MonoBehaviour {
    // Start is called before the first frame update
    Image bar;
    TMP_Text text;


    public void setBar(float life, float lifeMax) {
        bar.fillAmount = life / lifeMax;
        text.text = life + "/" + lifeMax;
    }
    void Awake() {
        bar = transform.GetChild(0).GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
    }

}
