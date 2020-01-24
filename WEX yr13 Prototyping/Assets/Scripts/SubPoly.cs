using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPoly : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Poly poly;
    public int storenumber;

    public bool switcher = false;

    public float maxTimer;
    float timer;

    void Awake()
    {
        if(storenumber == 2 && (!poly.fullRed || !poly.fullBlue))
        {
            switcher = true;
        } else
        {
            switcher = false;
        }
    }
    void Start()
    {
        timer = maxTimer;
    }
    public void s_color (int colour)
    {
        sprite.color = poly.colours[colour];
        storenumber = colour;

    }

    private void Update()
    {

        if(poly.fullRed || poly.fullBlue)
        {
            switcher = false;
        }
    }

    public void colourSwitch()
    {
        if (switcher == true)
        {
            sprite.color = poly.colours[Random.Range(0, poly.colours.Count)];
        }
    }


}
