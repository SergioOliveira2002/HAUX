using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poly : MonoBehaviour
{
    int colour;
    public List<Color> colours = new List<Color>();
    public List<SubPoly> s_Poly = new List<SubPoly>();
    public bool s_friendly = false;

    public bool hasRed = false;
    public bool fullRed = false;

    public bool hasBlue = false;
    public bool fullBlue = false;

    public List<int> subpolyColours = new List<int>();

    public int prevnum;
    public bool hasMoved = false;
    bool collectedAvailable = false;

    public List<Poly> availablePolys = new List<Poly>();
    public List<Vector3> moveValues = new List<Vector3>();

    public bool frontlineBase = false;
    int avPolyCount = 0;

    public int basePolyCount = 0;

    Vector2 ownPos;

    // Start is called before the first frame update


    public Color colourPicker()
    {
        int y = Random.Range(0, colours.Count);

        if (y == 2 || y == 3)
        {
            int z = Random.Range(0, 6);

            if (z == 0 || z == 1)
            {
                return colours[0];
            }

            if (z == 2 || z == 3)
            {
                return colours[1];
            }

            if (z == 4)
            {
                return colours[2];
            }

            if (z == 5)
            {
                return colours[3];
            }
        }
        else
        {
            return colours[y];
        }
        return colours[y];
    }

    public void t_Red()
    {
        subpolyColours = new List<int>();
        foreach (SubPoly x in s_Poly)
        {

            x.s_color(0);
            subpolyColours.Add(x.storenumber);
        }
        fullRed = true;
        hasRed = true;
        fullBlue = false;
        hasBlue = false;
        hasMoved = true;
    }

    public void t_Blue()
    {
        subpolyColours = new List<int>();
        foreach (SubPoly x in s_Poly)
        {
            x.s_color(1);
            subpolyColours.Add(x.storenumber);
        }
        fullBlue = true;
        hasBlue = true;
        fullRed = false;
        hasRed = false;
    }

    public void t_Rand()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 4 && GetHomogenous(SubPolyColors()))
            {
                int z = Random.Range(0, 4);
                while (SubPolyColors()[0] == z)
                {
                    z = Random.Range(0, 4);
                }
                s_Poly[i].s_color(z);
                subpolyColours.Add(s_Poly[i].storenumber);
                if (z == 2)
                {
                    //s_Poly[i].switcher = true;
                }
                if (z == 0)
                {
                    hasRed = true;
                }
                if (z == 1)
                {
                    hasBlue = true;
                }
            }
            else
            {
                int y = Random.Range(0, 4);
                s_Poly[i].s_color(y);
                if (y == 2)
                {
                    s_Poly[i].switcher = true;
                }
                subpolyColours.Add(s_Poly[i].storenumber);
                if (y == 0)
                {
                    hasRed = true;
                }
                if (y == 1)
                {
                    hasBlue = true;
                }
            }
        }
    }

    public void r_colour()
    {
        Debug.Log("RecieveMsg");
        //StartCoroutine(larry());

    }

    private List<int> SubPolyColors()
    {
        List<int> colors = new List<int>();
        foreach (SubPoly sub in s_Poly)
        {
            colors.Add(sub.storenumber);
        }
        return colors;
    }

    private bool GetHomogenous(List<int> numbers)
    {
        int curnum = 100000;
        bool homogenous = true;
        foreach (int num in numbers)
        {
            if (num != curnum)
            {
                homogenous = false;
            }
            curnum = num;
        }
        return homogenous;
    }

    public void r_turnSPolyRed()
    {
        if (fullRed == true)
        {
            //meh
        }
        else if (!fullRed)
        {
            foreach (SubPoly x in s_Poly)
            {
                if (x.storenumber == 0)
                {
                    //rerun
                }
                else if (x.storenumber != 0)
                {
                    x.s_color(0);
                    break;
                }
            }
        }
    }

    public void checkColourStore()
    {
        subpolyColours = new List<int>();

        foreach (SubPoly x in s_Poly)
        {
            subpolyColours.Add(x.storenumber);
        }
    }

    public void checkColourBool()
    {
        foreach (SubPoly x in s_Poly)
        {
            if (x.storenumber == 0)
            {
                hasRed = true;
            }

            if (x.storenumber == 1)
            {
                hasBlue = true;
            }


        }

        foreach (SubPoly x in s_Poly)
        {
            //handle full colour cubes here bro
            int tempHomogenous = 0;
            bool y = true;
            bool rCheck = false;
            bool bCheck = false;
            if (fullRed == false && fullBlue == false)
            {
                foreach (SubPoly z in s_Poly)
                {
                    if (z.storenumber == 0)
                    {
                        tempHomogenous++;
                    }
                }
                if (tempHomogenous == 4)
                {
                    hasBlue = false;
                    fullBlue = false;
                    fullRed = true;
                    hasRed = true;
                    rCheck = true;
                    foreach (SubPoly n in s_Poly)
                    {
                        n.switcher = false;
                    }


                    if (!hasMoved)
                    {

                    }

                    tempHomogenous = 0;
                }
                else
                {
                    fullRed = false;
                    rCheck = true;
                }
            }
            else if (fullRed == false && fullBlue == false)
            {
                foreach (SubPoly z in s_Poly)
                {
                    if (z.storenumber == 1)
                    {
                        tempHomogenous++;
                    }
                }
                if (tempHomogenous == 4)
                {
                    hasBlue = true;
                    fullBlue = true;
                    fullRed = false;
                    hasRed = false;
                    bCheck = true;
                    tempHomogenous = 0;
                    foreach (SubPoly n in s_Poly)
                    {
                        n.switcher = false;
                    }
                }
                else
                {
                    fullBlue = false;
                    bCheck = true;
                }
            }
        }
    }

    void Update()
    {
        ownPos = gameObject.transform.position;
        checkColourStore();
        checkColourBool();

        if (fullRed == true && hasMoved == true && collectedAvailable == false)
        {
            adjacencyChecker();
        }

        avPolyCount = 0;
        foreach (Poly n in availablePolys)
        {
            avPolyCount++;
        }

        if (avPolyCount >= 1)
        {
            frontlineBase = true;
        }
        else if (avPolyCount <= 0)
        {
            frontlineBase = false;
        }


        if (fullRed == true && !hasMoved)
        {
            movementChecker();
        }

    }


    public void adjacencyChecker()
    {
        availablePolys = new List<Poly>();
        Vector3 t = gameObject.transform.position;

        for (int i = 0; i < 4; i++)
        {
            Vector3 y = new Vector3(t.x + moveValues[i].x, t.y + moveValues[i].y, 0);
            Poly newPoly = GridManager.Instance.GetPolyAtPos(y);

            if (newPoly == null)
            {

            }
            else if (!newPoly.fullRed)
            {
                Debug.Log("!ADDED!");
                availablePolys.Add(newPoly);

                y = new Vector3(0, 0, 0);

            }

            collectedAvailable = true;
        }
    }

    public void movementChecker()
    {
        for (int i = 0; i < GridManager.Instance.t_Pos.Count; i++)
        {
            Poly basePoly = GridManager.Instance.GetPolyAtPos(GridManager.Instance.t_Pos[i]);

            if (basePoly.availablePolys.Count > 0)
            {
                Poly switchPoly = basePoly.availablePolys[0];
                basePoly.availablePolys.RemoveAt(0);
                if (basePoly.availablePolys.Count == 0)
                {
                    GridManager.Instance.t_Pos.RemoveAt(0);
                    basePoly.adjacencyChecker();
                }
                else
                {
                    //leave it alone

                }

                Vector2 switchPolyPos = switchPoly.transform.position;

                gameObject.transform.position = switchPolyPos;
                GridManager.Instance.t_Pos.Add(gameObject.transform.position);

                switchPoly.transform.position = ownPos;

                adjacencyChecker();
                //GridManager.Instance.messageColourChanger();
                break;



            }
            else
            {
                //do nothing
            }
        }

        hasMoved = true;
    }

    public void activateSwitchers()
    {
        foreach (SubPoly n in s_Poly)
        {
            n.colourSwitch();
        }
    }
}