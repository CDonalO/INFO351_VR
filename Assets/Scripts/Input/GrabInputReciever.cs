using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInputReciever : InputReciever
{
    private BoardInputHandler inputHandler;

    public void Start()
    {
        inputHandler = GameObject.FindGameObjectsWithTag("Board")[0].GetComponent(typeof(BoardInputHandler)) as BoardInputHandler;
    }

    public void select(GameObject s)
    {
        Debug.Log("NAMEEEEEEEEE " + s.name);
        if (s.name == "Selector(Clone)")
        {
            inputHandler.ProcessInput(s.transform.position, null, null);
        }
        else
        {
            inputHandler.ProcessInput(new Vector3(2, 2, 2), s, null);
        }
    }

    public override void OnInputRecieved()
    {
    }
}