using UnityEngine;
using System;

public class GenRoads : MonoBehaviour
{
    public int LengthRange;
    public int WidthRange;/*
    [Range(1, 20)]
    public int minStreetLength;*/

    public float moveX;
    public float moveY;

    public float strenth;
    public float size;

    Point[,] map;
    float[,] xChanges;
    float[,] yChanges;

    public GameObject line;
    GameObject parent;

    //Creates basic grid
    public void Generate()
    {
        DestroyImmediate(parent);
        parent = new GameObject("Storage");

        map = new Point[LengthRange, WidthRange];
        xChanges = new float[LengthRange, WidthRange];
        yChanges = new float[LengthRange, WidthRange];
        System.Random r = new System.Random();

        //First declaration of map
        for (int i = 0; i < LengthRange; i++)
            for (int j = 0; j < WidthRange; j++)
            {
                map[i, j] = new Point();
                map[i, j].x = i * 2;
                map[i, j].y = j * 2;
                map[i, j].changed = false;
            }

        //Declaration of changes
        for (int i = 0; i < LengthRange; i++)
            for (int j = 0; j < WidthRange; j++)
            {
                xChanges[i, j] = Mathf.PerlinNoise((float)i / LengthRange * size + moveX, (float)j / WidthRange * size + moveY) * strenth - strenth/2;
                yChanges[i, j] = Mathf.PerlinNoise((float)i / LengthRange * size + moveX + LengthRange, (float)j / WidthRange * size + moveY + WidthRange) * strenth - strenth/2;
            }

        //Applying of changes
        for (int i = 0; i < LengthRange; i++)
            for (int j = 0; j < WidthRange; j++)
            {
                map[i, j].x += xChanges[i, j];
                map[i, j].y += yChanges[i, j];

                if (i < LengthRange - 1)
                {
                    map[i + 1, j].x += xChanges[i, j];
                    map[i + 1, j].y += yChanges[i, j];
                }
                if (i > 0)
                {
                    map[i - 1, j].x += xChanges[i, j];
                    map[i - 1, j].y += yChanges[i, j];
                }
                if (j < WidthRange - 1)
                {
                    map[i, j + 1].x += xChanges[i, j];
                    map[i, j + 1].y += yChanges[i, j];
                }
                if (j > 0)
                {
                    map[i, j - 1].x += xChanges[i, j];
                    map[i, j - 1].y += yChanges[i, j];
                }

                map[i, j].changed = true;
            }

        Print();
    }

    public void Print()
    {
        System.Random r = new System.Random();

        for (int i = 0; i < LengthRange; i++)
        {
            int streetStart = 0;//r.Next(WidthRange - minStreetLength);
            int streetLength = WidthRange; //r.Next(minStreetLength, WidthRange - streetStart);
            GameObject newLine = Instantiate(line);
            LineRenderer rend = newLine.GetComponent<LineRenderer>();
            rend.positionCount = streetLength;

            for (int j = streetStart; j < streetStart + streetLength; j++)
            {
                rend.SetPosition(j - streetStart, new Vector3(map[i, j].x, 0, map[i, j].y));
            }
            newLine.transform.parent = parent.transform;
        }

        for (int j = 0; j < WidthRange; j++)
        {
            int streetStart = 0; //r.Next(LengthRange - minStreetLength);
            int streetLength = LengthRange; //r.Next(minStreetLength, LengthRange - streetStart);
            GameObject newLine = Instantiate(line);
            LineRenderer rend = newLine.GetComponent<LineRenderer>();
            rend.positionCount = streetLength;

            for (int i = streetStart; i < streetStart + streetLength; i++)
            {
                rend.SetPosition(i - streetStart, new Vector3(map[i, j].x, 0, map[i, j].y));
            }
            newLine.transform.parent = parent.transform;
        }
    }
}

public class Point
{
    public float x;
    public float y;
    public bool changed;
}