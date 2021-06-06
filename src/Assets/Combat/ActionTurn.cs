using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTurn {
    public Dice dice;
    public int targetId;

    public ActionTurn(Dice _dice, int _targetId) {
        dice = _dice;
        targetId = _targetId;
    }
}
