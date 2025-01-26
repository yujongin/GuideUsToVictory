using UnityEngine;

public class BlockPlacementAI : MonoBehaviour
{
    private int[,] grid = new int[10, 10]; // 10x10 그리드
    private int[][][] tetrisBlocks = new int[][][] // 테트리스 블록 정의
    {
        new int[][] { new int[] {1, 1, 1, 1} },                     // I 블록
        new int[][] { new int[] {1, 1}, new int[] {1, 1} },         // O 블록
        new int[][] { new int[] {0, 1, 0}, new int[] {1, 1, 1} },   // T 블록
        new int[][] { new int[] {1, 1, 0}, new int[] {0, 1, 1} },   // S 블록
        new int[][] { new int[] {0, 1, 1}, new int[] {1, 1, 0} }    // Z 블록
    };

    void Start()
    {
        PlaceTetrisBlocks();
    }

    void PlaceTetrisBlocks()
    {
        // 랜덤하게 블록을 배치
        for (int i = 0; i < 10; i++) // 최대 10개의 블록
        {
            int[][] block = tetrisBlocks[Random.Range(0, tetrisBlocks.Length)];
            (int[][] bestBlock, int bestX, int bestY) = FindBestPosition(grid, block);

            if (bestBlock != null)
            {
                PlaceBlock(grid, bestBlock, bestX, bestY);
                PrintGrid(grid);
            }
        }
    }

    (int[][], int, int) FindBestPosition(int[,] grid, int[][] block)
    {
        int bestScore = int.MaxValue;
        int[][] bestBlock = null;
        int bestX = -1, bestY = -1;

        // 블록 회전 및 가능한 모든 위치 테스트
        for (int rotation = 0; rotation < 4; rotation++)
        {
            int[][] rotatedBlock = RotateBlock(block, rotation);

            for (int x = 0; x <= grid.GetLength(0) - rotatedBlock.Length; x++)
            {
                for (int y = 0; y <= grid.GetLength(1) - rotatedBlock[0].Length; y++)
                {
                    if (CanPlace(grid, rotatedBlock, x, y))
                    {
                        int[,] testGrid = (int[,])grid.Clone();
                        PlaceBlock(testGrid, rotatedBlock, x, y);
                        int score = EvaluateGrid(testGrid);

                        if (score < bestScore)
                        {
                            bestScore = score;
                            bestBlock = rotatedBlock;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }
            }
        }

        return (bestBlock, bestX, bestY);
    }

    int[][] RotateBlock(int[][] block, int rotations)
    {
        int[][] rotated = block;
        for (int i = 0; i < rotations; i++)
        {
            rotated = Rotate90(rotated);
        }
        return rotated;
    }

    int[][] Rotate90(int[][] block)
    {
        int rows = block.Length;
        int cols = block[0].Length;
        int[][] rotated = new int[cols][];

        for (int i = 0; i < cols; i++)
        {
            rotated[i] = new int[rows];
            for (int j = 0; j < rows; j++)
            {
                rotated[i][j] = block[rows - 1 - j][i];
            }
        }

        return rotated;
    }

    bool CanPlace(int[,] grid, int[][] block, int x, int y)
    {
        for (int i = 0; i < block.Length; i++)
        {
            for (int j = 0; j < block[i].Length; j++)
            {
                if (block[i][j] == 1)
                {
                    if (x + i >= grid.GetLength(0) || y + j >= grid.GetLength(1) || grid[x + i, y + j] == 1)
                        return false;
                }
            }
        }
        return true;
    }

    void PlaceBlock(int[,] grid, int[][] block, int x, int y)
    {
        for (int i = 0; i < block.Length; i++)
        {
            for (int j = 0; j < block[i].Length; j++)
            {
                if (block[i][j] == 1)
                {
                    grid[x + i, y + j] = 1;
                }
            }
        }
    }

    int EvaluateGrid(int[,] grid)
    {
        int emptySpaces = 0;
        int heightDeviation = 0;

        int[] columnHeights = new int[grid.GetLength(1)];

        // 빈 공간 계산
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            bool foundBlock = false;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                if (grid[i, j] == 1)
                {
                    foundBlock = true;
                    columnHeights[j] = grid.GetLength(0) - i;
                    break;
                }
            }

            if (!foundBlock)
            {
                columnHeights[j] = 0;
            }
        }

        // 높이 편차 계산
        int maxHeight = Mathf.Max(columnHeights);
        int minHeight = Mathf.Min(columnHeights);
        heightDeviation = maxHeight - minHeight;

        // 빈 공간 계산
        foreach (int cell in grid)
        {
            if (cell == 0) emptySpaces++;
        }

        return emptySpaces + heightDeviation * 2; // 빈 공간과 높이 편차를 동시에 고려
    }

    void PrintGrid(int[,] grid)
    {
        string output = "";
        output += "\n";
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                output += grid[i, j] + " ";
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
