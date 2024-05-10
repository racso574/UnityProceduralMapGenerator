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
        DrawMatrix(roomOrientationMap,DeadEndsList, cubePrefab);
    }

    public static void DrawMatrix(int[,] matrix, List<Vector2> highlightedCells, GameObject cubePrefab)
    {
        if (matrix == null)
        {
            Debug.LogWarning("No se ha especificado una matriz.");
            return;
        }

        if (highlightedCells == null)
        {
            Debug.LogWarning("No se ha especificado una lista de celdas destacadas.");
            return;
        }

        Color blackColor = Color.black;
        Color whiteColor = Color.white;
        Color[] highlightColors = { Color.green, Color.red, Color.magenta, Color.blue }; // Colores para resaltar las celdas de la lista

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Vector3 position = new Vector3(j, -i, 0); // Invertimos la posici�n en Y para que la matriz se vea correctamente

                Color color = (matrix[i, j] == 0) ? blackColor : whiteColor;

                // Comprobar si la celda actual est� en la lista de celdas destacadas
                Vector2Int currentCell = new Vector2Int(i, j);
                int highlightedIndex = highlightedCells.IndexOf(currentCell);
                if (highlightedIndex != -1)
                {
                    // Asignar un color diferente seg�n el �ndice en la lista
                    if (highlightedIndex < highlightColors.Length)
                    {
                        color = highlightColors[highlightedIndex];
                    }
                    else
                    {
                        color = Color.yellow; // Si hay m�s elementos en la lista que colores, usar amarillo para los restantes
                    }
                }

                GameObject cube = GameObject.Instantiate(cubePrefab, position, Quaternion.identity);
                cube.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}


