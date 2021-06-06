using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Damage,
    Heal,
    Protection,
    Stun
}

public class EffectVFX {
    public GameObject vfxObject = null;
    public bool projectile = false;
    public bool projInverse = false;

    public EffectVFX(GameObject _vfxObject, bool _projectile, bool _projInverse) {
        vfxObject = _vfxObject;
        projectile = _projectile;
        projInverse = _projInverse;
    }

    public EffectVFX() {

    }
}

public abstract class Effect {
    public EffectType type;
    public int duration;
    public EffectVFX vfx;
}

public class EntityEffect : Effect {
    public int value;
    public EntityEffect(DiceEffect diceEffect, Vector2Int _values) {
        type = diceEffect.type;
        vfx = diceEffect.vfx;

        if (type == EffectType.Damage || type == EffectType.Heal) {
            duration = diceEffect.duration;
            value = _values.x + _values.y;
        } else {
            duration = _values.x + _values.y;
        }

    }
    public EntityEffect(EntityEffect entityEffect) {
        type = entityEffect.type;
        duration = entityEffect.duration;
        value = entityEffect.value;
        vfx = entityEffect.vfx;

    }
}

public enum EffectTarget {
    Player,
    Enemy,
    Enemies
}

public class DiceEffect : Effect {
    public string name;
    public EffectTarget target;
    public Vector2Int power;

    public DiceEffect(EffectType _type, int _duration, string _name, EffectVFX _vfx, EffectTarget _target, Vector2Int _power) {
        type = _type;
        duration = _duration;
        name = _name;
        vfx = _vfx;
        target = _target;
        power = _power;
    }

    public Vector2Int roll() {
        return new Vector2Int(Random.Range(power.x, power.y + 1), Random.Range(power.x, power.y + 1));
    }

    public bool isInstant() {
        return duration == 1 && (type == EffectType.Damage || type == EffectType.Heal);
    }
}
