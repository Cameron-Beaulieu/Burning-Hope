using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Checkpoint : MonoBehaviour
{
    public TileBase unlitTile;
    public TileBase litTile;
    private Tilemap tilemap;
    public GameObject lightPrefab;
    private Dictionary<Vector2Int, bool> braziers = new Dictionary<Vector2Int, bool>();
    private Vector2Int prevLit;
    private GameObject prevLight;
    private Vector3 checkpointPos;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(prevLit);
        Debug.Log(prevLight);
        tilemap = GetComponent<Tilemap>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int loc = new Vector3Int(pos.x, pos.y, pos.z);
            if (tilemap.HasTile(loc))
            {
                braziers.Add((Vector2Int)loc, false);
                tilemap.SetTile(loc, unlitTile);
            }
        }
    }

    public Vector3 LightBrazier(Vector3 pos, bool lit)
    {
        Vector2Int localPos = (Vector2Int)tilemap.WorldToCell(pos);

        if (braziers.ContainsKey(localPos))
        {
            if (!braziers[localPos])
            {
                // Unlight previous checkpoint
                if (prevLight != null)
                {
                    braziers[prevLit] = false;
                    tilemap.SetTile((Vector3Int)prevLit, unlitTile);
                    prevLit = localPos;
                    prevLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0;
                }

                // Light new check point
                braziers[localPos] = lit;
                tilemap.SetTile((Vector3Int)localPos, litTile);
                checkpointPos = tilemap.GetCellCenterWorld((Vector3Int)localPos);
                prevLight = Instantiate(lightPrefab, checkpointPos, Quaternion.identity);
                prevLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1;
            }
        }
        return checkpointPos;
    }
}
