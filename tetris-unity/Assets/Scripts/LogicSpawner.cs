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
            tetronimHolded.transform.eulerAngles = new Vector3(0,0,0);
            
            tetronimHolded.transform.position = transform.position;
            tetronimHolded.GetComponent<LogicTetronim>().enabled = true;

            foreach (Transform child in tetronim.transform)
            {
                child.localRotation = Quaternion.Euler(0,0,-tetronim.transform.eulerAngles.z);

            }
            
            
            SetInBoxHoldTetronim(tetronim);

            canHoldTettronim = false;
        }
        else
        {
            SetInBoxHoldTetronim(tetronim);
            
            //TODO: rotate 
            

            NewTetronim();
        }
    }

    void SetInBoxHoldTetronim(GameObject tetronim)
    {
        tetronim.GetComponent<LogicTetronim>().enabled = false;
        tetronimHolded = tetronim;

        float eulerAnglesZ = tetronim.transform.eulerAngles.z;

        Vector3 addToCenterPosition= Vector3.zero;
        
        switch (tetronim.GetComponent<LogicTetronim>().nameTetronim)
        {
            case "I":
                tetronim.transform.RotateAround(tetronim.transform.TransformPoint(tetronim.GetComponent<LogicTetronim>().rotationPoint),new Vector3(0,0,1),-eulerAnglesZ+90); 
                addToCenterPosition += new Vector3(0.5f,0,0);
                break;
            case "J":
                tetronim.transform.RotateAround(tetronim.transform.TransformPoint(tetronim.GetComponent<LogicTetronim>().rotationPoint),new Vector3(0,0,1),-eulerAnglesZ-90); 
                addToCenterPosition += new Vector3(-1,-0.5f,0);
                break;
            case "L":
                tetronim.transform.RotateAround(tetronim.transform.TransformPoint(tetronim.GetComponent<LogicTetronim>().rotationPoint),new Vector3(0,0,1),-eulerAnglesZ+90); 
                addToCenterPosition += new Vector3(1,-0.5f,0);

                break;
            case "O":
                tetronim.transform.RotateAround(tetronim.transform.TransformPoint(tetronim.GetComponent<LogicTetronim>().rotationPoint),new Vector3(0,0,1),-eulerAnglesZ+90); 
                addToCenterPosition += new Vector3(0.5f,-0.5f,0);
                break;
            case "S":
                tetronim.transform.RotateAround(tetronim.transform.TransformPoint(tetronim.GetComponent<LogicTetronim>().rotationPoint),new Vector3(0,0,1),-eulerAnglesZ); 
                addToCenterPosition += new Vector3(0,-0.5f,0);
                break;
            case "T":
                tetronim.transform.RotateAround(tetronim.transform.TransformPoint(tetronim.GetComponent<LogicTetronim>().rotationPoint),new Vector3(0,0,1),-eulerAnglesZ); 
                addToCenterPosition += new Vector3(0,-0.5f,0);
                break;
            case "Z":
                tetronim.transform.RotateAround(tetronim.transform.TransformPoint(tetronim.GetComponent<LogicTetronim>().rotationPoint),new Vector3(0,0,1),-eulerAnglesZ); 
                addToCenterPosition += new Vector3(0,-0.5f,0);
                break;

        }

        tetronim.transform.position = parentHold.position+ addToCenterPosition;
        
        
        foreach (Transform child in tetronim.transform)
        {
            child.localRotation = Quaternion.Euler(0,0,-tetronim.transform.eulerAngles.z);

        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
