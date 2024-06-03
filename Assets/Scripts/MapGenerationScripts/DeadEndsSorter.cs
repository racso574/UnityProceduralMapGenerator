using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadEndsSorter : MonoBehaviour
{
    private List<Vector2> DeadEndsList;

    public List<Vector2> GetSortedDeadEndsList(int[,] roomOrientationMap, int mapSeed)
    {
        DeadEndsList = new List<Vector2>();
        Random.InitState(mapSeed);

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
                                       .OrderByDescending(deadEnd => GetDistance(farthestDeadEnd1, deadEnd, roomOrientationMap))
                                       .ToList();

        DeadEndsList.AddRange(otherDeadEnds);

        // Debug: Imprimir las distancias de cada deadEnd a farthestDeadEnd1
        foreach (Vector2 deadEnd in otherDeadEnds)
        {
            Debug.Log($"Distancia de {deadEnd} a {farthestDeadEnd1}: {GetDistance(farthestDeadEnd1, deadEnd, roomOrientationMap)}");
        }

        return DeadEndsList;
    }

    private List<Vector2> FindDeadEnds(int[,] map)
    {
        List<Vector2> deadEnds = new List<Vector2>();
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (IsDeadEnd(map, x, y))
                {
                    deadEnds.Add(new Vector2(x, y));
                }
            }
        }

        return deadEnds;
    }

    private bool IsDeadEnd(int[,] map, int x, int y)
    {
        if (map[x, y] == 0) return false; // No es una habitación
        int connections = 0;
        if (x > 0 && map[x - 1, y] != 0) connections++;
        if (x < map.GetLength(0) - 1 && map[x + 1, y] != 0) connections++;
        if (y > 0 && map[x, y - 1] != 0) connections++;
        if (y < map.GetLength(1) - 1 && map[x, y + 1] != 0) connections++;
        return connections == 1; // Dead end si solo tiene una conexión
    }

    private Vector2 FindFarthestDeadEnd(Vector2 start, int[,] map)
    {
        Vector2 farthest = start;
        int maxDistance = 0;

        Queue<Node> queue = new Queue<Node>();
        HashSet<Vector2> visited = new HashSet<Vector2>();

        queue.Enqueue(new Node(start, 0));
        visited.Add(start);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current.Distance > maxDistance)
            {
                maxDistance = current.Distance;
                farthest = current.Position;
            }

            foreach (Vector2 neighbor in GetNeighbors(current.Position, map))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(new Node(neighbor, current.Distance + 1));
                }
            }
        }

        return farthest;
    }

    private int GetDistance(Vector2 start, Vector2 end, int[,] map)
    {
        Queue<Node> queue = new Queue<Node>();
        HashSet<Vector2> visited = new HashSet<Vector2>();

        queue.Enqueue(new Node(start, 0));
        visited.Add(start);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current.Position == end)
            {
                return current.Distance;
            }

            foreach (Vector2 neighbor in GetNeighbors(current.Position, map))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(new Node(neighbor, current.Distance + 1));
                }
            }
        }

        return int.MaxValue; // No debería llegar aquí si end es alcanzable
    }

    private List<Vector2> GetNeighbors(Vector2 position, int[,] map)
    {
        List<Vector2> neighbors = new List<Vector2>();
        int x = (int)position.x;
        int y = (int)position.y;

        if (x > 0 && map[x - 1, y] != 0) neighbors.Add(new Vector2(x - 1, y));
        if (x < map.GetLength(0) - 1 && map[x + 1, y] != 0) neighbors.Add(new Vector2(x + 1, y));
        if (y > 0 && map[x, y - 1] != 0) neighbors.Add(new Vector2(x, y - 1));
        if (y < map.GetLength(1) - 1 && map[x, y + 1] != 0) neighbors.Add(new Vector2(x, y + 1));

        return neighbors;
    }

    private class Node
    {
        public Vector2 Position { get; }
        public int Distance { get; }

        public Node(Vector2 position, int distance)
        {
            Position = position;
            Distance = distance;
        }
    }
}
