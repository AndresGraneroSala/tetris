using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LogicTetronim : MonoBehaviour
{

    [SerializeField] private float timeBefore, 
        timeDown = 1.25f;

    public static int heightGrid = 20, widthGrid=10;

    public Vector3 rotationPoint;

    private static Transform[,] grid = new Transform[widthGrid,heightGrid];

    public static int score=0;
    public static int level=1;

    [FormerlySerializedAs("name")] public string nameTetronim;

    [SerializeField] private Sprite spriteGhostPiece;
    [SerializeField] static GameObject ghostPiece;
    
    // Start is called before the first frame update
    void Start()
    {
        UpLevel();
        UpDifficult();
        SetGhostInGrid();
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
                TryDestroyGhost();

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

            UpdateRotationChilds();
            
            if (!Limits())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
                
                foreach (Transform child in transform)
                {
                    child.localRotation = Quaternion.Euler(0, 0, -transform.eulerAngles.z);

                }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryDestroyGhost();

            
            while (Limits())
            {
                transform.position += new Vector3(0, -1, 0);
            }
            transform.position -= new Vector3(0, -1, 0);

            AddToGrid();
            ReviewLines();
            FindObjectOfType<LogicSpawner>().NewTetronim();
            

            
            this.enabled = false;
            
        }

        if (Input.GetKeyDown(KeyCode.C) && !Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<LogicSpawner>().HoldTetronim(gameObject);
        }

        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.DownArrow)&& !Input.GetKeyDown(KeyCode.C) )
        {
            SetGhostInGrid();
        }

    }

    public void UpdateRotationChilds()
    {
        foreach (Transform child in transform)
        {
            child.localRotation = Quaternion.Euler(0, 0, -transform.eulerAngles.z);

        }
    }

    public bool Limits()
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


            if (Y >= 19)
            {
                score = 0;
                level = 0;
                timeDown = 0.8f;
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                
                return;
            }
            grid[X, Y] = child;

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

        score += 100*(level);
        //Debug.Log(score);
        
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
            
            case >=25000:
                level = 9;
                break;
            
            case >=15000:
                level = 8;
                break;
            
            case >=10000:
                level = 7;
                break;
            
            case >=5000:
                level = 6;
                break;

            case >=2600:
                level = 5;
                break;

            case >=2000:
                level = 4;
                break;

            case >=1000:
                level = 3;
                break;

            case >=500:
                level = 2;
                break;
            case 0: level = 1;
                break;
            
        }

        GameObject.Find("ScoreNum").GetComponent<Text>().text = score.ToString();

    }

    void UpDifficult()
    {
        switch (level)
        {
            case 2: timeDown = 1f;
                break;
            case 3: timeDown = 0.9f;
                break;
            case 4: timeDown = 0.8f;
                break;
            case 5: timeDown = 0.7f;
                break;
            case 6: timeDown = 0.65f;
                break;
            case 7: timeDown = 0.6f;
                break;
            case 8: timeDown = 0.55f;
                break;
            case 9: timeDown = 0.5f;
                break;
            
        }
        
        GameObject.Find("LevelNum").GetComponent<Text>().text = level.ToString();

    }

    public void TryDestroyGhost()
    {
        if (ghostPiece != null)
        {
            Destroy(ghostPiece);
        }

    }
    
    public void SetGhostInGrid()
    {
    
        //print("set ghost");
        
        TryDestroyGhost();

        ghostPiece = Instantiate(gameObject);
        ghostPiece.GetComponent<LogicTetronim>().enabled = false;

        ghostPiece.transform.position += new Vector3(0,-1,0);

        /*switch (nameTetronim)
        {
            case "I":
                ghostPiece.transform.position += new Vector3(0,-1,0);
                break;
            case "J":
            case "L":
                ghostPiece.transform.position += new Vector3(0,-1,0);
                break;
        }*/
        
        while (ghostPiece.GetComponent<LogicTetronim>().Limits())
        {
            ghostPiece.transform.position += new Vector3(0, -1, 0);
        }
        ghostPiece.transform.position -= new Vector3(0, -1, 0);

        foreach (Transform child in ghostPiece.transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = spriteGhostPiece;
            child.GetComponent<SpriteRenderer>().sortingOrder -= 1;
        }
        
    }
    
}
