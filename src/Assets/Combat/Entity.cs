using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityObserver {
    void notifyEndTurn(Entity entity, ActionTurn action);
    void notifyLifeChanged(Entity entity, int life, int lifeMax);
    void notifyDied(Entity entity);
    void notifyStuned(Entity entity, int remainingTurn);
    void notifyAttackBlocked(Entity entity, int remainingTurn);
    void notifyTakeDamage(Entity entity, int value);
    void notifyHealed(Entity entity, int value);
    void notifyEffectsChanged(Entity entity, List<EntityEffect> effects);
    void notifySlotChanged(Entity entity, List<DiceSlot> slots);
}

public abstract class Entity {
    protected string name;
    protected int lifeMax = 1;
    protected int life = 1;
    protected List<DiceSlot> slots = null;
    protected List<EntityEffect> effects = new List<EntityEffect>();
    protected bool turnEnded = true;

    protected List<IEntityObserver> observers = new List<IEntityObserver>();


    public Entity(string _name, int _life, List<DiceSlot> _slots) {
        name = _name;
        lifeMax = _life;
        life = lifeMax;
        slots = _slots;
    }

    protected void cleanEffects() {
        effects.RemoveAll(effect => effect.duration == 0);
        foreach (IEntityObserver obsever in observers) {
            obsever.notifyEffectsChanged(this, effects);
        }
    }

    protected void applyEffects() {
        foreach (EntityEffect effect in effects) {
            if (effect.duration <= 0) {
                continue;
            }

            if (effect.type == EffectType.Damage) {
                takeDamage(effect.value);
                if (isDead()) {
                    endTurn();
                    return;
                }
            } else if (effect.type == EffectType.Heal) {
                heal(effect.value);
            } else if (effect.type == EffectType.Stun && !turnEnded) {
                turnEnded = true;
                foreach (IEntityObserver obsever in observers) {
                    obsever.notifyStuned(this, effect.duration);
                }
            }

            if (effect.type != EffectType.Protection) {
                effect.duration--;
            }
        }
        cleanEffects();
    }
    public virtual void newTurn() {
        turnEnded = false;

        applyEffects();

        foreach (DiceSlot slot in slots) {
            if (slot.cooldown > 0)
                slot.cooldown--;
        }
        foreach (IEntityObserver obsever in observers) {
            obsever.notifySlotChanged(this, slots);
        }
    }

    public virtual void endTurn(ActionTurn action = null) {
        foreach (IEntityObserver obsever in observers) {
            obsever.notifyEndTurn(this, action);
        }
        turnEnded = true;
    }

    public bool canPlay() {
        bool canSpell = false;
        foreach (DiceSlot slot in slots) {
            if (slot.cooldown == 0) {
                canSpell = true;
                break;
            }
        }
        return !isDead() && slots.Count > 0 && canSpell && !turnEnded;
    }
    public abstract void action(int selfId, List<Entity> entities);

    public bool isTurnEnded() { return turnEnded; }

    public bool isDead() { return life <= 0; }

    public void takeDamage(int value) {
        bool applyDamage = true;
        foreach (EntityEffect effect in effects) {
            if (effect.duration > 0 && effect.type == EffectType.Protection) {
                effect.duration--;
                applyDamage = false;
                foreach (IEntityObserver obsever in observers) {
                    obsever.notifyAttackBlocked(this, effect.duration);
                }
                break;
            }
        }

        if (applyDamage) {
            life = Mathf.Clamp(life - value, 0, lifeMax);

            foreach (IEntityObserver obsever in observers) {
                obsever.notifyTakeDamage(this, value);
                obsever.notifyLifeChanged(this, life, lifeMax);
            }
            if (isDead()) {
                foreach (IEntityObserver obsever in observers) {
                    obsever.notifyDied(this);
                }
            }
        }
    }
    protected void heal(int value) {
        if (!isDead()) {
            life = Mathf.Clamp(life + value, 0, lifeMax);
            foreach (IEntityObserver obsever in observers) {
                obsever.notifyHealed(this, value);
                obsever.notifyLifeChanged(this, life, lifeMax);
            }
        }
    }

    public void applyEffect(EntityEffect effect) {
        if (isDead()) return;

        if (effect.duration > 1) {
            effects.Add(effect);
        } else {
            if (effect.type == EffectType.Damage) {
                takeDamage(effect.value);
            } else if (effect.type == EffectType.Heal) {
                heal(effect.value);
            }
        }
        cleanEffects();
    }

    public string getName() { return name; }

    public int getLife() { return life; }
    public int getLifeMax() { return lifeMax; }
    public List<DiceSlot> getSlots() { return slots; }

    public void addObserver(IEntityObserver observer) {
        observers.Add(observer);
    }
    public void removeObserver(IEntityObserver observer) {
        observers.Remove(observer);
    }

    public List<EntityEffect> getEffects() { return effects; }
}
