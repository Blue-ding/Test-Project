using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    private Tilemap tilemap;
    public List<TileBase> tileBases;


    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        int scale = (int)Camera.main.orthographicSize*2;
        Vector3Int playerPosition = new((int)GameController.instance.player.transform.position.x, (int)GameController.instance.player.transform.position.y);
        for (int i = -scale; i <= scale; i++)
        {
            if (tilemap.GetTile(new Vector3Int(i, scale) + playerPosition) == null)
            {
                ExpandMap(new Vector3Int(i, scale) + playerPosition);
            }
            if (tilemap.GetTile(new Vector3Int(scale, i) + playerPosition) == null)
            {
                ExpandMap(new Vector3Int(scale, i) + playerPosition);
            }
            if (tilemap.GetTile(new Vector3Int(i, -scale) + playerPosition) == null)
            {
                ExpandMap(new Vector3Int(i, -scale) + playerPosition);
            }
            if (tilemap.GetTile(new Vector3Int(-scale, i) + playerPosition) == null)
            {
                ExpandMap(new Vector3Int(-scale, i) + playerPosition);
            }

        }
    }

    private void ExpandMap(Vector3Int position)
    {
        tilemap.SetTile(position, tileBases[Random.Range(0, tileBases.Count)]);
        GameController.instance.SpanStillEnmey(position);
    }

}
