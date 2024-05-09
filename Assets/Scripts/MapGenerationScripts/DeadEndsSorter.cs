using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadEndsSorter : MonoBehaviour
{
    private List<Vector2> DeadEndsList;

    public List<Vector2> GetSortedDeadEndsList(int[,] roomOrientationMap)
    {
        DeadEndsList = new List<Vector2>();

        // Encuentra todas las casillas deadends y guárdalas en una lista
        List<Vector2> deadEnds = FindDeadEnds(roomOrientationMap);

        // Si no hay casillas deadends, devuelve una lista vacía
        if (deadEnds.Count == 0)
        {
            return DeadEndsList;
        }

        // Elije una casilla deadend aleatoriamente
        Vector2 startDeadEnd = deadEnds[Random.Range(0, deadEnds.Count)];

        // Utiliza el algoritmo de búsqueda de caminos para encontrar el camino más largo
        Vector2 farthestDeadEnd1 = FindFarthestDeadEnd(startDeadEnd, roomOrientationMap);
        DeadEndsList.Add(farthestDeadEnd1);

        // Ordena los demás deadends según su distancia a farthestDeadEnd1
        List<Vector2> otherDeadEnds = deadEnds.Where(deadEnd => deadEnd != farthestDeadEnd1)
                                       .OrderByDescending(deadEnd => Vector2.Distance(farthestDeadEnd1, deadEnd))
                                       .ToList();
        DeadEndsList.AddRange(otherDeadEnds);

        return DeadEndsList;
    }

    // Encuentra todas las casillas deadends y guárdalas en una lista
    private List<Vector2> FindDeadEnds(int[,] roomOrientationMap)
    {
        List<Vector2> deadEnds = new List<Vector2>();

        for (int x = 0; x < roomOrientationMap.GetLength(0); x++)
        {
            for (int y = 0; y < roomOrientationMap.GetLength(1); y++)
            {
                if (roomOrientationMap[x, y] == 1 || roomOrientationMap[x, y] == 2 ||
                    roomOrientationMap[x, y] == 4 || roomOrientationMap[x, y] == 8)
                {
                    deadEnds.Add(new Vector2(x, y));
                }
            }
        }

        return deadEnds;
    }

    // Utiliza el algoritmo de búsqueda de caminos para encontrar el camino más largo
    private Vector2 FindFarthestDeadEnd(Vector2 startDeadEnd, int[,] roomOrientationMap)
    {
        // Lista de casillas deadends
        List<Vector2> deadEnds = FindDeadEnds(roomOrientationMap);

        // Si solo hay una casilla deadend, devuelve esa casilla
        if (deadEnds.Count == 1)
        {
            return startDeadEnd;
        }

        // Inicializa una variable para almacenar la casilla más lejana
        Vector2 farthestDeadEnd = startDeadEnd;

        // Inicializa una variable para almacenar la longitud del camino más largo
        float longestPathLength = 0;

        // Itera sobre todas las casillas deadend para encontrar la más lejana
        foreach (Vector2 deadEnd in deadEnds)
        {
            // Evita procesar la casilla de inicio
            if (deadEnd == startDeadEnd)
            {
                continue;
            }

            // Encuentra el camino desde startDeadEnd hasta deadEnd
            List<Vector2> path = FindPath(startDeadEnd, deadEnd, roomOrientationMap);

            // Calcula la longitud del camino
            float pathLength = CalculatePathLength(path);

            // Si el camino es más largo que el camino anteriormente encontrado,
            // actualiza farthestDeadEnd y longestPathLength
            if (pathLength > longestPathLength)
            {
                farthestDeadEnd = deadEnd;
                longestPathLength = pathLength;
            }
        }

        // Devuelve la casilla más lejana encontrada
        return farthestDeadEnd;
    }

    // Encuentra un camino desde start hasta end utilizando A*
    private List<Vector2> FindPath(Vector2 start, Vector2 end, int[,] roomOrientationMap)
    {
        List<Vector2> path = new List<Vector2>();

        // Crear nodos de inicio y objetivo
        Node startNode = new Node(start);
        Node endNode = new Node(end);

        // Conjuntos de nodos abiertos y cerrados
        HashSet<Node> openSet = new HashSet<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        // Agregar el nodo de inicio al conjunto abierto
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Obtener el nodo con el costo más bajo en el conjunto abierto
            Node currentNode = GetLowestCostNode(openSet);

            // Si el nodo actual es el nodo objetivo, reconstruir el camino y devolverlo
            if (currentNode.position == endNode.position)
            {
                path = RetracePath(startNode, currentNode);
                break;
            }

            // Mover el nodo actual del conjunto abierto al conjunto cerrado
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Obtener los vecinos del nodo actual
            List<Node> neighbors = GetNeighbors(currentNode, roomOrientationMap);

            foreach (Node neighbor in neighbors)
            {
                // Si el vecino ya está en el conjunto cerrado, saltar este vecino
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                // Calcular el nuevo costo de movimiento hasta el vecino
                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                // Si el vecino no está en el conjunto abierto o el nuevo costo de movimiento es menor que el existente
                if (!openSet.Contains(neighbor) || newMovementCostToNeighbor < neighbor.gCost)
                {
                    // Actualizar el costo y establecer el nodo padre
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    // Si el vecino no está en el conjunto abierto, agregarlo
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return path;
    }

    // Obtiene el nodo con el costo más bajo del conjunto abierto
    private Node GetLowestCostNode(HashSet<Node> nodes)
    {
        Node lowestCostNode = null;
        float lowestCost = Mathf.Infinity;

        foreach (Node node in nodes)
        {
            if (node.fCost < lowestCost)
            {
                lowestCost = node.fCost;
                lowestCostNode = node;
            }
        }

        return lowestCostNode;
    }

    // Obtiene los vecinos válidos de un nodo
    private List<Node> GetNeighbors(Node node, int[,] roomOrientationMap)
    {
        List<Node> neighbors = new List<Node>();
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in directions)
        {
            Vector2 neighborPos = node.position + dir;
            if (IsPositionValid(neighborPos, roomOrientationMap))
            {
                Node neighborNode = new Node(neighborPos);
                neighbors.Add(neighborNode);
            }
        }

        return neighbors;
    }

    // Verifica si una posición está dentro de los límites del mapa y es transitable
    private bool IsPositionValid(Vector2 position, int[,] roomOrientationMap)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        return x >= 0 && x < roomOrientationMap.GetLength(0) &&
               y >= 0 && y < roomOrientationMap.GetLength(1) &&
               roomOrientationMap[x, y] != 0;
    }

    // Calcula la distancia entre dos nodos
    private float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector2.Distance(nodeA.position, nodeB.position);
    }

    // Reconstruye el camino desde el nodo inicial hasta el nodo final
    private List<Vector2> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    // Estructura para representar un nodo en el algoritmo A*
    private class Node
    {
        public Vector2 position;
        public Node parent;
        public float gCost; // Costo acumulado desde el nodo inicial
        public float hCost; // Costo heurístico hasta el nodo objetivo

        public float fCost => gCost + hCost; // Costo total

        public Node(Vector2 position)
        {
            this.position = position;
            this.parent = null;
            this.gCost = 0;
            this.hCost = 0;
        }
    }

    // Calcula la longitud del camino
    private float CalculatePathLength(List<Vector2> path)
    {
        if (path == null || path.Count <= 1)
        {
            return 0;
        }

        float length = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            length += Vector2.Distance(path[i], path[i + 1]);
        }

        return length;
    }
}
