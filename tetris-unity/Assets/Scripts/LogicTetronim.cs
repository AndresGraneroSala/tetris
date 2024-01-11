using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicTetronim : MonoBehaviour
{

    [SerializeField] private float timeBefore, 
        timeDown = 0.8f;

    public static int heightGrid = 20, widthGrid=10;

    [SerializeField] private Vector3 rotationPoint;

    private static Transform[,] grid = new Transform[widthGrid,heightGrid];

    public static int score=0;
    public static int level=0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        UpLevel();
        UpDifficult();
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
        if (Time.time - timeBefore > (Input.GetKey(KeyCode.DownArrow)? timeDown/20 : timeDown))
        {
            transform.position += new Vector3(0, -1, 0);
            
            if (!Limits())
            {
                transform.position -= new Vector3(0, -1, 0);
                
                AddToGrid();
                ReviewLines();
                

                
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (Limits())
            {
                transform.position += new Vector3(0, -1, 0);
            }
            transform.position -= new Vector3(0, -1, 0);

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            FindObjectOfType<LogicSpawner>().HoldTetronim(gameObject);
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

            if (Y >= 19)
            {
                score = 0;
                level = 0;
                timeDown = 0.8f;
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
    }


    void ReviewLines()
    {
        for (int i = heightGrid - 1; i >= 0; i--)
        {
            if (HaveLine(i))
            {
                UpLevel();
                UpDifficult();
                
                DeleteLine(i);
                DownLine(i);
            }
        }
    }





    private bool HaveLine(int i)
    {
        for (int j = 0; j < widthGrid; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        score += 100;
        Debug.Log(score);
        
        return true;
    }
    
    
    private void DeleteLine(int i)
    {
        for (int j = 0; j < widthGrid; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j, i] = null;
        }
    }
    
    private void DownLine(int i)
    {
        for (int y = i; y < heightGrid; y++)
        {
            for (int j = 0; j < widthGrid; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y]= null;
                    grid[j, y-1].transform.position-= new Vector3(0,1,0);
                }
            }
        }
    }


    void UpLevel()
    {
        switch (score)
        {
            case 200: level = 1;
                break;
            
            case 400: level = 4;
                break;
            
           
        }

        GameObject.Find("ScoreNum").GetComponent<Text>().text = score.ToString();

    }

    void UpDifficult()
    {
        switch (level)
        {
            case 1: timeDown = 0.4f;
                break;
            case 2: timeDown = 0.2f;
                break;
        }
        
        GameObject.Find("LevelNum").GetComponent<Text>().text = level.ToString();

    }
}
