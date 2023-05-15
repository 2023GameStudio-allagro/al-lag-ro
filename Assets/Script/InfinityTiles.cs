using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfinityTiles : MonoBehaviour
{
    private int left, right, top, bottom, width, height;
    private Vector2 positionOffset;
    private Tilemap tilemap;
    public GameObject followee;


    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        positionOffset = followee.transform.position;
        BoundsInt bound = tilemap.cellBounds;
        left = bound.xMin;
        right = bound.xMax;
        top = bound.yMax;
        bottom = bound.yMin;
        width = bound.size.x;
        height = bound.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = followee.transform.position;
        Vector2 offset = newPosition - positionOffset;
        if(offset.x > 1f)
        {
            positionOffset = new Vector2(positionOffset.x+1f, positionOffset.y);
            ShiftTileHorizontal(1);
        }
        else if(offset.x < -1f)
        {
            positionOffset = new Vector2(positionOffset.x-1f, positionOffset.y);
            ShiftTileHorizontal(-1);
        }
        if(offset.y > 1f)
        {
            positionOffset = new Vector2(positionOffset.x, positionOffset.y+1f);
            ShiftTileVertical(1);
        }
        else if(offset.y < -1f)
        {
            positionOffset = new Vector2(positionOffset.x, positionOffset.y-1f);
            ShiftTileVertical(-1);
        }
    }

    void ShiftTileHorizontal(int dx)
    {
        Vector3Int[] positions = new Vector3Int[height];
        TileBase[] tiles = new TileBase[height];
        int index = 0;
        int from = dx < 0 ? (right-1) : left;
        int to = dx < 0 ? (left-1) : right;
        for(int y=bottom; y<top; y++)
        {
            Vector3Int oldTilePos = new Vector3Int(from, y, 0);
            positions[index] = new Vector3Int(to, y, 0);
            tiles[index] = tilemap.GetTile(oldTilePos);
            tilemap.SetTile(oldTilePos, null);
            index++;
        }
        tilemap.SetTiles(positions, tiles);
        left += dx;
        right += dx;
    }
    void ShiftTileVertical(int dy)
    {
        Vector3Int[] positions = new Vector3Int[width];
        TileBase[] tiles = new TileBase[width];
        int index = 0;
        int from = dy < 0 ? (top-1) : bottom;
        int to = dy < 0 ? (bottom-1) : (top);
        for(int x=left; x<right; x++)
        {
            Vector3Int oldTilePos = new Vector3Int(x, from, 0);
            positions[index] = new Vector3Int(x, to, 0);
            tiles[index] = tilemap.GetTile(oldTilePos);
            tilemap.SetTile(oldTilePos, null);
            index++;
        }
        tilemap.SetTiles(positions, tiles);
        top += dy;
        bottom += dy;
    }
}
