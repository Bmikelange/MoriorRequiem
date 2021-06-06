using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityInfo : MonoBehaviour {
    LifeBar bar;
    TMP_Text text;
    [SerializeField]
    List<Sprite> effectsIcons = new List<Sprite>();

    Transform statusEffects;

    void Awake() {
        bar = GetComponentInChildren<LifeBar>();
        text = GetComponentInChildren<TMP_Text>();
        statusEffects = transform.GetChild(2);
    }

    public void ChangeLife(int life, int lifeMax) {
        if (bar == null)
            bar = GetComponentInChildren<LifeBar>();
        bar.setBar((float)life, (float)lifeMax);
    }

    public void SetName(string name) {
        text.text = name;
    }

    public void showEffects(List<EntityEffect> effects) {
        foreach (Transform g in statusEffects) {
            GameObject.Destroy(g.gameObject);
        }
        foreach (var effect in effects) {
            GameObject g = new GameObject();
            Image myImage = g.AddComponent<Image>();
            myImage.rectTransform.sizeDelta = new Vector2(30, 30);
            myImage.sprite = effectsIcons[(int)effect.type];
            GameObject txt = new GameObject();
            TMP_Text t = txt.AddComponent<TextMeshProUGUI>();
            // t.autoSizeTextContainer = true;
            t.fontSize = 28;
            t.text = "" + effect.duration;
            t.horizontalAlignment = HorizontalAlignmentOptions.Center;
            t.verticalAlignment = VerticalAlignmentOptions.Middle;
            t.color = Color.black;
            txt.transform.SetParent(g.transform);
            ((RectTransform)txt.transform).sizeDelta = new Vector2(30, 30);
            g.transform.SetParent(statusEffects, false);
            ((RectTransform)txt.transform).pivot = new Vector2(0.5f, 0.5f);
            ((RectTransform)txt.transform).anchorMin = new Vector2(0f, 0f);
            ((RectTransform)txt.transform).anchorMax = new Vector2(1f, 1f);
        }
    }
}
