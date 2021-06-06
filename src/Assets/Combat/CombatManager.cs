using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICombatObserver {
    void notifyNewTurn(int turnEntityId);
    void notifyLifeChanged(int entityId, int life, int lifeMax);
    void notifyDied(int entityId);
    void notifyStuned(int entityId, int remainingTurn);
    void notifyAttackBlocked(int entityId, int remainingTurn);
    void notifyTakeDamage(int entityId, int value);
    void notifyHealed(int entityId, int value);
    void notifyEffectsChanged(int entityId, List<EntityEffect> effects);
    void notifySlotChanged(int entityId, List<DiceSlot> slots);
    void notifyAction(int entityId, ActionTurn action);
    void notifyEffectRoll(DiceEffect effect, Vector2Int result);
    void notifyStateChanged(CombatState newState);
    void notifyCombatStart(List<int> enemiesId, bool boss);
}

public enum CombatState {
    NoCombat,
    OnGoing,
    GameOver,
    Win
}

public class CombatManager : MonoBehaviour, IEntityObserver {
    public const float EFFECT_ROLL_WAIT_TIME = 3f;
    public const float END_COMBAT_WAIT_TIME = 2.5f;
    public const float END_TURN_WAIT_TIME = 1.5f;
    public const float APPLIED_EFFECT_WAIT_TIME = 1f;
    public const float PROCESS_ACTION_WAIT_TIME = 2f;

    private const int playerId = 0;

    private List<Entity> entities = new List<Entity>();
    private Player player;
    private int entityTurn = playerId;

    CombatState state = CombatState.NoCombat;
    bool waiting = false;

    List<ICombatObserver> observers = new List<ICombatObserver>();

    [SerializeField]
    bool isBossCombat = false;

    private void Start() {
        startCombat(GameManager.instance.GetListOfEnemyDices(), GameManager.instance.GetPlayerInfo(), GameManager.instance.GetLevel());
    }


    protected int getNumEnemies(int level) {
        float f = (float)level / ((float)(GraphCreator.NUMBER_ROOMS - 1) * 0.5f) + 1f;

        int n = (int)f;
        float prob = f - (float)n;
        float r = Random.Range(0f, 1f);

        if (r < prob) {
            n++;
        }

        return n;
    }

    public void startCombat(List<Dice> dices, PlayerInfo playerInfo, int level) {
        state = CombatState.OnGoing;
        waiting = false;
        entityTurn = playerId;

        entities = new List<Entity>();
        List<int> entitiesId = new List<int>();

        // player = new Player("Player", 150, new List<DiceSlot> { new DiceSlot(dices[2]), new DiceSlot(dices[3]), new DiceSlot(dices[4]) });
        player = new Player("Player", playerInfo);
        entities.Add(player);

        for (int i = 0; i < (isBossCombat ? 3 : getNumEnemies(level)); i++) {
            if (isBossCombat && i == 1) {
                entities.Add(new Abbadon("Abaddon", 24, new List<DiceSlot> { new DiceSlot(dices[3]), new DiceSlot(dices[4]), new DiceSlot(dices[5]), new DiceSlot(dices[6]) }));
                entitiesId.Add(1);
            } else {
                int r = Random.Range(0, 2);

                if (r == 0 && !isBossCombat) {
                    entities.Add(new Enemy("Icepargus", 16, new List<DiceSlot> { new DiceSlot(dices[1]), new DiceSlot(dices[2]) }));
                    entitiesId.Add(2);
                } else {
                    entities.Add(new Enemy("Hound", 12, new List<DiceSlot> { new DiceSlot(dices[0]) }));
                    entitiesId.Add(0);
                }
            }
        }
        foreach (Entity entity in entities) {
            entity.addObserver(this);
        }

        foreach (ICombatObserver observer in observers) {

            observer.notifyCombatStart(entitiesId, isBossCombat);

        }
        for (int i = 0; i < entities.Count; i++) {
            foreach (ICombatObserver observer in observers) {
                observer.notifyLifeChanged(i, entities[i].getLife(), entities[i].getLifeMax());
                observer.notifyEffectsChanged(i, entities[i].getEffects());
                observer.notifySlotChanged(i, entities[i].getSlots());
            }
        }

        player.newTurn();
        foreach (ICombatObserver observer in observers) {
            observer.notifyNewTurn(entityTurn);
        }
    }

    bool waitAction = false;
    IEnumerator processAction(ActionTurn action) {
        waitAction = true;

        List<Entity> tempEntity = new List<Entity>(entities);

        foreach (DiceEffect effect in action.dice.effects) {
            Vector2Int result = effect.roll();
            EntityEffect entityEffect = new EntityEffect(effect, result);
            foreach (ICombatObserver observer in observers) {
                observer.notifyEffectRoll(effect, result);
            }

            if (effect.power.y == 0) {
                continue;
            }

            if (entityTurn == 0)
                yield return new WaitForSeconds(EFFECT_ROLL_WAIT_TIME + 0.125f);

            // if (entityTurn == playerId) {
            if (effect.target == EffectTarget.Player) {
                player.applyEffect(new EntityEffect(entityEffect));
            } else if (effect.target == EffectTarget.Enemy) {
                tempEntity[action.targetId].applyEffect(new EntityEffect(entityEffect));
            } else {
                for (int i = 1; i < tempEntity.Count; i++) {
                    tempEntity[i].applyEffect(new EntityEffect(entityEffect));
                }
            }
        }

        waitAction = false;

        yield return null;

    }

