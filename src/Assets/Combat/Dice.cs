using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice {
    public string name;
    public int level;
    public List<DiceEffect> effects;
    public int cooldown;
    public Sprite icon;

    public Dice(string _name, int _level, List<DiceEffect> _effects, int _cooldown, Sprite _icon = null) {
        name = _name;
        level = _level;
        effects = _effects;
        cooldown = _cooldown;
        icon = _icon;
    }

    public bool isMonoTarget() {
        foreach (DiceEffect effect in effects) {
            if (effect.target == EffectTarget.Enemy) {
                return true;
            }
        }

        return false;
    }

    public Dice(Dice other) {
        name = other.name;
        level = other.level;
        effects = new List<DiceEffect>();
        foreach (DiceEffect e in other.effects) {
            effects.Add(new DiceEffect(e.type, e.duration, e.name, e.vfx, e.target, e.power));
        }
        cooldown = other.cooldown;
        icon = other.icon;
    }
}
