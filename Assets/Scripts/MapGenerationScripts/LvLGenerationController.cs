using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LvLGenerationController : MonoBehaviour
{
    public Vector2Int MapSize;
    public int RoomQuantity;
    public int minDeadEnds;
    public int mapSeed;

    private int[,] bitmap;
    private int[,] roomorientationmap;

    public void GenerateNewMatrix()
    {
        MatrixPseudorandomGenerator matrixGenerator = GetComponent<MatrixPseudorandomGenerator>();
        mapSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        (bitmap, roomorientationmap) = matrixGenerator.StartRoomGeneration(MapSize, RoomQuantity, minDeadEnds, mapSeed);
        FunctionToExecuteAfterMatrixGeneration();
    }

    public void FunctionToExecuteAfterMatrixGeneration()
    {
        // Aquí es donde puedes ejecutar la función que desees después de recibir las matrices
        Debug.Log("Matrices generadas correctamente.");
    }

  
}

