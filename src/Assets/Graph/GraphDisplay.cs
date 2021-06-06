using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GraphDisplay : MonoBehaviour {

    [SerializeField]
    AudioSource closeSound;
    public List<Sprite> buttonImage;
    RectTransform canvasTransform;
    List<GameObject> buttons = new List<GameObject>();
    List<GameObject> lines = new List<GameObject>();
    GameObject token;
    Graph graph;

    // int position = 0;

    static private float nodePosX(Graph graph, int i) {
        return 80 + (graph.nodes[i].pos.x) * 32;
    }

    static private float nodePosY(Graph graph, int i) {
        return (graph.nodes[i].pos.y - 2) * 40;
    }


    static private Vector3 nodePosition(Graph graph, int i) {
        if (i == 0 || i == graph.nodes.Count - 1) {
            return new Vector3(nodePosX(graph, i), nodePosY(graph, i), 0);
        } else {
            float randX = Random.Range(-7f, 7f);
            float randY = Random.Range(-10f, 10f);
            return new Vector3(nodePosX(graph, i) + randX, nodePosY(graph, i) + randY, 0);
        }
    }

    static public List<Vector3> nodesPositions(Graph graph)
    {
        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < graph.nodes.Count; ++i)
        {
            positions.Add(nodePosition(graph, i));
        }
        return positions;
    }

    private Vector3 tokenPosition(int i) {
        Vector3 buttonPos = buttons[i].GetComponent<RectTransform>().localPosition;
        return new Vector3(buttonPos.x, buttonPos.y, 0);
    }

    public void clearChildren() {
        foreach (GameObject child in buttons) {
            GameObject.Destroy(child);
        }
        buttons.Clear();
        foreach (GameObject child in lines) {
            GameObject.Destroy(child);
        }
        lines.Clear();
    }

    private void activateDeactivateButtons(int newPos, HashSet<int> visitedNodes) {
        // foreach (int n in graph.nodes[position].nextNodes) {
        //     buttons[n].GetComponent<Button>().interactable = false;
        // }
        foreach (int n in graph.nodes[newPos].nextNodes) {
            buttons[n].GetComponent<Button>().interactable = true;
        }
        foreach (int n in visitedNodes) {
             buttons[n].GetComponent<Image>().sprite = buttonImage[5];
             Color dis = Color.white;
             ColorBlock blk = buttons[n].GetComponent<Button>().colors;
             blk.disabledColor = dis;
             buttons[n].GetComponent<Button>().colors = blk;
        }
    }

    public void moveToPosition(int graphPosition) {
        generateToken(graphPosition);
        // visitedNodes.Add(position);
        activateDeactivateButtons(graphPosition, GameManager.instance.GetVisitedNodes());
        // position = graphPosition;
    }

    void DisableAllButtons() {
        foreach (GameObject o in buttons) {
            Button b = o.GetComponent<Button>();
            b.interactable = false;
        }
    }

    IEnumerator WaitForSound(int number) {
        if (!closeSound.isPlaying) {
            closeSound.Play();
        }
        while (closeSound.isPlaying) {
            yield return null;
        }
        GameManager.instance.nextRoom(number);
    }

    public void displayGraph(Graph graphe, List<Vector3> positions) {
        graph = graphe;
        clearChildren();
        for (int i = 0; i < graph.nodes.Count; i++) {
            GameObject temp = new GameObject();
            temp.transform.SetParent(transform);
            lines.Add(temp);
            GameObject button = new GameObject();
            button.transform.SetParent(transform);
            button.AddComponent<CanvasRenderer>();
            RectTransform rectTransform = button.AddComponent<RectTransform>();
            Button myButton = button.AddComponent<Button>();
            Image mImage = button.AddComponent<Image>();
            myButton.targetGraphic = mImage;
            int number = i;
            myButton.onClick.AddListener(delegate {
                DisableAllButtons();
                StartCoroutine(WaitForSound(number));
            });
            if (!graph.nodes[i].hidden) {
                if (graph.nodes[i].type == RoomType.EMPTY_ROOM)
                    mImage.sprite = buttonImage[0];
                if (graph.nodes[i].type == RoomType.TREASURE_ROOM)
                    mImage.sprite = buttonImage[1];
                if (graph.nodes[i].type == RoomType.ENEMY_ROOM)
                    mImage.sprite = buttonImage[2];
                if (graph.nodes[i].type == RoomType.BOSS_ROOM)
                    mImage.sprite = buttonImage[3];
            } else
                mImage.sprite = buttonImage[4];
            // Set disabled color alpha at 1
            Color dis = myButton.colors.disabledColor;
            dis /= 1.5f;
            dis.a = 1;
            ColorBlock blk = myButton.colors;
            blk.disabledColor = dis;
            myButton.colors = blk;

            myButton.interactable = false;
            rectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            rectTransform.localPosition = positions[i];
            buttons.Add(button);
        }
        generateLine(graph);
        // generateToken(0);
        // activateDeactivateButtons(0);
        // position = 0;
    }

    void generateToken(int graphPosition) {
        RectTransform rectTransform = token.GetComponent<RectTransform>();
        rectTransform.localPosition = tokenPosition(graphPosition);
        rectTransform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        token.transform.SetAsLastSibling();
    }

    void generateLine(Graph graph) {
        for (int i = 0; i < graph.nodes.Count; i++) {

            LineRenderer2D line = lines[i].AddComponent<LineRenderer2D>();
            line.gridSize = new Vector2Int(1, 1);
            line.color = Color.black;
            for (int j = 0; j < graph.nodes[i].nextNodes.Count; j++) {
                float w = (float)GetComponent<RectTransform>().rect.width / 2.0f * canvasTransform.localScale.x;
                Vector3 begin = (buttons[i].GetComponent<RectTransform>().position - new Vector3(w, 0.0f, 0.0f)) / 100.0f;
                Vector3 end = (buttons[graph.nodes[i].nextNodes[j]].GetComponent<RectTransform>().position - new Vector3(w, 0.0f, 0.0f)) / 100.0f;

                line.points.Add(begin);
                line.points.Add(end);
            }
        }
    }
    // Start is called before the first frame update
    void Awake() {
        canvasTransform = (RectTransform)transform.GetComponentInParent<Canvas>().transform;
        token = GameObject.Find("Token");
    }

};