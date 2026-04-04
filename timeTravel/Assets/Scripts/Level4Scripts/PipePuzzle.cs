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
        DebugConnections();
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

                if (iConnects && neighbour == -1)
                {
                    Debug.Log($"❌ Tile {i} connects toward edge in dir {dir} — invalid");
                    return;
                }

                if (iConnects != neighbourConnects)
                {
                    Debug.Log($"❌ Mismatch: Tile {i} dir {dir} = {iConnects} but neighbour {neighbour} opposite = {neighbourConnects}");
                    return;
                }
            }
        }

        Debug.Log("✅ Puzzle solved!");
        wallInteract.OnPuzzleSolved();
    }

    private void DebugConnections()
    {
        Debug.Log("--- Current Tile Connections ---");
        for (int i = 0; i < tiles.Length; i++)
        {
            bool t = tiles[i].Connects(0);
            bool r = tiles[i].Connects(1);
            bool b = tiles[i].Connects(2);
            bool l = tiles[i].Connects(3);
            Debug.Log($"Tile {i}: Top={t} Right={r} Bottom={b} Left={l}");
        }
    }
}