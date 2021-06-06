using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentUI : MonoBehaviour {
    RectTransform dices;
    RewardUI rewardUI;
    List<SlotUI> slotsUI = null;
    SlotUI rewardDice = null;
    Modal modal;
    List<DiceSlot> mySlots;
    Dice dice;
    void Awake() {
        modal = transform.parent.GetComponentInChildren<Modal>();
        rewardUI = GetComponentInParent<RewardUI>();
        dices = transform.GetChild(1).GetComponent<RectTransform>();
        slotsUI = new List<SlotUI>(transform.GetChild(1).GetChild(0).GetComponentsInChildren<SlotUI>());
        // Debug.Log(slotsUI.Count);
        rewardDice = transform.GetChild(1).GetChild(1).GetComponentInChildren<SlotUI>();
        modal.gameObject.SetActive(false);
        // rewardDice.GetComponent<Button>().onClick.AddListener(gameManager.exitToMap);
    }

    public void FillSlots(List<DiceSlot> slots) {
        mySlots = slots;
        for (int i = 0; i < slotsUI.Count; ++i) {
            slotsUI[i].SetDice(slots[i].dice);
            int n = i;
            Dice d = new Dice(mySlots[i].dice);
            slotsUI[i].GetComponent<Button>().onClick.AddListener(delegate {
                string title = "Discard " + SlotUI.DiceTitle(d) + " ?";
                string description = SlotUI.DiceDescription(d);
                modal.ActivateModal(title, description, delegate { rewardUI.ReplaceWith(n); 
                }); 
            });
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(dices);
    }

    public void FillRewardDice(Dice slot) {
        dice = slot;
        Dice d = new Dice(dice);
        rewardDice.SetDice(dice);
        rewardDice.GetComponent<Button>().onClick.AddListener(delegate {
            string title = "Discard " + SlotUI.DiceTitle(d) + " ?";
            modal.ActivateModal(title, SlotUI.DiceDescription(d), rewardUI.DiscardReward);
        });
        LayoutRebuilder.ForceRebuildLayoutImmediate(dices);
    }

}
