using UnityEngine;

public class PipePuzzle : MonoBehaviour
{
    [Header("References")]
    public PipeWallInteract wallInteract;

    [Header("Grid Settings")]
    public PipeTile[] tiles;
    public int gridWidth = 5;
    public int gridHeight = 5;

    private int[][] adjacency;

    private void Awake()
    {
        BuildAdjacency();
        foreach (var tile in tiles) tile.Init();
    }

    private void BuildAdjacency()
    {
        int count = gridWidth * gridHeight;
        adjacency = new int[count][];

        for (int i = 0; i < count; i++)
        {
            int row = i / gridWidth;
            int col = i % gridWidth;

            int top    = row > 0              ? i - gridWidth : -1;
            int right  = col < gridWidth - 1  ? i + 1         : -1;
            int bottom = row < gridHeight - 1 ? i + gridWidth : -1;
            int left   = col > 0              ? i - 1         : -1;

            adjacency[i] = new int[] { top, right, bottom, left };
        }
    }

    public void OnTileClicked(int tileIndex)
    {
        tiles[tileIndex].Rotate();
        CheckWin();
    }

    private void CheckWin()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int dir = 0; dir < 4; dir++)
            {
                int neighbour = adjacency[i][dir];
                int opposite  = (dir + 2) % 4;

                bool iConnects         = tiles[i].Connects(dir);
                bool neighbourConnects = neighbour >= 0 && tiles[neighbour].Connects(opposite);

                if (iConnects && neighbour == -1) return;
                if (iConnects != neighbourConnects) return;
            }
        }

        wallInteract.OnPuzzleSolved();
    }
}
