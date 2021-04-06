using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : TriggerEvent
{
    public int sceneToLoad;
    public override void OnTrigger()
    {
        GlobalState.LoadScene(sceneToLoad);
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnTrigger();
        }
    }
}
