using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    #region - stuff is here
    public static GridManager Instance;

    public GameObject polygon;
    [HideInInspector]
    public Vector2 startingPos;
    public float shiftx = 2;
    public float shifty = 4;
    [SerializeField]
    Vector2 position;
    bool hasRan = false;
    public int rowNum;

    public List<Poly> polys = new List<Poly>();

    public List<Vector3> t_Pos = new List<Vector3>();
    public List<Vector3> e_pos = new List<Vector3>();

    public List<Poly> selectedPolys = new List<Poly>();
    public int numOfSelectedPolys = 0;

    public List<Poly> adjacentPolys = new List<Poly>();

    public bool baseSelected = false;

    public int numStorer = 0;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPos = new Vector2(.5f,.5f);
        for (int i = 0; i < 2; i++)
        {
            for (int y = 0; y < rowNum; y++)
            {
                position = startingPos;
                for (int x = 0; x < rowNum; x++)
                {
                    position = new Vector2(position.x + (x * shiftx), position.y + (y * shifty));
                    Poly theCurrentBoi = Instantiate(polygon, position, Quaternion.Euler(0, 0, 0)).GetComponent<Poly>();
                    theCurrentBoi.t_Rand();
                    polys.Add(theCurrentBoi);

                    position = startingPos;
                }
            }

            startingPos = new Vector2(1.5f, 1.5f);
            rowNum--;
        }

        for(int i = 0; i < 3; i++)
        {
            if (GetPolyAtPos(t_Pos[i]) != null) GetPolyAtPos(t_Pos[i]).t_Red();
        }

        for(int i = 0; i < 3; i++)
        {
            if (GetPolyAtPos(e_pos[i]) != null) GetPolyAtPos(e_pos[i]).t_Blue();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) == true)
        {
            Debug.Log("Click!!");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Poly tempPoly = hit.collider.GetComponent<Poly>();
                if (!baseSelected)
                {
                    if (tempPoly.fullRed == true)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        selectedPolys.Add(hit.collider.GetComponent<Poly>());
                        baseSelected = true;
                    }
                    else
                    {
                        Debug.Log("must start on a full red");
                    }
                }
                else if (baseSelected)
                {
                    if (tempPoly.hasRed == true)
                    {
                        selectedPolys.Add(hit.collider.GetComponent<Poly>());
                        foreach (Poly n in selectedPolys)
                        {
                            numOfSelectedPolys++;
                        }
                        numOfSelectedPolys -= 2;

                        //numOfSelectedPolys++;
                        Vector2 prevPos = selectedPolys[numOfSelectedPolys].transform.position;

                        float distDifference = Vector2.Distance(prevPos, tempPoly.transform.position);

                        if (distDifference > 2 )
                        {
                            Debug.Log("out of range");
                            numOfSelectedPolys = 0;
                            baseSelected = false;
                            selectedPolys = new List<Poly>();
                        }
                        else
                        {
                            Debug.Log("X dif = " + (tempPoly.transform.position.x - prevPos.x) + "y dif = " + (tempPoly.transform.position.y - prevPos.y));
                            Debug.Log(hit.collider.gameObject.name);
                            Debug.Log("Entered first");
                            numOfSelectedPolys = 0;
                        }
                    }
                    else if (!tempPoly.hasRed)
                    {
                        Debug.Log("Poly needs to have a red");
                        baseSelected = false;
                        selectedPolys = new List<Poly>();
                    }
                    else if (tempPoly.fullBlue)
                    {
                        Debug.Log("Poly cant belong to enemy");
                        baseSelected = false;
                        selectedPolys = new List<Poly>();
                    }

                    if(tempPoly.fullRed == true)
                    {
                        Debug.Log("Complete round");
                        foreach(Poly n in selectedPolys)
                        {
                            n.r_turnSPolyRed();
                        }
                        numOfSelectedPolys = 0;
                        baseSelected = false;
                        foreach (Poly n in selectedPolys)
                        {
                            n.adjacencyChecker();
                        }
                        selectedPolys = new List<Poly>();
                    }
                }
                
            }
        }
    }

    public Poly GetPolyAtPos(Vector3 t_Pos)
    {
        foreach(Poly pol in polys)
        {
            if (pol.transform.position == t_Pos)
            {
                return pol;
            }
        }
        return null;


    }

    public Poly baseFinder()
    {
        Poly selectedPoly;
        for (int i = 0; i < polys.Count; i++)
        {
            if (polys[i].frontlineBase)
            {
                numStorer = i;

            }

            break;
        }
        Poly p = polys[numStorer];

        if(p.availablePolys.Count < 1)
        {
            return null;
        } else if (p.availablePolys.Count > 0)
        {
            selectedPoly = p.availablePolys[0];
            p.availablePolys.Remove(p.availablePolys[0]);

            return selectedPoly;
        }
        return null;

    }

    public void spawner(Vector2 pos)
    {
        Poly newthing = Instantiate(polygon, pos, Quaternion.Euler(0, 0, 0)).GetComponent<Poly>();
        newthing.t_Rand();
        polys.Add(newthing);

    }

    public void messageColourChanger()
    {
        foreach(Poly n in polys)
        {
            n.activateSwitchers();
        }
    }
}