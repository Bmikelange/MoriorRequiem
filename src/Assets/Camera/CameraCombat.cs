using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCombat : MonoBehaviour, ICombatObserver {

    public CameraTween targetCamera;

    public CombatManager combatManager;

    List<GameObject> enemiesPos = new List<GameObject>();
    GameObject defaultPosition;
    GameObject enemyGroupPosition;

    int entityTurn = 0;
    int targetId = 0;

    private void Awake() {
        combatManager.addObserver(this);
        defaultPosition = transform.GetChild(0).gameObject;
        enemyGroupPosition = transform.GetChild(1).gameObject;
    }

    void setCameraTransform(GameObject obj) {
        targetCamera.setDestination(obj.transform, 1f);
    }

    public void createTokensPOV(CombatViewer combatViewer) {
        foreach (Transform child in combatViewer.transform) {
            Vector3 offset = Vector3.up - child.transform.right + child.transform.forward * 3.25f;

            float tokenScale = 1f;
            if (child.GetComponent<TokenVFX>() != null) {
                tokenScale = child.GetComponent<TokenVFX>().vfxScale;
            }

            Vector3 pos = child.transform.position + offset * tokenScale;
            Quaternion rot = Quaternion.LookRotation(child.transform.position + Vector3.up * tokenScale - pos);
            enemiesPos.Add(Instantiate(new GameObject(), pos, rot, transform));
        }
    }

    IEnumerator focusEnemies(int turnEntityId, float time) {
        if (turnEntityId == 0) {
            setCameraTransform(defaultPosition);
        } else {
            setCameraTransform(enemiesPos[turnEntityId]);
            yield return new WaitForSeconds(time);
            setCameraTransform(defaultPosition);
        }

        yield return null;
    }

    void ICombatObserver.notifyNewTurn(int turnEntityId) {
        entityTurn = turnEntityId;

        StartCoroutine(focusEnemies(turnEntityId, 1.5f));

    }
    void ICombatObserver.notifyLifeChanged(int entityId, int life, int lifeMax) { }
    void ICombatObserver.notifyDied(int entityId) {
        //GameObject.Destroy(enemiesPos[entityId]);
        enemiesPos.Remove(enemiesPos[entityId]);
    }
    void ICombatObserver.notifyStuned(int entityId, int remainingTurn) { }
    void ICombatObserver.notifyAttackBlocked(int entityId, int remainingTurn) { }
    void ICombatObserver.notifyTakeDamage(int entityId, int value) { }
    void ICombatObserver.notifyHealed(int entityId, int value) { }
    void ICombatObserver.notifyEffectsChanged(int entityId, List<EntityEffect> effects) { }
    void ICombatObserver.notifySlotChanged(int entityId, List<DiceSlot> slots) { }
    void ICombatObserver.notifyAction(int entityId, ActionTurn action) { targetId = action.targetId; }

    IEnumerator effectRollCoroutine(DiceEffect effect, Vector2Int result) {
        if (entityTurn == 0) {
            yield return new WaitForSeconds(CombatManager.EFFECT_ROLL_WAIT_TIME - 0.5f);

            if (effect.target == EffectTarget.Player) {
                setCameraTransform(defaultPosition);
            } else {
                setCameraTransform(enemyGroupPosition);
                yield return new WaitForSeconds(1.5f);
                setCameraTransform(defaultPosition);
            }
        }

        yield return null;
    }

    void ICombatObserver.notifyEffectRoll(DiceEffect effect, Vector2Int result) {
        if (effect.power.y == 0) return;

        StartCoroutine(effectRollCoroutine(effect, result));

    }

    void ICombatObserver.notifyStateChanged(CombatState newState) { }
    void ICombatObserver.notifyCombatStart(List<int> enemiesId, bool boss) {
        entityTurn = 0;
    }
}
