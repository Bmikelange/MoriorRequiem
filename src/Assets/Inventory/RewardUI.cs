using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour {
    // Start is called before the first frame update
    RewardDisplayUI rewardDisplayPanel = null;
    EquipmentUI diceDiscardPanel = null;

    Dice rewardDice = null;

    private void Start() {
        rewardDisplayPanel = transform.GetComponentInChildren<RewardDisplayUI>();
        diceDiscardPanel = transform.GetComponentInChildren<EquipmentUI>();
        diceDiscardPanel.gameObject.SetActive(false);
        DiceLootTable lootTable = new DiceLootTable(GameManager.instance.GetListOfDices());
        rewardDice = lootTable.generateDice(GameManager.instance.GetLevel());
        rewardDisplayPanel.SetDice(rewardDice);
    }

    public void ClaimDice() {
        if (GameManager.instance.PlayerHasFreeSlot()) {
            GameManager.instance.ClaimDice(rewardDice);
            GameManager.instance.SetGameState(GameState.MAP);
        } else {
            rewardDisplayPanel.gameObject.SetActive(false);
            diceDiscardPanel.gameObject.SetActive(true);
            diceDiscardPanel.FillSlots(GameManager.instance.GetPlayerSlots());
            diceDiscardPanel.FillRewardDice(rewardDice);
        }
    }

    public void ReplaceWith(int index) {
        GameManager.instance.ClaimDice(rewardDice, index);
        GameManager.instance.SetGameState(GameState.MAP);
    }

    public void DiscardReward() {
        GameManager.instance.SetGameState(GameState.MAP);
    }


}
