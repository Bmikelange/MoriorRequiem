using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLootTable {
    List<Dice> referenceDices;
    List<float> cumulatedProbabilities;

    public DiceLootTable(List<Dice> dices, List<float> probabilites = null) {
        referenceDices = dices;
        cumulatedProbabilities = new List<float>();
        float sum = 0f;
        if (probabilites == null) {
            for (int i = 0; i < dices.Count; ++i) {
                cumulatedProbabilities.Add((float)i / (float)dices.Count);
            }
        } else {
            foreach (float f in probabilites) {
                sum += f;
                cumulatedProbabilities.Add(sum);
            }
        }
        cumulatedProbabilities[cumulatedProbabilities.Count - 1] = 1f;
    }

    static float effectFormula(int level) {
        return 0.75f * Mathf.Log10((float)level + 1f) + 1f;
    }

    private int GetRandomDiceIndex() {
        float rnd = Random.Range(0f, 1f);
        int index = 0;
        while (rnd > cumulatedProbabilities[index]) {
            index++;
        }
        return index;
    }

    static public Dice BoostDice(Dice copy, int level) {
        Dice d = new Dice(copy);
        d.level = level;
        float coeff = effectFormula(level);
        // Debug.Log(coeff);
        foreach (var effect in d.effects) {
            // Debug.Log("Effect " + effect.name + " power : " + effect.power);
            Vector2 res = new Vector2(effect.power.x, effect.power.y) * coeff;
            effect.power = new Vector2Int((int)res.x, (int)res.y);
            // Debug.Log("New power : " + effect.power);
        }
        return d;
    }

    public Dice generateDice(int level) {
        int index = GetRandomDiceIndex();
        Dice d = BoostDice(referenceDices[index], level);
        return d;
    }
}
