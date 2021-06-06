using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour, ICombatObserver {
    [SerializeField]
    CombatManager combatManager = null;

    [SerializeField]
    GameObject mainInfo;

    GameObject playerInfoPanel = null;
    EntityInfo playerInfo;

    GameObject enemiesInfoPanel = null;

    List<EntityInfo> enemiesInfo;

    List<Button> choiceButtons;

    GameObject diceSlots;

    Transform choicePanel;

    int currentEntityPlaying = 0;

    bool hasPlayedTurn = false;

    [SerializeField]
    GameObject rollInfoPrefab;

    [SerializeField]
    AudioSource attackBlocked;

    private void Awake() {
        if (combatManager != null) {
            combatManager.addObserver(this);
        }
        playerInfoPanel = transform.GetChild(0).gameObject;
        playerInfo = playerInfoPanel.GetComponentInChildren<EntityInfo>();
        enemiesInfoPanel = transform.GetChild(1).gameObject;
        diceSlots = transform.GetChild(2).gameObject;
        choicePanel = transform.GetChild(3);
        choiceButtons = new List<Button>(choicePanel.GetComponentsInChildren<Button>());
        enemiesInfo = new List<EntityInfo>(enemiesInfoPanel.GetComponentsInChildren<EntityInfo>());
        attackBlocked = GetComponent<AudioSource>();
    }

    void ICombatObserver.notifyNewTurn(int turnEntityId) {
        if (combatManager.getState() != CombatState.OnGoing) {
            return;
        }
        currentEntityPlaying = turnEntityId;
        /* SetMainMessage("Turn of " + combatManager.getEntity(turnEntityId).getName());*/
        GameObject g = GameObject.Instantiate(mainInfo, transform, false);
        TMP_Text txt = g.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = "Turn of " + combatManager.getEntity(turnEntityId).getName();
        RectTransform rect = ((RectTransform)g.transform);
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -10f);
        if (turnEntityId != 0) {
            diceSlots.gameObject.SetActive(false);
        } else
        {
            hasPlayedTurn = false;
        }
    }

    void ICombatObserver.notifyLifeChanged(int entityId, int life, int lifeMax) {
        if (combatManager.getState() != CombatState.OnGoing) {
            return;
        }
        if (entityId == 0) {
            playerInfo.ChangeLife(life, lifeMax);
        } else if (enemiesInfo.Count > entityId - 1) {
            enemiesInfo[entityId - 1].ChangeLife(life, lifeMax);
        }
    }

    void ICombatObserver.notifyDied(int entityId) {
        if (entityId == 0) {
            return;
        }
        enemiesInfo[entityId - 1].gameObject.SetActive(false);
        enemiesInfo.RemoveAt(entityId - 1);
        choiceButtons[entityId - 1].gameObject.SetActive(false);
        choiceButtons.RemoveAt(entityId - 1);
    }

    void ICombatObserver.notifyEffectsChanged(int entityId, List<EntityEffect> effects) {
        if (entityId == 0) {
            playerInfo.showEffects(effects);
        } else {
            enemiesInfo[entityId - 1].showEffects(effects);
        }
    }

    void openPrompt() {
        diceSlots.gameObject.SetActive(false);
        choicePanel.gameObject.SetActive(true);
    }

    void clearPrompt() {
        diceSlots.gameObject.SetActive(false);
        choicePanel.gameObject.SetActive(false);
    }

    void cancel() {
        diceSlots.gameObject.SetActive(true);
        choicePanel.gameObject.SetActive(false);
    }

    void createPrompt() {
        openPrompt();
        for (int i = 0; i < choiceButtons.Count - 1; ++i) {
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = combatManager.getEntity(i + 1).getName();
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.RemoveAllListeners();
            int n = i + 1;
            choiceButtons[i].onClick.AddListener(delegate {
                hasPlayedTurn = true;
                clearPrompt();
                combatManager.getPlayer().setChoosenTargetId(n);
            });
        }
        choiceButtons[choiceButtons.Count - 1].onClick.RemoveAllListeners();
        choiceButtons[choiceButtons.Count - 1].onClick.AddListener(cancel);
    }

    void ICombatObserver.notifySlotChanged(int entityId, List<DiceSlot> slots) {
        if (entityId == 0) {
            if(!hasPlayedTurn)
            {
                diceSlots.gameObject.SetActive(true);
            }
            int i = 0;
            for (; i < slots.Count; ++i) {
                diceSlots.transform.GetChild(i).gameObject.SetActive(true);
                Button b = diceSlots.transform.GetChild(i).GetComponent<Button>();
                Image im = diceSlots.transform.GetChild(i).GetComponent<Image>();
                im.preserveAspect = true;
                TMP_Text t = b.GetComponentInChildren<TMP_Text>();
                b.interactable = slots[i].cooldown == 0;

                ColorBlock block = b.colors;
                block.disabledColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
                b.colors = block;
                im.sprite = slots[i].dice.icon;
                t.text = "";//slots[i].dice.name;
                // im.sprite = gameManager.GetSpriteFromIcon(slots[i].dice.icon);
                if (!b.interactable) {
                    t.text +=  slots[i].cooldown + " turn" + (slots[i].cooldown > 1 ? "s" : "") + " left";
                }
                int number = i;
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(delegate {
                    if (currentEntityPlaying == 0) {
                        combatManager.getPlayer().setChoosenDiceId(number);
                        if (slots[number].dice.isMonoTarget())
                            createPrompt();
                        else
                        {
                            hasPlayedTurn = true;
                            clearPrompt();
                        }
                    }
                });
            }
            for (; i < 3; ++i) {
                diceSlots.transform.GetChild(i).gameObject.SetActive(false);
            }

        }
    }
    void ICombatObserver.notifyAction(int entityId, ActionTurn action) {
        GameObject g = GameObject.Instantiate(rollInfoPrefab, transform, false);
        TMP_Text text = g.GetComponentInChildren<TextMeshProUGUI>();
        text.text = action.dice.name;
        text.color = new Color(1f, 155f / 255f, 0f);
        RectTransform rect = ((RectTransform)g.transform);
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -60f);
    }

    void ICombatObserver.notifyEffectRoll(DiceEffect effect, Vector2Int result) {
        if (currentEntityPlaying != 0) return;
        if (effect.power.y == 0) return;

        GameObject g = GameObject.Instantiate(rollInfoPrefab, transform, false);
        TMP_Text txt = g.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = effect.name;
        RectTransform rect = ((RectTransform)g.transform);
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -60f);
    }

    void ICombatObserver.notifyCombatStart(List<int> enemiesId, bool boss) {
        playerInfo.SetName(combatManager.getEntity(0).getName());
        int i = 0;
        for (; i < enemiesId.Count; ++i) {
            enemiesInfo[i].gameObject.SetActive(true);
            enemiesInfo[i].SetName(combatManager.getEntity(i + 1).getName());
            choiceButtons[i].gameObject.SetActive(true);
        }
        for (int j = i; j < 3; ++j) {
            enemiesInfo[j].gameObject.SetActive(false);
            choiceButtons[j].gameObject.SetActive(false);
        }
        enemiesInfo.RemoveRange(i, 3 - i);
        if (3 - i > 0)
            choiceButtons.RemoveRange(i, 3 - i);
        choicePanel.gameObject.SetActive(false);
    }
    void ICombatObserver.notifyStateChanged(CombatState newState) { }
    void ICombatObserver.notifyStuned(int entityId, int remainingTurn) { }
    void ICombatObserver.notifyAttackBlocked(int entityId, int remainingTurn) {
        attackBlocked.Play();
    }
    void ICombatObserver.notifyTakeDamage(int entityId, int value) { }
    void ICombatObserver.notifyHealed(int entityId, int value) { }
}
