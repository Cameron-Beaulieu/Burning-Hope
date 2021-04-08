using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : TriggerEvent
{
    public int sceneToLoad;
    private GameObject splits;

    //Called on object init
    public void Start()
    {
        splits = GameObject.Find("Splits");
    }

    public override void OnTrigger()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                GlobalState.CaveTime = splits.GetComponent<Splits>().getTime();
                break;
            case 4:
                GlobalState.MineTime = splits.GetComponent<Splits>().getTime();
                break;
            case 6:
                GlobalState.CultTime = splits.GetComponent<Splits>().getTime();
                break;
            default:
                break;
        }

        if (splits != null)
        {
            GlobalState.Time += splits.GetComponent<Splits>().getTime();
        }
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
