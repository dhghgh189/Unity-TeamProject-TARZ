using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BackTracking 알고리즘 (BFS 사용)
/// 1. 시작점 선택
/// </summary>
public class DungeonGenerator_BFS : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    public Vector2 size;
    public int startPos = 0;
    public GameObject room;
    public Vector2 offset; // 방 사이 거리

    List<Cell> board;

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
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];

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
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        Queue<int> queue = new Queue<int>();
        queue.Enqueue(startPos);
        board[startPos].visited = true;

        while (queue.Count > 0)
        {
            int currentCell = queue.Dequeue();

            List<int> neighbors = CheckNeighbors(currentCell);
            foreach (int neighbor in neighbors)
            {
                if (!board[neighbor].visited)
                {
                    board[neighbor].visited = true;
                    queue.Enqueue(neighbor);

                    // 방향 설정
                    if (neighbor > currentCell)
                    {
                        // Down or Right
                        if (neighbor - 1 == currentCell)
                        {
                            board[currentCell].status[2] = true;
                            board[neighbor].status[3] = true;
                        }
                        else
                        {
                            board[currentCell].status[1] = true;
                            board[neighbor].status[0] = true;
                        }
                    }
                    else
                    {
                        // Up or Left
                        if (neighbor + 1 == currentCell)
                        {
                            board[currentCell].status[3] = true;
                            board[neighbor].status[2] = true;
                        }
                        else
                        {
                            board[currentCell].status[0] = true;
                            board[neighbor].status[1] = true;
                        }
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
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }
        // Down 이웃
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }
        // Right 이웃
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }
        // Left 이웃
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }
}
