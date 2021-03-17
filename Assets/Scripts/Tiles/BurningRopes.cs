using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BurningRopes : MonoBehaviour
{
    public TileBase unlitTile;
    public TileBase litTile;
    private Tilemap tilemap;
    public GameObject lightPrefab;
    private Dictionary<Vector2Int, float> ropeTimers = new Dictionary<Vector2Int, float>();
    private Dictionary<Vector2Int, bool> ropeBurning = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, GameObject> ropeLights = new Dictionary<Vector2Int, GameObject>();
    public float burnTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int loc = new Vector3Int(pos.x, pos.y, pos.z);
            if (tilemap.HasTile(loc))
            {
                ropeTimers.Add((Vector2Int)loc, burnTime);
                ropeBurning.Add((Vector2Int)loc, false);
                tilemap.SetTile(loc, unlitTile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // burn ropes
        foreach (KeyValuePair<Vector2Int, bool> rope in ropeBurning)
        {
            if (rope.Value)
            {
                ropeTimers[rope.Key] -= Time.deltaTime;
            }
            if (ropeTimers[rope.Key] <= 0)
            {
                ropeTimers.Remove(rope.Key);
                ropeBurning.Remove(rope.Key);
                Destroy(ropeLights[rope.Key]);
                ropeLights.Remove(rope.Key);
                // burn surrounding ropes
                Vector2Int up = rope.Key + Vector2Int.up;
                LightRopeCell(up, true);
                Vector2Int down = rope.Key + Vector2Int.down;
                LightRopeCell(down, true);
                Vector2Int left = rope.Key + Vector2Int.left;
                LightRopeCell(left, true);
                Vector2Int right = rope.Key + Vector2Int.right;
                LightRopeCell(right, true);
                tilemap.SetTile((Vector3Int)rope.Key, null);
            }
        }
    }

    public void LightRope(Vector3 pos, bool lit)
    {
        Vector2Int localPos = (Vector2Int)tilemap.WorldToCell(pos);

        LightRopeCell(localPos, lit);
    }

    public void LightRopeCell(Vector2Int localPos, bool lit)
    {
        if (ropeBurning.ContainsKey(localPos))
        {
            if (!ropeBurning[localPos])
            {
                ropeBurning[localPos] = lit;
                Vector3 worldPos = tilemap.GetCellCenterWorld((Vector3Int)localPos);
                ropeLights[localPos] = Instantiate(lightPrefab, worldPos, Quaternion.identity);
                ropeLights[localPos].GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1;
                tilemap.SetTile((Vector3Int)localPos, litTile);
            }
        }
    }
}

