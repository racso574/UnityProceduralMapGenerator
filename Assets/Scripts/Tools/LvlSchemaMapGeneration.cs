using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LvlSchemaMapGeneration : MonoBehaviour
{
    public void DrawMatrix(int[,] matrix, List<Vector2> highlightedCells, GameObject cubePrefab)
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
        ClearMap();

        Color blackColor = Color.black;
        Color whiteColor = Color.white;
        Color[] highlightColors = { Color.green, Color.red, Color.yellow, Color.yellow }; // Colores para resaltar las celdas de la lista

        Transform parentTransform = this.transform; // Obtener el transform del objeto que tiene este script
        int highlightCounter = 2; // Empezamos en 2 para las celdas amarillas

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Vector3 position = new Vector3(j, -i, 0); // Invertimos la posición en Y para que la matriz se vea correctamente

                Color color = (matrix[i, j] == 0) ? blackColor : whiteColor;

                // Comprobar si la celda actual está en la lista de celdas destacadas
                Vector2Int currentCell = new Vector2Int(i, j);
                int highlightedIndex = highlightedCells.IndexOf(currentCell);
                if (highlightedIndex != -1)
                {
                    // Asignar un color diferente según el índice en la lista
                    if (highlightedIndex < highlightColors.Length)
                    {
                        color = highlightColors[highlightedIndex];
                    }
                    else
                    {
                        color = Color.yellow; // Si hay más elementos en la lista que colores, usar amarillo para los restantes
                    }

                    // Instanciar el cubo y configurar el texto
                    GameObject cube = GameObject.Instantiate(cubePrefab, position, Quaternion.identity, parentTransform);
                    cube.GetComponent<SpriteRenderer>().color = color;

                    // Buscar el componente TextMeshPro en el hijo Canvas del cubo
                    TextMeshProUGUI textComponent = cube.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        if (color == Color.red)
                        {
                            textComponent.text = "1";
                        }
                        else if (color == Color.yellow)
                        {
                            textComponent.text = highlightCounter.ToString();
                            highlightCounter++;
                        }
                    }
                }
                else
                {
                    GameObject cube = GameObject.Instantiate(cubePrefab, position, Quaternion.identity, parentTransform);
                    cube.GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
    }

    public void ClearMap()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        // Destruir cada hijo en la lista
        foreach (Transform child in children)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
