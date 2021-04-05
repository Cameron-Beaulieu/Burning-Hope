using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BurningRopes : MonoBehaviour
{
    public TileBase unlitTile;
    public TileBase litTile;
    private Tilemap tilemap;
    private List<BrazierGroup> brazierControllers = new List<BrazierGroup>();
    private List<Tilemap> brazierMaps = new List<Tilemap>();
    public GameObject lightPrefab;
    private Dictionary<Vector2Int, float> ropeTimers = new Dictionary<Vector2Int, float>();
    private Dictionary<Vector2Int, bool> ropeBurning = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, GameObject> ropeLights = new Dictionary<Vector2Int, GameObject>();
    private List<Vector3> braziers = new List<Vector3>();
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

        // Find all braziers
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].layer == LayerMask.NameToLayer("Braziers"))
            {
                brazierControllers.Add(objects[i].GetComponent<BrazierGroup>());
                brazierMaps.Add(objects[i].GetComponent<Tilemap>());
                foreach (var pos in brazierMaps[brazierMaps.Count - 1].cellBounds.allPositionsWithin)
                {
                    Vector3Int loc = new Vector3Int(pos.x, pos.y, pos.z);
                    if (brazierMaps[brazierMaps.Count - 1].HasTile(loc))
                    {
                        braziers.Add(brazierMaps[brazierMaps.Count - 1].CellToWorld(loc));
                    }
                }
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
                Vector3 brazierUp = tilemap.CellToWorld((Vector3Int)up);
                if (braziers.Contains(brazierUp))
                {
                    LightBrazier(brazierUp);
                }
                Vector2Int down = rope.Key + Vector2Int.down;
                LightRopeCell(down, true);
                Vector3 brazierDown = tilemap.CellToWorld((Vector3Int)down);
                if (braziers.Contains(brazierDown))
                {
                    LightBrazier(brazierDown);
                }
                Vector2Int left = rope.Key + Vector2Int.left;
                LightRopeCell(left, true);
                Vector3 brazierLeft = tilemap.CellToWorld((Vector3Int)left);
                if (braziers.Contains(brazierLeft))
                {
                    LightBrazier(brazierLeft);
                }
                Vector2Int right = rope.Key + Vector2Int.right;
                LightRopeCell(right, true);
                Vector3 brazierRight = tilemap.CellToWorld((Vector3Int)right);
                if (braziers.Contains(brazierRight))
                {
                    LightBrazier(brazierRight);
                }
                tilemap.SetTile((Vector3Int)rope.Key, null);
            }
        }
    }

    public void LightBrazier(Vector3 pos)
    {
        for (int i = 0; i < brazierControllers.Count; i++)
        {
            brazierControllers[i].LightBrazier(pos, true);
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

