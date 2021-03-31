using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrazierGroup : MonoBehaviour
{
    public TileBase unlitTile;
    public TileBase litTile;
    private Tilemap tilemap;
    public GameObject lightPrefab;
    private Dictionary<Vector2Int, bool> braziers = new Dictionary<Vector2Int, bool>();
    public bool triggerActive;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        // activate trigger when all braziers are lit
        if (!triggerActive)
        {
            triggerActive = true;
            foreach (KeyValuePair<Vector2Int, bool> brazier in braziers)
            {
                if (!brazier.Value)
                {
                    triggerActive = false;
                }
            }
        }
    }

    public void LightBrazier(Vector3 pos, bool lit)
    {
        Vector2Int localPos = (Vector2Int)tilemap.WorldToCell(pos);

        if (braziers.ContainsKey(localPos))
        {
            if (!braziers[localPos])
            {
                braziers[localPos] = lit;
                tilemap.SetTile((Vector3Int)localPos, litTile);
                Vector3 worldPos = tilemap.GetCellCenterWorld((Vector3Int)localPos);
                GameObject light = Instantiate(lightPrefab, worldPos, Quaternion.identity);
                light.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1;
            }
        }
    }
}
