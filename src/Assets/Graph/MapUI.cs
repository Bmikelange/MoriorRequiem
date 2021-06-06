using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour {

    GraphDisplay graphDisplay;
    // Start is called before the first frame update
    void Start() {
        graphDisplay = GetComponentInChildren<GraphDisplay>();
        graphDisplay.displayGraph(GameManager.instance.GetGraph(), GameManager.instance.GetNodesPositions());
        graphDisplay.moveToPosition(GameManager.instance.GetPosition());
    }
}
