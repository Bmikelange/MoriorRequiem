using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    EMPTY_ROOM, TREASURE_ROOM, ENEMY_ROOM, BOSS_ROOM
}


public class Room
{
    RoomType type;
    bool hidden;

}

public class Node
{
    public List<int> nextNodes;
    public Vector2Int pos;
    public RoomType type;
    public bool hidden;

    public Node(RoomType _type, bool _hidden, Vector2Int _pos)
    {
        nextNodes = new List<int>();
        type = _type;
        hidden = _hidden;
        pos = _pos;
    }

    public RoomType getType()
    {
        return type;
    }
}

public class Graph
{
    public List<Node> nodes = new List<Node>();

    public void addNode(RoomType type, bool hidden, Vector2Int pos)
    {
        nodes.Add(new Node(type, hidden, pos));
    }

    public void addLink(int origin, int target)
    {
        nodes[origin].nextNodes.Add(target);
    }
}



