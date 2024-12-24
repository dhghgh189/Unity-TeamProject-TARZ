using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BackTracking 알고리즘
/// 1. 시작점 선택
/// </summary>
public class DungeonGenerator_DFS : MonoBehaviour
{
    public Vector2 size; // 맵 전체 사이즈 NxN 사이즈

    public int startPos; // 원점 시작

    public GameObject room;

    public Vector2 offset; // 방 사이 거리

    public int RoomCount;

    List<Node> NodeList;

    public class Node
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    private void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Node currentCell = NodeList[Mathf.FloorToInt(i + j * size.x)];
              
                if (currentCell.visited)
                {
                    var newRoom = Instantiate(room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);

                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }
    void MazeGenerator()
    {
        NodeList = new List<Node>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                NodeList.Add(new Node());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;  


        while (k < RoomCount)
        {
            k++;

            NodeList[currentCell].visited = true;

            if (currentCell == NodeList.Count - 1)
            {
                break;
            }

            // 셀의 이웃을 확인
            List<int> neighbors = CheckNeighbors(currentCell);

            if ((neighbors.Count) == 0)
            {
                if ((path.Count) == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or R
                    if (newCell - 1 == currentCell)
                    {
                        NodeList[currentCell].status[2] = true;
                        currentCell = newCell;
                        NodeList[currentCell].status[3] = true;    
                    }
                    else
                    {
                        NodeList[currentCell].status[1] = true;
                        currentCell = newCell;
                        NodeList[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or L
                    if (newCell + 1 == currentCell)
                    {
                        NodeList[currentCell].status[3] = true;
                        currentCell = newCell;
                        NodeList[currentCell].status[2] = true;
                    }
                    else
                    {
                        NodeList[currentCell].status[0] = true;
                        currentCell = newCell;
                        NodeList[currentCell].status[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // Up 이웃
        if (cell - size.x >= 0 && !NodeList[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }
        // Down 이웃
        if (cell + size.x < NodeList.Count && !NodeList[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }
        // Right 이웃
        if ((cell + 1) % size.x != 0 && !NodeList[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }
        // Left 이웃
        if (cell % size.x != 0 && !NodeList[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }
}
