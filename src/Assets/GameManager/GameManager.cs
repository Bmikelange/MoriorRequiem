using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    MAIN_MENU,
    EMPTY_ROOM,
    MAP,
    TREASURE_ROOM,
    COMBAT,
    INTRO_BOSS,
    BOSS,
    WIN
};

public class GameManager : MonoBehaviour {
    // Static instance for Singleton pattern
    public static GameManager instance = null;
    public const int NB_COMBAT_ROOMS = 4;
    // Graph data
    Graph levelGraph;
    int graphPosition;
    List<Vector3> nodesPositions = new List<Vector3>();
    HashSet<int> visitedNodes = new HashSet<int>();

    List<Dice> dices = new List<Dice>();
    List<Dice> enemyDices = new List<Dice>();

    // Game data
    PlayerInfo playerInfo;
    int level;
    GameState gameState;

    // Singleton pattern
    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        GameObject.DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        SetGameState(GameState.MAIN_MENU);
    }

    public void NewGame() {
        levelGraph = GraphCreator.createGraph();
        nodesPositions = GraphDisplay.nodesPositions(levelGraph);
        graphPosition = 0;
        visitedNodes = new HashSet<int>();
        dices = GetComponent<DiceLoader>().getListOfDices();
        enemyDices = GetComponent<DiceLoader>().getListOfEnemyDices();
        playerInfo = new PlayerInfo(100, 100, new List<DiceSlot>() { new DiceSlot(dices[2]) });
        level = 1;
        SetGameState(GameState.MAP);
    }

    public void enterRoom(RoomType type) {
        switch (type) {
            case RoomType.EMPTY_ROOM:
                SetGameState(GameState.EMPTY_ROOM);
                break;
            case RoomType.TREASURE_ROOM:
                SetGameState(GameState.TREASURE_ROOM);
                break;
            case RoomType.ENEMY_ROOM:
                SetGameState(GameState.COMBAT);
                break;
            case RoomType.BOSS_ROOM:
                SetGameState(GameState.INTRO_BOSS);
                break;
        }
    }

    public void nextRoom(int number) {
        level++;
        visitedNodes.Add(graphPosition);
        graphPosition = number;
        enterRoom(levelGraph.nodes[graphPosition].getType());
    }

    public Graph GetGraph() {
        return levelGraph;
    }

    public List<Vector3> GetNodesPositions() {
        return nodesPositions;
    }

    public HashSet<int> GetVisitedNodes() {
        return visitedNodes;
    }

    public int GetPosition() {
        return graphPosition;
    }

    public PlayerInfo GetPlayerInfo() {
        return playerInfo;
    }

    public void SetPlayerInfo(PlayerInfo newInfo) {
        playerInfo = newInfo;
    }

    public int GetLevel() {
        return level;
    }

    public GameState GetGameState() {
        return gameState;
    }

    public void SetGameState(GameState value) {
        gameState = value;
        OnGameStateChanged();
    }

    private void OnGameStateChanged() {
        SceneManager.LoadScene("LOADING");
    }

    public void HealPlayer() {
        playerInfo.life = playerInfo.lifeMax;
    }

    public bool PlayerHasFreeSlot() {
        return playerInfo.slots.Count < 3;
    }

    public List<DiceSlot> GetPlayerSlots() {
        return playerInfo.slots;
    }

    public void ClaimDice(Dice dice, int slotToReplace = -1) {
        if (slotToReplace < 0) {
            playerInfo.slots.Add(new DiceSlot(dice));
        } else if (slotToReplace < 3) {
            playerInfo.slots[slotToReplace] = new DiceSlot(dice);
        }
        Debug.Assert(playerInfo.slots.Count <= 3);
    }

    public List<Dice> GetListOfDices() { return dices; }
    public List<Dice> GetListOfEnemyDices() { return enemyDices; }

    private void Update() {
        //if (Debug.isDebugBuild) {
/*            if (Input.GetKeyDown(KeyCode.B)) {
                graphPosition = levelGraph.nodes.Count - 2;
                SetGameState(GameState.MAP);
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                playerInfo.slots = new List<DiceSlot>() { new DiceSlot(DiceLootTable.BoostDice(dices[2], 999999999)), new DiceSlot(DiceLootTable.BoostDice(dices[7], 999999999)) };
            }*/
        //}
        if(Input.GetKeyDown(KeyCode.M))
        {
            AudioListener.volume = (AudioListener.volume > 0f ? 0f : 1f);
        }
    }
}
