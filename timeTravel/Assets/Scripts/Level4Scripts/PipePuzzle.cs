using UnityEngine;

public class PipePuzzle : MonoBehaviour
{
    [Header("References")]
    public PipeWallInteract wallInteract; //called when puzzle is successfully solved

    [Header("Grid Settings")]
    public PipeTile[] tiles; //all tiles in the grid, ordered from left to right, top to bottom, top left tile is tile 0
    public int gridWidth = 5;
    public int gridHeight = 5;

    // adjacency[i] = { topIndex, rightIndex, bottomIndex, leftIndex }
    //-1 equals off edge of grid
    private int[][] adjacency;

    private void Awake()
    {
        BuildAdjacency(); //calculates all neighbours before tile logic runs
        foreach (var tile in tiles) tile.Init();
    }

    //build the adjacencies for each tile
    //each tile has 4 neighbour indices": top, right, bottom, left
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

    //called by each PipeTile upon being clicked
    public void OnTileClicked(int tileIndex)
    {
        tiles[tileIndex].Rotate();
        DebugConnections(); //logs state of all tiles in the grid after the action
        CheckWin();
    }


    //failure conditions:
    // 1. tile connects toward the edge of the grid
    // 2. tile connects in a direction but not the same as its neighbour
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

                //all pipes connecting off edge of grid invalid
                if (iConnects && neighbour == -1)
                {
                    Debug.Log($"❌ Tile {i} connects toward edge in dir {dir} — invalid");
                    return;
                }

                //if one side connects,the other must match
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

    //used for debugging, making sure puzzle actually closes when solved. 
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