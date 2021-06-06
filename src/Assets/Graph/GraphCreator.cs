using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GraphCreator {
    public const int NUMBER_ROOMS = 20;

    private static void initializeArray(bool[,] test) {
        for (int j = 0; j < 5; ++j) {
            test[0, j] = false;
        }
        test[0, 2] = true;
        for (int i = 1; i < NUMBER_ROOMS; ++i) {
            for (int j = 0; j < 5; ++j) {
                test[i, j] = true;
            }
        }
        for (int j = 0; j < 5; ++j) {
            test[NUMBER_ROOMS, j] = false;
        }
        test[NUMBER_ROOMS, 2] = true;
    }

    private static List<int> getNodesAt(bool[,] test, int i) {
        List<int> nodes = new List<int>();
        for (int j = 0; j < 5; ++j) {
            if (test[i, j]) {
                nodes.Add(j);
            }
        }
        return nodes;
    }

    private static int eliminateUnreachableNodes(bool[,] test, List<int> previousNodes, int i, int nbEliminations) {
        for (int j = 0; j < 5; ++j) {
            Vector2 node = new Vector2(i, j);
            bool reachable = false;
            foreach (int n in previousNodes) {
                Vector2 previous = new Vector2(i - 1, n);
                if (Vector2.Distance(previous, node) < 2) {
                    reachable = true;
                    break;
                }
            }
            if (!reachable) {
                test[i, j] = false;
                nbEliminations--;
            }
        }
        return nbEliminations;
    }



    public static Graph createGraph() {
        bool[,] test = new bool[NUMBER_ROOMS + 1, 5];
        // initialize the array
        initializeArray(test);

        // eliminate nodes
        for (int i = 1; i < NUMBER_ROOMS - 1; ++i) {
            int nbEliminations = Random.Range(2, 3 + 1);

            // find previous nodes
            List<int> previousNodes = getNodesAt(test, i - 1);

            // eliminate unreachable nodes
            nbEliminations = eliminateUnreachableNodes(test, previousNodes, i, nbEliminations);

            List<int> currentNodes = getNodesAt(test, i);

            // randomly eliminate reachable nodes to get an interesting graph
            while (nbEliminations > 0) {
                Dictionary<int, List<int>> possibleLinks = new Dictionary<int, List<int>>();
                foreach (int p in previousNodes) {
                    possibleLinks.Add(p, new List<int>());
                    Vector2 previous = new Vector2(i - 1, p);
                    foreach (int n in currentNodes) {
                        Vector2 node = new Vector2(i, n);
                        if (Vector2.Distance(previous, node) < 2) {
                            possibleLinks[p].Add(n);
                        }
                    }
                }

                // Remove from paths eligible to deletion those with only one direction left
                foreach (var p in possibleLinks) {
                    if (p.Value.Count == 1) {
                        currentNodes.Remove(p.Value[0]);
                    }
                }

                if (currentNodes.Count == 0)
                    break;
                // Node is deleted only if it doesn't kill a path
                int id = Random.Range(0, currentNodes.Count);
                test[i, currentNodes[id]] = false;
                currentNodes.RemoveAt(id);
                nbEliminations--;
            }
        }

        // Make sure all paths converge to the end
        for (int j = 0; j < 5; ++j) {
            test[NUMBER_ROOMS - 1, j] = false;
        }
        if (test[NUMBER_ROOMS - 2, 0] || test[NUMBER_ROOMS - 2, 1] || test[NUMBER_ROOMS - 2, 2])
            test[NUMBER_ROOMS - 1, 1] = true;
        if (test[NUMBER_ROOMS - 2, 2] || test[NUMBER_ROOMS - 2, 3] || test[NUMBER_ROOMS - 2, 4])
            test[NUMBER_ROOMS - 1, 3] = true;

        // float emptyRoomsProportions = 0.1f;
        // float enemyRoomsProportions = 0.65f;
        // float treasureRoomsProportions = 0.2f;

        Graph graph = new Graph();
        // Generate a graph from the array
        int[] previousNodesArr = new int[5] { -1, -1, -1, -1, -1 };
        graph.addNode(RoomType.EMPTY_ROOM, false, new Vector2Int(0, 2));
        previousNodesArr[2] = 0;
        int nbTreasures = 0, nbEnemy = 0, nbEmpty = 0;
        for (int i = 1; i <= NUMBER_ROOMS; i++) {
            List<int> previousNodes = getNodesAt(test, i - 1);
            List<int> currentNodes = getNodesAt(test, i);

            int[] currentNodesArr = new int[5] { -1, -1, -1, -1, -1 };
            int[] previousNbLinks = new int[5] { 0, 0, 0, 0, 0 };

            foreach (int n in currentNodes) {

                RoomType type = RoomType.BOSS_ROOM;
                bool hidden = false;
                if (i < NUMBER_ROOMS) {
                    float res = Random.Range(0.0f, 1.0f);
                    if (res <= 0.1f) {
                        nbEmpty++;
                        type = RoomType.EMPTY_ROOM;
                    } else if (res <= 0.25f) {
                        nbTreasures++;
                        type = RoomType.TREASURE_ROOM;
                    } else {
                        nbEnemy++;
                        type = RoomType.ENEMY_ROOM;
                    }
                    res = Random.Range(0.0f, 1.0f);
                    hidden = (res <= (double)(Mathf.Max(0, i - 7)) / (double)NUMBER_ROOMS);
                }
                // define if room is hidden
                int index = graph.nodes.Count;
                graph.addNode(type, hidden, new Vector2Int(i, n));
                currentNodesArr[n] = index;

                Vector2 node = new Vector2(i, n);
                List<int> possibleLinks = new List<int>();
                foreach (int p in previousNodes) {
                    Vector2 previous = new Vector2(i - 1, p);
                    if (Vector2.Distance(previous, node) < 2) {
                        possibleLinks.Add(p);
                    }
                }

                int id = Random.Range(0, possibleLinks.Count);
                graph.addLink(previousNodesArr[possibleLinks[id]], index);
                previousNbLinks[possibleLinks[id]]++;
            }

            foreach (int p in previousNodes) {
                if (previousNbLinks[p] == 0) {
                    Vector2 previous = new Vector2(i - 1, p);
                    List<int> possibleLinks = new List<int>();
                    foreach (int n in currentNodes) {
                        Vector2 node = new Vector2(i, n);
                        if (Vector2.Distance(previous, node) < 2) {
                            possibleLinks.Add(n);
                        }
                    }
                    int id = Random.Range(0, possibleLinks.Count);
                    graph.addLink(previousNodesArr[p], currentNodesArr[possibleLinks[id]]);
                    previousNbLinks[p]++;
                }
            }

            previousNodesArr = currentNodesArr;
        }
        // Debug.Log("Enemy rooms : " + nbEnemy);
        // Debug.Log("Empty rooms : " + nbEmpty);
        // Debug.Log("Treasure rooms : " + nbTreasures);

        return graph;
    }
}