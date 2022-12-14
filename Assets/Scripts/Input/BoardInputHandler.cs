using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class BoardInputHandler : MonoBehaviour, IInputHandler
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action onClick)
    {
        if (selectedObject != null) {
            board.OnSquareSelected(selectedObject);
            return;
        }
        Debug.Log("Bleep " + inputPosition);
        if (selectedObject != null){
            board.OnSquareSelected(selectedObject.transform.position);
        } else {
        board.OnSquareSelected(inputPosition);
        }
    }
}