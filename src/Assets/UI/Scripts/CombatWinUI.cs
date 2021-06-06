using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatWinUI : MonoBehaviour {
    CombatStateUI combatStateUI;
    Button button;
    void Start() {
        combatStateUI = GetComponentInParent<CombatStateUI>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(combatStateUI.ProceedToReward);
    }

}
