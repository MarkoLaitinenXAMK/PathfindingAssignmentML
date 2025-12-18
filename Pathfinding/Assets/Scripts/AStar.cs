using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class AStar : MonoBehaviour
{
    public GameObject tileObject;

    public int Width = 20;
    public int Height = 20;
    public float Spacing = 2.05f;

    // Set the start position
    public Vector2Int StartPos = new Vector2Int(2, 2);

    // Set the target position
    public Vector2Int TargetPos = new Vector2Int(15, 15);

    // Unvisited list of tiles
    private List<ATile> unvisited = new List<ATile>();

    // Visited list of tiles
    private List<ATile> visited = new List<ATile>();

    private bool bStart = true;

    private bool btargetFound = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject gobj = Instantiate(tileObject, new Vector3(x*Spacing, y*Spacing, 0), tileObject.transform.rotation);
                ATile tile = gobj.GetComponent<ATile>(); // Get the tile class component
                tile.Pos = new Vector2Int(x, y); // Set the tile's coordinates
                tile.ComputeHCost(TargetPos);
                tile.SetGCost(int.MaxValue); // Set the cost to max integer value
                
                if (tile.Pos.y == 6 && tile.Pos.x > 4 &&tile.Pos.x < 15)
                tile.isWalkable = false;

                if (tile.Pos.y == 13 && tile.Pos.x > 1 &&tile.Pos.x < 17)
                tile.isWalkable = false;

                if (tile.Pos == StartPos)
                {
                    tile.SetGCost(0);
                    //tile.SetColor(Color.green);
                }
                else if (tile.Pos == TargetPos)
                {
                    //tile.SetColor(Color.red);
                }

                if(tile.isWalkable)
                unvisited.Add(tile); // Add the tile to the unvisited list (all tiles!)
            }
        }
    }

    void StepAStar()
    {

        int minCost = unvisited.Min(tile => tile.GetFCost()); // Find min cost
        ATile currentTile = unvisited.Find(tile => tile.GetFCost() == minCost); // Get corresponding tile
        unvisited.Remove(currentTile); // Remove the current tile from unvisited

        for (int x = currentTile.Pos.x - 1; x <= currentTile.Pos.x + 1; x++) 
            {
                for (int y = currentTile.Pos.y - 1; y <= currentTile.Pos.y + 1; y++ )
                {
                    if (x < 0 || x > Width -1 || y < 0 || y > Height -1)
                    {
                        continue; // Skip out-of-bounds neigbor
                    }
                    // now we hav valid neighbor coordinates
                    Vector2Int neighborPos = new Vector2Int(x, y);

                    if (neighborPos == currentTile.Pos)
                    {
                        continue; // Skip the current tile
                    }

                    ATile neighbor = unvisited.Find(tile => tile.Pos == neighborPos);
                    if (neighbor == null)
                    {
                        continue; // SKip visited neighbors
                    }

                    Vector2Int delta = neighborPos -currentTile.Pos;
                    delta.x = Mathf.Abs(delta.x);
                    delta.y = Mathf.Abs(delta.y);
                    int moveCost = (delta.x + delta.y == 1) ? 10 : 14; // Orthogonal:10, Diagonal: 14

                    int costToNeighbor = currentTile.GetGCost() + moveCost;

                    // If the cost is lower than neightbor's curretn cost, update it
                    if (costToNeighbor < neighbor.GetGCost()) 
                    {
                        neighbor.SetGCost(costToNeighbor);
                        neighbor.CameFrom = currentTile.Pos;
                    }
                }
            }
        if(currentTile.Pos == TargetPos)
        {
            btargetFound = true;
        }


        if (currentTile.Pos != StartPos && currentTile.Pos != TargetPos)
        {
            currentTile.SetColor(Color.blue);
        }
        visited.Add(currentTile);
    }

    private void TraceRoute(Color _color)
    {
        ATile tile = visited.Find(t => t.Pos == TargetPos);
        if (!tile)
        return;

        while (tile.Pos != StartPos)
        {
            tile = visited.Find(t => t.Pos == tile.CameFrom);
            if (tile.Pos == StartPos)
            {
            break;
            }
            tile.SetColor(_color);
        }

    }

    IEnumerator DoAStar()
    {
        while (unvisited.Count>0 && !btargetFound)
        {
            StepAStar();
            yield return new WaitForSeconds(.0f);
        }
        TraceRoute(Color.gray);
    }

    // Update is called once per frame
    void Update()
    {
        // Highlight start and target tiles
        if (bStart)
        {
            foreach (ATile tile in unvisited) 
            {
                if (tile.Pos == StartPos) 
                {
                    tile.SetColor(Color.green);
                }
                else if (tile.Pos == TargetPos)
                {
                    tile.SetColor(Color.red);
                }                
            }

            //Dijkstra();
            //TraceRoute(Color.blue);
            StartCoroutine(DoAStar());
            bStart = false;
        }
    }
}