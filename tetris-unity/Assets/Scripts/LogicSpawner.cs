using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LogicSpawner : MonoBehaviour
{

    public GameObject[] tetronims;

    public Transform parentHold;
    private bool canHoldTettronim;

    private GameObject tetronimHolded;
    
    // Start is called before the first frame update
    void Start()
    {
        NewTetronim();
    }

    public void NewTetronim()
    {
        GameObject tetronim= Instantiate(tetronims[Random.Range(0, tetronims.Length)]);
        tetronim.transform.position = transform.position;
        canHoldTettronim = true;
    }

    public void HoldTetronim(GameObject tetronim)
    {
        if (!canHoldTettronim)
        {
            return;
        }

        if (tetronimHolded != null)
        {
            tetronimHolded.transform.position = transform.position;
            tetronimHolded.GetComponent<LogicTetronim>().enabled = true;

            tetronim.GetComponent<LogicTetronim>().enabled = false;
            tetronim.transform.position = parentHold.position;
            tetronimHolded = tetronim;
            canHoldTettronim = false;
        }
        else
        {
            tetronim.GetComponent<LogicTetronim>().enabled = false;
            tetronim.transform.position = parentHold.position;
            tetronimHolded = tetronim;

            NewTetronim();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
