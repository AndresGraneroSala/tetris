using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LogicSpawner : MonoBehaviour
{

    public GameObject[] tetronims;
    
    // Start is called before the first frame update
    void Start()
    {
        NewTetronim();
    }

    public void NewTetronim()
    {
        GameObject tetronim= Instantiate(tetronims[Random.Range(0, tetronims.Length)]);
        tetronim.transform.position = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
