using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLoader : MonoBehaviour {

    [SerializeField]
    public List<GameObject> vfxGameObjects = new List<GameObject>();

    [SerializeField]
    public List<Sprite> diceIcons = new List<Sprite>();

    public List<Dice> getListOfDices() {
        return new List<Dice> {
            new Dice("Flame", 1, new List<DiceEffect>{ // 0
                new DiceEffect(EffectType.Damage, 4, "Fire", new EffectVFX(vfxGameObjects[3], false, false), EffectTarget.Enemies, new Vector2Int(1,3))
            }, 1, diceIcons[4]),
            new Dice("Molten Impact", 1, new List<DiceEffect>{ // 1
                new DiceEffect(EffectType.Damage, 1, "Impact", new EffectVFX(vfxGameObjects[4], true, false), EffectTarget.Enemy, new Vector2Int(1,3)),
                new DiceEffect(EffectType.Damage, 2, "Fire", new EffectVFX(vfxGameObjects[3], false, false), EffectTarget.Enemies, new Vector2Int(1,2))
            }, 1, diceIcons[5]),
            new Dice("Boulder", 1, new List<DiceEffect>{ // 2
                new DiceEffect(EffectType.Damage, 1, "Impact", new EffectVFX(vfxGameObjects[8], false, false), EffectTarget.Enemy, new Vector2Int(2,4))
            }, 1, diceIcons[6]),
            new Dice("Earth Shield", 1, new List<DiceEffect>{ // 3
                new DiceEffect(EffectType.Protection, 1, "Protection", new EffectVFX(vfxGameObjects[9], false, false), EffectTarget.Player, new Vector2Int(2,3))
            }, 5, diceIcons[7]),
            new Dice("Cure Wounds", 1, new List<DiceEffect>{ // 4
                new DiceEffect(EffectType.Heal, 1, "Heal", new EffectVFX(vfxGameObjects[0], false, false), EffectTarget.Player, new Vector2Int(3,5))
            }, 2, diceIcons[8]),
            new Dice("Spikes", 1, new List<DiceEffect>{ // 5
                new DiceEffect(EffectType.Stun, 1, "Snare", new EffectVFX(vfxGameObjects[10], false, false), EffectTarget.Enemy, new Vector2Int(1,2))
            }, 5, diceIcons[9]),
            new Dice("Bandaid", 1, new List<DiceEffect>{  // 6
                new DiceEffect(EffectType.Heal, 3, "Heal", new EffectVFX(vfxGameObjects[5], false, false), EffectTarget.Player, new Vector2Int(2,3))
            }, 3, diceIcons[10]),
            new Dice("Sacrifice", 1, new List<DiceEffect>{ // 7
                new DiceEffect(EffectType.Damage, 1, "Sacrifice", new EffectVFX(vfxGameObjects[1], false, false), EffectTarget.Player, new Vector2Int(2,3)),
                new DiceEffect(EffectType.Damage, 2, "Fire", new EffectVFX(vfxGameObjects[3], false, false), EffectTarget.Enemies, new Vector2Int(2,4)),
                new DiceEffect(EffectType.Damage, 1, "Fire", new EffectVFX(vfxGameObjects[2], true, false), EffectTarget.Enemies, new Vector2Int(0,0))
            }, 2, diceIcons[11]),
            new Dice("Spark", 1, new List<DiceEffect>{ // 8
                new DiceEffect(EffectType.Damage, 1, "Thunder", new EffectVFX(vfxGameObjects[11], false, false), EffectTarget.Enemies, new Vector2Int(1,3))
            }, 2, diceIcons[12]),
            new Dice("Drain Touch", 1, new List<DiceEffect>{ // 9
                new DiceEffect(EffectType.Damage, 1, "Impact", new EffectVFX(vfxGameObjects[2], true, true), EffectTarget.Enemy, new Vector2Int(1,3)),
                new DiceEffect(EffectType.Heal, 1, "Heal", new EffectVFX(vfxGameObjects[1], false, false), EffectTarget.Player, new Vector2Int(1,3))
            }, 3, diceIcons[13]),
        };
    }

    public List<Dice> getListOfEnemyDices() {
        return new List<Dice> {
            new Dice("Crunch", 1, new List<DiceEffect>{ // 0
                new DiceEffect(EffectType.Damage, 1, "Crunch", new EffectVFX(vfxGameObjects[6], false, false), EffectTarget.Player, new Vector2Int(1,3))
            }, 1),

            new Dice("IceBall", 1, new List<DiceEffect>{ // 1
                new DiceEffect(EffectType.Damage, 1, "Impact", new EffectVFX(vfxGameObjects[12], true, false), EffectTarget.Player, new Vector2Int(1,2))
            }, 1),

            new Dice("Bandaid", 1, new List<DiceEffect>{  // 2
                new DiceEffect(EffectType.Heal, 3, "Heal", new EffectVFX(vfxGameObjects[5], false, false), EffectTarget.Enemies, new Vector2Int(0,2))
            }, 3),

            // Boss dices

            new Dice("Loki's Horn", 1, new List<DiceEffect>{ // 3
                new DiceEffect(EffectType.Damage, 1, "Impact", new EffectVFX(vfxGameObjects[13], true, false), EffectTarget.Player, new Vector2Int(2,5))
            }, 1),

            new Dice("Rest", 1, new List<DiceEffect>{ // 4
                new DiceEffect(EffectType.Heal, 1, "Heal", new EffectVFX(vfxGameObjects[0], false, false), EffectTarget.Enemies, new Vector2Int(1,3))
                }, 2),

            new Dice("Burn Them All", 1, new List<DiceEffect>{ // 5 
                new DiceEffect(EffectType.Damage, 3, "Fire", new EffectVFX(vfxGameObjects[3], false, false), EffectTarget.Player, new Vector2Int(1,4))
                }, 2),

            new Dice("True Punch", 1, new List<DiceEffect>{ // 6
                new DiceEffect(EffectType.Damage, 1, "Impact", new EffectVFX(vfxGameObjects[14], false, false), EffectTarget.Player, new Vector2Int(4,6))
                }, 3)
        };
    }

    public List<float> getProbabilities() {
        return new List<float> {
            0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f
        };
    }
}
