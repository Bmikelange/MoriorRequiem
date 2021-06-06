using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour {
    [SerializeField]
    bool displayAllInfo = false;

    TMP_Text title = null;
    TMP_Text description = null;
    Image image = null;
    // GameManager1 gameManager;

    // Start is called before the first frame update
    void Awake() {
        // gameManager = GetComponentInParent<GameManager1>();
        title = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        image = GetComponentInChildren<Image>();
        // text.fontSize = 
    }

    static string and(Vector2Int pow) {
        return pow.x * 2 + " and " + pow.y * 2;
    }

    static string to(Vector2Int pow) {
        return pow.x * 2 + " to " + pow.y * 2;
    }

    static string target(EffectTarget target) {
        string txt = "";
        switch (target) {
            case EffectTarget.Player:
                txt = "yourself";
                break;
            case EffectTarget.Enemy:
                txt = "an enemy";
                break;
            case EffectTarget.Enemies:
                txt = "all enemies";
                break;
        }
        return txt;
    }

    static string shorter(Vector2Int pow) {
        return "(" + pow.x * 2 + "," + pow.y * 2 + ")";
    }

    string shorterTarget(EffectTarget target) {
        string txt = "";
        switch (target) {
            case EffectTarget.Player:
                txt = " on yourself";
                break;
            case EffectTarget.Enemies:
                txt = " on all enemies";
                break;
        }
        return txt;
    }

    static public string DiceDescription(Dice d, bool allInfo = true) {
        string description = "";
        if (!allInfo) {
            return description;
        }
        description = "Cooldown : " + d.cooldown + " turn" + (d.cooldown > 1 ? "s" : "") + ".\n";
        foreach (DiceEffect eff in d.effects) {
            if (eff.power.y == 0) continue;
            description += eff.name + " : ";
            switch (eff.type) {
                case EffectType.Damage:
                    description += "Deals between " + and(eff.power) + " damage to " + target(eff.target);
                    break;
                case EffectType.Heal:
                    description += "Heals between " + and(eff.power) + " life points";
                    break;
                case EffectType.Stun:
                    description += "Stuns " + target(eff.target) + " for " + to(eff.power) + " turns";
                    break;
                case EffectType.Protection:
                    description += "Protects from direct damage for " + to(eff.power) + " turns";
                    break;
            }
            if (eff.duration > 1) {
                description += " for " + eff.duration + " turns";
            }
            description += ".\n";
        }
        return description;
    }

    static public string DiceTitle(Dice d) {
        return d.name + " (level " + d.level + ")";
    }

    public void SetDice(Dice d) {
        title.text = DiceTitle(d);
        description.text = DiceDescription(d, displayAllInfo);

        image.sprite = d.icon;
        ((RectTransform)image.transform).sizeDelta = new Vector2(80f, 80f);
    }
}

