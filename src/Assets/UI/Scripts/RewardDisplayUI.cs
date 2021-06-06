using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardDisplayUI : MonoBehaviour {
    SlotUI slot = null;
    // GameManager1 gameManager = null;
    Button button = null;
    RewardUI rewardUI;
    // Start is called before the first frame update
    void Awake() {
        slot = GetComponentInChildren<SlotUI>();
        button = GetComponentInChildren<Button>();
        rewardUI = GetComponentInParent<RewardUI>();
        button.onClick.AddListener(rewardUI.ClaimDice);
    }

    public void SetDice(Dice dice) {
        slot.SetDice(dice);
    }
}
