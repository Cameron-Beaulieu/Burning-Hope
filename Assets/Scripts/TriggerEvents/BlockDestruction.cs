using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockDestruction : TriggerEvent
{
    public override void OnTrigger()
    {
        Debug.Log("Trigger");
        Tilemap tilemap = GetComponent<Tilemap>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int loc = new Vector3Int(pos.x, pos.y, pos.z);
            if (tilemap.HasTile(loc))
            {
                tilemap.SetTile(loc, null);
            }
        }
    }
}
