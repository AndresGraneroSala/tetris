using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LogicSpawner : MonoBehaviour
{

    public GameObject[] tetronims;

    public Transform parentHold;
    private bool _canHoldTettronim;

    private GameObject _tetronimHolded;

    [SerializeField] private Transform[] boxNext;
    private GameObject[] tetronimNext;
    
    // Start is called before the first frame update
    void Start()
    {

        tetronimNext = new GameObject[boxNext.Length];

        for (int i = 0; i < tetronimNext.Length; i++)
        {
            GameObject tetronim = GenerateTetronim();
            tetronimNext[i] = tetronim;
            SetInBoxHoldTetronim(tetronim, boxNext[i]);
        }
        
        NewTetronim();

    }

    GameObject GenerateTetronim()
    {
        GameObject tetronim= Instantiate(tetronims[Random.Range(0, tetronims.Length)]);
        tetronim.GetComponent<LogicTetronim>().enabled = false;

        return tetronim;
    }
    
    public void NewTetronim()
    {
        GameObject tetronim = GenerateTetronim();
        tetronimNext[0].transform.position = transform.position;
        tetronimNext[0].GetComponent<LogicTetronim>().enabled = true;
        
        for (int i = 1; i < tetronimNext.Length; i++)
        {
            tetronimNext[i - 1] = tetronimNext[i];
        }
        tetronimNext[tetronimNext.Length - 1] = tetronim;

        for (int i = 0; i < tetronimNext.Length; i++)
        {
            SetInBoxHoldTetronim(tetronimNext[i],boxNext[i]);
        }
        
        
        _canHoldTettronim = true;
    }

    public void HoldTetronim(GameObject tetronim)
    {
        if (!_canHoldTettronim)
        {
            return;
        }

        if (_tetronimHolded != null)
        {
            _tetronimHolded.transform.eulerAngles = new Vector3(0,0,0);
            
            _tetronimHolded.transform.position = transform.position;
            _tetronimHolded.GetComponent<LogicTetronim>().enabled = true;

            
            tetronim.GetComponent<LogicTetronim>().UpdateRotationChilds();
            
            SetInBoxHoldTetronim(tetronim,parentHold);
            _tetronimHolded = tetronim;
            
            _canHoldTettronim = false;
        }
        else
        {
            SetInBoxHoldTetronim(tetronim,parentHold);
            _tetronimHolded = tetronim;
            
            

            NewTetronim();
        }
    }

    void SetInBoxHoldTetronim(GameObject tetronim,Transform box)
    {
        tetronim.GetComponent<LogicTetronim>().enabled = false;
        

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

        tetronim.transform.position = box.position+ addToCenterPosition;
        
        tetronim.GetComponent<LogicTetronim>().UpdateRotationChilds();
        

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
