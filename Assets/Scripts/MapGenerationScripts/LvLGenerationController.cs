using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LvlGenerationController : MonoBehaviour
{
    public Vector2Int MapSize;
    public int RoomQuantity;
    public int minDeadEnds;

    

    public int mapSeed;
    public int testiterations;

    private int[,] bitmap;
    private int[,] roomOrientationMap;
    private int[,] roomTypeMap;

    public void GenerateNewMatrix()
    {
        for (int i = 0; i < testiterations; i++)
        {
            Funcion1();
        }
    }

    private void Funcion1()
    {
        MatrixPseudorandomGenerator matrixGenerator = GetComponent<MatrixPseudorandomGenerator>();
        RoomTypeDesignator roomTypeDesignator = GetComponent<RoomTypeDesignator>();
        mapSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        (bitmap, roomOrientationMap) = matrixGenerator.StartRoomOrientationMatrixGeneration(MapSize, RoomQuantity, minDeadEnds, mapSeed);
        roomTypeMap = roomTypeDesignator.StartRoomTypeMatrixGeneration();
    }
}


