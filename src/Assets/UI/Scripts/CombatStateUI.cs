using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateUI : MonoBehaviour, ICombatObserver {
    [SerializeField]
    CombatManager combatManager;
    [SerializeField]
    RewardUI rewardUI;
    CombatUI combatUI;
    GameOverUI gameOverUI;
    CombatWinUI combatWinUI;
    RewardDisplayUI rewardDisplayUI;
    EquipmentUI equipmentUI;

    void Awake() {
        combatUI = GetComponentInChildren<CombatUI>();
        gameOverUI = GetComponentInChildren<GameOverUI>();
        combatWinUI = GetComponentInChildren<CombatWinUI>();
        rewardDisplayUI = GetComponentInChildren<RewardDisplayUI>();
        equipmentUI = GetComponentInChildren<EquipmentUI>();
        if (combatManager != null) {
            combatManager.addObserver(this);
        }
    }

    public void notifyNewTurn(int turnEntityId) { }
    public void notifyLifeChanged(int entityId, int life, int lifeMax) { }
    public void notifyDied(int entityId) { }
    public void notifyStuned(int entityId, int remainingTurn) { }
    public void notifyAttackBlocked(int entityId, int remainingTurn) { }
    public void notifyTakeDamage(int entityId, int value) { }
    public void notifyHealed(int entityId, int value) { }
    public void notifyEffectsChanged(int entityId, List<EntityEffect> effects) { }
    public void notifySlotChanged(int entityId, List<DiceSlot> slots) { }
    public void notifyAction(int entityId, ActionTurn action) { }
    public void notifyEffectRoll(DiceEffect effect, Vector2Int result) { }
    public void notifyStateChanged(CombatState newState) {
        if (newState == CombatState.GameOver) {
            combatUI.gameObject.SetActive(false);
            combatWinUI.gameObject.SetActive(false);
            gameOverUI.gameObject.SetActive(true);
        } else if (newState == CombatState.Win) {
            combatUI.gameObject.SetActive(false);
            gameOverUI.gameObject.SetActive(false);
            combatWinUI.gameObject.SetActive(true);
        }
    }
    public void notifyCombatStart(List<int> enemiesId, bool boss) {
        combatUI.gameObject.SetActive(true);
        combatWinUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
        if (rewardUI != null)
            rewardUI.gameObject.SetActive(false);
    }

    virtual public void ProceedToReward() {
        rewardUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
