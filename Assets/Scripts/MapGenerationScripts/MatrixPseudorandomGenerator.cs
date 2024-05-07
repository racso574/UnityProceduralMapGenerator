using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixPseudorandomGenerator : MonoBehaviour
{
    private int maxDeadEndsIteration;
    private int DeadEndsIteration;
    private int roomsPlaced;
    private int deadEndsCount;

    private int[,] binariMatrix;
    private int[,] roomLayoutTypeMatrix;

    public (int[,], int[,]) StartRoomGeneration(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed)
    {
        UnityEngine.Random.InitState(mapSeed);
        binariMatrix = new int[MapSize.x, MapSize.y];
        roomsPlaced = 0;
        deadEndsCount = 0;
        maxDeadEndsIteration = 1000;
        PrepareMatrixBeforeFilling(MapSize,RoomQuantity,minDeadEnds,mapSeed); 
        FillOutTheMatrix(MapSize,RoomQuantity,minDeadEnds,mapSeed);
        MatrixRoomLayoutTypeReEnumeration(MapSize,RoomQuantity,minDeadEnds,mapSeed);  
        return (binariMatrix, roomLayoutTypeMatrix);
    }

    void PrepareMatrixBeforeFilling(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed){
        
        int casillaCentralX = binariMatrix.GetLength(0) / 2;
        int casillaCentralY = binariMatrix.GetLength(1) / 2;

        binariMatrix[casillaCentralX, casillaCentralY] = 1;

        roomsPlaced++;
        DeadEndsIteration++;
        
    }
    
    void FillOutTheMatrix(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed)
    {
       
        int maxIterations = 10000; // Ajusta según sea necesario
        int iterationCount = 0;

        while (roomsPlaced < RoomQuantity && iterationCount < maxIterations)
        {
            for (int i = 0; i < binariMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < binariMatrix.GetLength(1); j++)
                {
                    if (roomsPlaced < RoomQuantity && binariMatrix[i, j] == 0)
                    {
                        int adjacentOnes = CountAdjacentOnes(i, j);

                        if (adjacentOnes == 1)
                        {
                            float randomProbability = UnityEngine.Random.value;
                            float probabilityThreshold = 0.5f;

                            if (randomProbability <= probabilityThreshold)
                            {
                                binariMatrix[i, j] = 1;
                                roomsPlaced++;
                            }
                        }
                    }
                }
            }

            iterationCount++;
        }

        if (iterationCount >= maxIterations)
        {
            Debug.LogError("Generación de mapa fallida. Se alcanzó el límite de iteraciones.");
        }
           
           
    }

    void MatrixRoomLayoutTypeReEnumeration(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed){

        roomLayoutTypeMatrix = new int[binariMatrix.GetLength(0), binariMatrix.GetLength(1)];

        for (int i = 0; i < binariMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < binariMatrix.GetLength(1); j++)
            {
                int adjacentOnes = CountAdjacentOnes(i, j);

                if (binariMatrix[i, j] == 1)
                {
                    if (adjacentOnes == 4)
                        roomLayoutTypeMatrix[i, j] = 15;
                    else if (adjacentOnes == 3)
                        roomLayoutTypeMatrix[i, j] = GetAdjacentConfiguration(i, j);
                    else if (adjacentOnes == 2)
                        roomLayoutTypeMatrix[i, j] = GetAdjacentConfiguration(i, j);
                    else{
                        roomLayoutTypeMatrix[i, j] = GetAdjacentConfiguration(i, j);
                        deadEndsCount++;
                    }
                        
                }
                else
                {
                    roomLayoutTypeMatrix[i, j] = 0;
                }
            }
        }
        if (deadEndsCount >= minDeadEnds)
        {
 
            PrintMatrix(roomLayoutTypeMatrix);
            
        }
        else if (DeadEndsIteration < maxDeadEndsIteration)
        {
            // Recursive call with updated parameters
            StartRoomGeneration(MapSize, RoomQuantity, minDeadEnds, mapSeed);
            Debug.Log("a generar de nuevo");
        }
        else
        {
            Debug.Log("liada maxima datos de mapa mal configurados");
        }
    }

    private void PrintMatrix(int[,] matrix){
        string matrixString = "Matriz recibida:\n";

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrixString += matrix[i, j] + " ";
            }
            matrixString += "\n";
        }

        Debug.Log(matrixString);
    }

    int GetAdjacentConfiguration(int x, int y)
    {
        int configuration = 0;

        if (x > 0 && binariMatrix[x - 1, y] == 1) configuration += 2;
        if (x < binariMatrix.GetLength(0) - 1 && binariMatrix[x + 1, y] == 1) configuration += 1;
        if (y > 0 && binariMatrix[x, y - 1] == 1) configuration += 4;
        if (y < binariMatrix.GetLength(1) - 1 && binariMatrix[x, y + 1] == 1) configuration += 8;

        return configuration;
    }

    int CountAdjacentOnes(int x, int y)
    {
        int count = 0;

        if (x > 0 && binariMatrix[x - 1, y] == 1) count++;
        if (x < binariMatrix.GetLength(0) - 1 && binariMatrix[x + 1, y] == 1) count++;
        if (y > 0 && binariMatrix[x, y - 1] == 1) count++;
        if (y < binariMatrix.GetLength(1) - 1 && binariMatrix[x, y + 1] == 1) count++;

        return count;
    }
}


