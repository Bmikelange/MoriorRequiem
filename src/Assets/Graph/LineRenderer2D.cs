﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRenderer2D : Graphic {
    public Vector2Int gridSize;
    public List<Vector2> points = new List<Vector2>();
    float width;
    float height;
    float unitWidth;
    float unitHeight;

    public float thickness = 3f;

    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;

        if (points.Count < 2) {
            return;
        }

        for (int i = 0; i < points.Count; i++) {
            Vector2 point = points[i];
            DrawVerticesForPoint(point, vh);
        }

        for (int i = 0; i < points.Count - 1; i++) {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }
    }

    void DrawVerticesForPoint(Vector2 point, VertexHelper vh) {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(0, -thickness / 2);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
        vertex.position = new Vector3(0, thickness / 2);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

    }
}
