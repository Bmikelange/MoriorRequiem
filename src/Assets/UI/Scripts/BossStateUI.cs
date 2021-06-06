using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateUI : CombatStateUI {
    override public void ProceedToReward() {
        GameManager.instance.SetGameState(GameState.WIN);
    }
}
