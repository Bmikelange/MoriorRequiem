using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {
    public int life, lifeMax;
    public List<DiceSlot> slots;

    public PlayerInfo(int _lifeMax, int _life, List<DiceSlot> _slots) {
        life = _life;
        lifeMax = _lifeMax;
        slots = _slots;
    }

    public PlayerInfo(Player player) {
        life = player.getLife();
        lifeMax = player.getLifeMax();
        slots = player.getSlots();
        foreach (DiceSlot slot in slots) {
            slot.cooldown = 0;
        }
    }

    public PlayerInfo(PlayerInfo other) {
        life = other.life;
        lifeMax = other.lifeMax;
        slots = new List<DiceSlot>(other.slots);
    }
}
public class Player : Entity {

    int choosenDiceId = -1;
    int choosenTarget = -1;

    public Player(string _name, int _life, List<DiceSlot> _slots) : base(_name, _life, _slots) { }

    public Player(string _name, PlayerInfo playerInfo) : base(_name, playerInfo.lifeMax, playerInfo.slots) { life = playerInfo.life; }

    public override void endTurn(ActionTurn action = null) {
        base.endTurn(action);
        choosenDiceId = -1;
        choosenTarget = -1;
    }

    public override void action(int selfId, List<Entity> entities) {
        if (!canPlay()) {
            endTurn();
        }

        if (choosenDiceId < 0) {
            for (int i = 0; i < slots.Count; i++) {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
                    if (slots[i].cooldown == 0) {
                        Dice choosenDice = slots[i].dice;
                        choosenDiceId = i;
                        if (!choosenDice.isMonoTarget()) {
                            slots[choosenDiceId].applyCooldown();
                            foreach (IEntityObserver obsever in observers) {
                                obsever.notifySlotChanged(this, slots);
                            }
                            endTurn(new ActionTurn(choosenDice, choosenTarget));
                        }
                    }
                }
            }
        } else if (choosenTarget < 0 && slots[choosenDiceId].dice.isMonoTarget()) {
            for (int i = 1; i < entities.Count; i++) {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
                    choosenTarget = i;
                    slots[choosenDiceId].applyCooldown();
                    foreach (IEntityObserver obsever in observers) {
                        obsever.notifySlotChanged(this, slots);
                    }
                    endTurn(new ActionTurn(slots[choosenDiceId].dice, choosenTarget));
                }
            }
        } else {
            slots[choosenDiceId].applyCooldown();
            foreach (IEntityObserver obsever in observers) {
                obsever.notifySlotChanged(this, slots);
            }
            endTurn(new ActionTurn(slots[choosenDiceId].dice, choosenTarget));
        }

    }

    public void setChoosenDiceId(int _newChoosenId) {
        choosenDiceId = _newChoosenId;
    }

    public void setChoosenTargetId(int _choosenTarget) {
        choosenTarget = _choosenTarget;
    }
}
