using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatViewer : MonoBehaviour, ICombatObserver {
    [SerializeField]
    GameObject playerModel;

    [SerializeField]
    List<GameObject> enemyModels;

    [SerializeField]
    CombatManager combatManager;

    [SerializeField]
    GameObject damagePrefab;

    [SerializeField]
    GameObject healPrefab;

    [SerializeField]
    GameObject diceVFX;

    int entityTurn = 0;
    int targetId = 0;
    [SerializeField]
    GameObject projectileVFX;

    List<GameObject> entitiesToken = new List<GameObject>();

    //[SerializeField]
    new Camera camera;
    public CameraCombat camCombat;

    private void Awake() {
        camera = GameObject.FindObjectOfType<Camera>();
        combatManager.addObserver(this);
    }

    void ICombatObserver.notifyNewTurn(int turnEntityId) {
        entityTurn = turnEntityId;
        foreach (var e in entitiesToken) {
            MovingEntity move = e.GetComponent<MovingEntity>();
            if (move != null) move.setTimeSign(-1f);

            Animator anim = e.GetComponent<Animator>();
            if (anim) {
                anim.SetInteger("state", 0);
            }
        }
    }
    void ICombatObserver.notifyLifeChanged(int entityId, int life, int lifeMax) { }

    IEnumerator waitDieToken(GameObject token) {
        yield return new WaitForSeconds(0.5f);
        Animator anim = token.GetComponent<Animator>();
        if (anim) {
            anim.SetInteger("state", 2);
        }
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(token);
    }

    void ICombatObserver.notifyDied(int entityId) {
        EntitySound sound = entitiesToken[entityId].GetComponent<EntitySound>();
        if (sound != null) {
            sound.PlayDeath();
        }
        StartCoroutine(waitDieToken(entitiesToken[entityId]));
        entitiesToken.Remove(entitiesToken[entityId]);
    }
    void ICombatObserver.notifyStuned(int entityId, int remainingTurn) { }
    void ICombatObserver.notifyAttackBlocked(int entityId, int remainingTurn) {
        TokenVFX tokenvfx = entitiesToken[entityId].GetComponent<TokenVFX>();
        if (tokenvfx != null) {
            tokenvfx.attackBlocked();
        }
    }
    void ICombatObserver.notifyTakeDamage(int entityId, int value) {
        EntitySound sound = entitiesToken[entityId].GetComponent<EntitySound>();
        if (sound != null) {
            sound.PlayDamage();
        }
        ShakingObject shake = entitiesToken[entityId].GetComponent<ShakingObject>();
        shake.setShaking(1.0f);
        if (value == 0) return;
        GameObject g = Instantiate(damagePrefab, entitiesToken[entityId].transform.position + Vector3.up * 2, camera.transform.rotation);
        g.GetComponentInChildren<TMPro.TMP_Text>().text = "" + value;
        if (entityId == 0) {
            g.GetComponentInChildren<TMPro.TMP_Text>().fontSize /= 2;
        }
    }
    void ICombatObserver.notifyHealed(int entityId, int value) {
        GameObject g = Instantiate(healPrefab, entitiesToken[entityId].transform.position + Vector3.up * 2, camera.transform.rotation);
        g.GetComponentInChildren<TMPro.TMP_Text>().text = "" + value;
        if (entityId == 0) {
            g.GetComponentInChildren<TMPro.TMP_Text>().fontSize /= 2;
        }
    }

    void ICombatObserver.notifyEffectsChanged(int entityId, List<EntityEffect> effects) {
        TokenVFX tokenvfx = entitiesToken[entityId].GetComponent<TokenVFX>();
        if (tokenvfx != null) {
            tokenvfx.processEffects(effects);
        }
    }

    void ICombatObserver.notifySlotChanged(int entityId, List<DiceSlot> slots) { }

    IEnumerator waitAttackAnim(GameObject obj) {
        yield return new WaitForSeconds(1f);
        EntitySound sound = obj.GetComponent<EntitySound>();
        if (sound != null) {
            sound.PlayAttack();
        }
        Animator anim = obj.GetComponent<Animator>();
        if (anim) {
            anim.SetInteger("state", 1);
        }
    }

    ActionTurn lastAction = null;
    void ICombatObserver.notifyAction(int entityId, ActionTurn action) {
        if (action == null) return;
        lastAction = action;
        MovingEntity move = entitiesToken[entityId].GetComponent<MovingEntity>();
        if (move != null) move.setTimeSign(1f);

        targetId = action.targetId;

        StartCoroutine(waitAttackAnim(entitiesToken[entityId]));
    }

    void spawnProjectileVFX(int srcId, int dstId, EffectVFX vfx) {
        Vector3 srcPosition = entitiesToken[srcId].transform.position + Vector3.up * 2f * getTokenScale(srcId);
        GameObject gO = Instantiate(projectileVFX, srcPosition, transform.rotation);
        ProjectileVFX projVFX = gO.GetComponent<ProjectileVFX>();
        if (projVFX != null) {
            projVFX.Init(
                srcPosition,
                entitiesToken[dstId].transform.position + Vector3.up * getTokenScale(dstId),
                vfx.vfxObject, 0.375f, vfx.projInverse
            );
        }

    }

    float getTokenScale(int tokenId) {
        float scale = 1f;
        TokenVFX tokenVFX = entitiesToken[tokenId].GetComponent<TokenVFX>();
        if (tokenVFX != null) {
            scale = tokenVFX.vfxScale;
        }
        return scale;
    }

    void spawnRollVFX(DiceEffect effect, Vector2Int result) {
        EffectVFX vfx = effect.vfx;
        if (vfx != null) {
            if (effect.isInstant() || vfx.projectile) {
                if (vfx.projectile) {
                    if (effect.target == EffectTarget.Enemy) {
                        spawnProjectileVFX(entityTurn, targetId, vfx);
                    } else if (effect.target == EffectTarget.Enemies) {
                        for (int i = 1; i < entitiesToken.Count; i++) {
                            spawnProjectileVFX(entityTurn, i, vfx);
                        }
                    } else {
                        spawnProjectileVFX(targetId, 0, vfx);
                    }
                } else {
                    if (effect.target == EffectTarget.Enemy) {
                        GameObject go = Instantiate(vfx.vfxObject, entitiesToken[targetId].transform.position, entitiesToken[targetId].transform.rotation);
                        go.transform.localScale = Vector3.one * getTokenScale(targetId);
                    } else if (effect.target == EffectTarget.Enemies) {
                        for (int i = 1; i < entitiesToken.Count; i++) {
                            GameObject go = Instantiate(vfx.vfxObject, entitiesToken[i].transform.position, entitiesToken[i].transform.rotation);
                            go.transform.localScale = Vector3.one * getTokenScale(i);
                        }
                    } else {
                        GameObject go = Instantiate(vfx.vfxObject, entitiesToken[0].transform.position, entitiesToken[0].transform.rotation);
                        go.transform.localScale = Vector3.one * getTokenScale(0);
                    }
                }
            }
        }
    }

    IEnumerator effectRollCoroutine(DiceEffect effect, Vector2Int result) {
        if (entityTurn == 0 && effect.power.y > 0) {
            GameObject obj = Instantiate(diceVFX, entitiesToken[entityTurn].transform.position, entitiesToken[entityTurn].transform.rotation);
            obj.GetComponent<DiceVFX>().Init(entitiesToken[entityTurn].transform.forward, effect.power, result, lastAction.dice.icon);
            yield return new WaitForSeconds(CombatManager.EFFECT_ROLL_WAIT_TIME);
        }


        spawnRollVFX(effect, result);
    }
    void ICombatObserver.notifyEffectRoll(DiceEffect effect, Vector2Int result) {
        StartCoroutine(effectRollCoroutine(effect, result));
    }

    void ICombatObserver.notifyStateChanged(CombatState newState) { }

    static Vector3 correctYPosition(Vector3 pos) {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(pos.x, 5, pos.z), Vector3.down, out hit, Mathf.Infinity)) {
            return hit.point;
        }

        return pos;
    }

    void ICombatObserver.notifyCombatStart(List<int> enemiesId, bool boss) {
        entityTurn = 0;
        int entityNumber = enemiesId.Count + 1;
        foreach (var token in entitiesToken) { GameObject.Destroy(token); }
        entitiesToken.Clear();

        Vector3 playerPos = new Vector3(0, 0, 5);
        playerPos = correctYPosition(playerPos);
        entitiesToken.Add(Instantiate<GameObject>(playerModel, playerPos, Quaternion.AngleAxis(180.0f, Vector3.up), transform));

        for (int i = 1; i < entityNumber; i++) {
            int id = entityNumber - i;
            Vector3 position = new Vector3((id - 1) * 4f - ((entityNumber - 2) * 2f), 0, -2.5f);

            Vector3 dir = (playerPos - position).normalized;
            Quaternion rot = Quaternion.LookRotation(dir);


            float dist = 8f;
            if (boss && i == 2) {
                dist = 14f;
            }

            position = playerPos - dir * dist;

            if (!(boss && i == 2)) {
                position = correctYPosition(position);
            }

            entitiesToken.Add(Instantiate<GameObject>(enemyModels[enemiesId[i - 1]], position, rot, transform));
        }

        if (camCombat != null) {
            camCombat.createTokensPOV(this);
        }
    }

}
