using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvLGenerationController : MonoBehaviour
{
    public Vector2Int MapSize;
    public int RoomQuantity;
    public int minDeadEnds;
    public int mapSeed;

    public void GenerateNewMatrix()
    {
        MatrixPseudorandomGenerator matrixGenerator = GetComponent<MatrixPseudorandomGenerator>();
        matrixGenerator.StartRoomGeneration(MapSize, RoomQuantity, minDeadEnds, mapSeed);
    }
}

