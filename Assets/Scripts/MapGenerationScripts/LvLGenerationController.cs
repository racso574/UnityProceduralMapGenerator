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

    private int[,] roomOrientationMap;
    public List<Vector2> DeadEndsList;
    public GameObject cubePrefab;

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
        DeadEndsSorter roomTypeDesignator = GetComponent<DeadEndsSorter>();
        mapSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        roomOrientationMap = matrixGenerator.StartRoomOrientationMatrixGeneration(MapSize, RoomQuantity, minDeadEnds, mapSeed);
        DeadEndsList = roomTypeDesignator.GetSortedDeadEndsList(roomOrientationMap , mapSeed);
        
        // Llamamos a DrawMatrix del script LvlSchemaMapGeneration
        LvlSchemaMapGeneration mapGenerator = GetComponent<LvlSchemaMapGeneration>();
        mapGenerator.DrawMatrix(roomOrientationMap, DeadEndsList, cubePrefab);
    }
}
