using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicTetronim : MonoBehaviour
{

    [SerializeField] private float timeBefore, 
        timeDown = 0.8f;

    public static int heightGrid = 18, widthGrid=10;

    [SerializeField] private Vector3 rotationPoint;

    private static Transform[,] grid = new Transform[widthGrid,heightGrid];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!Limits())
            {
                transform.position += new Vector3(1, 0, 0);

            }
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position -= new Vector3(-1, 0, 0);
            if (!Limits())
            {
                transform.position -= new Vector3(1, 0, 0);

            }
        }


        /*tiempo caida es 1
          cada segundo baja
          
        */
        if (Time.time - timeBefore > (Input.GetKey(KeyCode.DownArrow)? timeDown/18 : timeDown))
        {
            transform.position += new Vector3(0, -1, 0);
            
            if (!Limits())
            {
                transform.position -= new Vector3(0, -1, 0);
                
                AddToGrid();
                
                this.enabled = false;
                
                FindObjectOfType<LogicSpawner>().NewTetronim();
            }

            timeBefore = Time.time;

        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);

            if (!Limits())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            }
            
        }
    }

    bool Limits()
    {
        foreach (Transform child in transform)
        {
            int X = Mathf.RoundToInt(child.transform.position.x);
            int Y = Mathf.RoundToInt(child.transform.position.y);

            if (X < 0 || X >= widthGrid || Y < 0 || Y >= heightGrid)
            {
                return false;
            }

            if (grid[X, Y] != null)
            {
                return false;
            }


        }


        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int X = Mathf.RoundToInt(child.transform.position.x);
            int Y = Mathf.RoundToInt(child.transform.position.y);

            grid[X, Y] = child;

        }
    }
    
}
