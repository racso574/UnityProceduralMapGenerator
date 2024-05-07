using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixPseudorandomGenerator : MonoBehaviour
{
    private int roomsPlaced;
    private int deadEndsCount;

    private int contarodorejecuciones;

    private int[,] bitmap;
    private int[,] roomOrientationMap;

    public (int[,], int[,]) StartRoomOrientationMatrixGeneration(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed)
    {
        contarodorejecuciones = 0;
        UnityEngine.Random.InitState(mapSeed);
        bitmap = new int[MapSize.x, MapSize.y];
        PrepareMatrixBeforeFilling(MapSize,RoomQuantity,minDeadEnds,mapSeed); 
        Debug.Log("contarodorejecuciones"+ contarodorejecuciones);
        return (bitmap, roomOrientationMap);
    }

    void PrepareMatrixBeforeFilling(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed){
        
        roomsPlaced = 1;
        deadEndsCount = 0;
        contarodorejecuciones++;

        int casillaCentralX = bitmap.GetLength(0) / 2;
        int casillaCentralY = bitmap.GetLength(1) / 2;

        bitmap[casillaCentralX, casillaCentralY] = 1;

        FillOutTheMatrix(MapSize,RoomQuantity,minDeadEnds,mapSeed);
        MatrixRoomDirectionTypeReEnumeration(MapSize,RoomQuantity,minDeadEnds,mapSeed);
    }
    
    void FillOutTheMatrix(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed)
    {
        while (roomsPlaced < RoomQuantity)
        {
            for (int i = 0; i < bitmap.GetLength(0); i++)
            {
                for (int j = 0; j < bitmap.GetLength(1); j++)
                {
                    if (bitmap[i, j] == 0)
                    {
                        int adjacentOnes = CountAdjacentOnes(i, j);

                        if (adjacentOnes == 1)
                        {
                            float randomProbability = UnityEngine.Random.value;
                            float probabilityThreshold = 0.5f;

                            if (randomProbability <= probabilityThreshold)
                            {
                                bitmap[i, j] = 1;
                                roomsPlaced++;
                            }
                        }
                    }
                }
            }
        }  
    }

    void MatrixRoomDirectionTypeReEnumeration(Vector2Int MapSize, int RoomQuantity, int minDeadEnds, int mapSeed){

        roomOrientationMap = new int[bitmap.GetLength(0), bitmap.GetLength(1)];

        for (int i = 0; i < bitmap.GetLength(0); i++)
        {
            for (int j = 0; j < bitmap.GetLength(1); j++)
            {
                int adjacentOnes = CountAdjacentOnes(i, j);

                if (bitmap[i, j] == 1)
                {
                    if (adjacentOnes == 4)
                        roomOrientationMap[i, j] = 15;
                    else if (adjacentOnes == 3)
                        roomOrientationMap[i, j] = GetAdjacentConfiguration(i, j);
                    else if (adjacentOnes == 2)
                        roomOrientationMap[i, j] = GetAdjacentConfiguration(i, j);
                    else{
                        roomOrientationMap[i, j] = GetAdjacentConfiguration(i, j);
                        deadEndsCount++;
                    }
                        
                }
                else
                {
                    roomOrientationMap[i, j] = 0;
                }
            }
        }
        if (deadEndsCount >= minDeadEnds)
        {
 
            PrintMatrix(roomOrientationMap);
            
        }
        else
        {
            SetMatrixToZero(bitmap);
            SetMatrixToZero(roomOrientationMap);
            PrepareMatrixBeforeFilling(MapSize,RoomQuantity,minDeadEnds,mapSeed); 
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

    private void SetMatrixToZero(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = 0;
            }
        }
    }

    int GetAdjacentConfiguration(int x, int y)
    {
        int configuration = 0;

        if (x > 0 && bitmap[x - 1, y] == 1) configuration += 2;
        if (x < bitmap.GetLength(0) - 1 && bitmap[x + 1, y] == 1) configuration += 1;
        if (y > 0 && bitmap[x, y - 1] == 1) configuration += 4;
        if (y < bitmap.GetLength(1) - 1 && bitmap[x, y + 1] == 1) configuration += 8;

        return configuration;
    }

    int CountAdjacentOnes(int x, int y)
    {
        int count = 0;

        if (x > 0 && bitmap[x - 1, y] == 1) count++;
        if (x < bitmap.GetLength(0) - 1 && bitmap[x + 1, y] == 1) count++;
        if (y > 0 && bitmap[x, y - 1] == 1) count++;
        if (y < bitmap.GetLength(1) - 1 && bitmap[x, y + 1] == 1) count++;

        return count;
    }
}