    void nextTurn() {
        if (state != CombatState.OnGoing) return;

        entityTurn = (entityTurn + 1) % entities.Count;

        if (state != CombatState.OnGoing) {
            return;
        }

        StartCoroutine(waitNextTurn());


    }

    void gameOver() {
        StopAllCoroutines();
        entities.Clear();
        state = CombatState.GameOver;
        StartCoroutine(waitNotifyEndCombat());
    }

    void playerWin() {
        StopAllCoroutines();
        entities.Clear();
        state = CombatState.Win;
        GameManager.instance.SetPlayerInfo(new PlayerInfo(player));
        StartCoroutine(waitNotifyEndCombat());
    }

    IEnumerator waitNotifyEndCombat() {
        yield return new WaitForSeconds(END_COMBAT_WAIT_TIME);
        foreach (ICombatObserver observer in observers) {
            observer.notifyStateChanged(state);
        }
    }

    void Update() {
        if (waiting || waitAction || state != CombatState.OnGoing) return;

        entities[entityTurn].action(entityTurn, entities);

/*        if (Input.GetKeyDown(KeyCode.K)) {
            for (int i = entities.Count - 1; i >= 1; i--) {
                entities[i].takeDamage(int.MaxValue);
            }
        }*/

    }

    IEnumerator waitNextTurn() {
        waiting = true;

        yield return new WaitForSeconds(END_TURN_WAIT_TIME);

        foreach (ICombatObserver observer in observers) {
            observer.notifyNewTurn(entityTurn);
        }

        yield return new WaitForSeconds(END_TURN_WAIT_TIME);


        if (entityTurn < entities.Count)
            entities[entityTurn].newTurn();

        if (entityTurn < entities.Count && entities[entityTurn].getEffects().Any(effect => effect.type == EffectType.Damage))
            yield return new WaitForSeconds(APPLIED_EFFECT_WAIT_TIME);

        waiting = false;

        yield return null;
    }

    public CombatState getState() {
        return state;
    }

    public PlayerInfo getPlayerInfo() {
        return new PlayerInfo(player);
    }

    public void addObserver(ICombatObserver observer) {
        observers.Add(observer);
    }
    public void removeObserver(ICombatObserver observer) {
        observers.Remove(observer);
    }

    void IEntityObserver.notifyEndTurn(Entity entity, ActionTurn action) {

        if (entities.IndexOf(entity) != entityTurn)
            return;

        StartCoroutine(waitEndTurn(entity, action));
        // processAction(action);
        // nextTurn();
        // foreach (ICombatObserver observer in observers) {
        //     observer.notifyNewTurn(entityTurn);
        // }
    }

    IEnumerator waitEndTurn(Entity entity, ActionTurn action) {
        waiting = true;
        if (action != null) {
            foreach (ICombatObserver observer in observers) {
                observer.notifyAction(entityTurn, action);
            }
            yield return new WaitForSeconds(PROCESS_ACTION_WAIT_TIME);

            StartCoroutine(processAction(action));

            yield return new WaitWhile(() => waitAction == true);
        }

        if (state == CombatState.OnGoing) {
            nextTurn();
        }
    }

    void IEntityObserver.notifyLifeChanged(Entity entity, int life, int lifeMax) {
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifyLifeChanged(entityId, life, lifeMax);
        }
    }
    void IEntityObserver.notifyDied(Entity entity) {
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifyDied(entityId);
        }

        if (entityId == playerId) {
            gameOver();
        } else {
            if (entityId <= entityTurn) {
                entityTurn = Mathf.Max(entityTurn - 1, 0) % entities.Count;
            }
            entities.Remove(entity);
            if (entities.Count <= 1) {
                playerWin();
            }
        }
    }
    void IEntityObserver.notifyStuned(Entity entity, int remainingTurn) {
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifyStuned(entityId, remainingTurn);
        }
    }
    void IEntityObserver.notifyAttackBlocked(Entity entity, int remainingTurn) {
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifyAttackBlocked(entityId, remainingTurn);
        }
    }
    void IEntityObserver.notifyTakeDamage(Entity entity, int value) {
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifyTakeDamage(entityId, value);
        }
    }
    void IEntityObserver.notifyHealed(Entity entity, int value) {
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifyHealed(entityId, value);
        }
    }
    void IEntityObserver.notifyEffectsChanged(Entity entity, List<EntityEffect> effects) {
        if (entity.isDead()) return;
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifyEffectsChanged(entityId, effects);
        }
    }
    void IEntityObserver.notifySlotChanged(Entity entity, List<DiceSlot> slots) {
        int entityId = entities.IndexOf(entity);
        foreach (ICombatObserver observer in observers) {
            observer.notifySlotChanged(entityId, slots);
        }
    }

    public Entity getEntity(int entityId) { return entities[entityId]; }
    public Player getPlayer() { return player; }
}
