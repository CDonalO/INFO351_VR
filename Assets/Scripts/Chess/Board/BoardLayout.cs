using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BoardSetup", order=1)]
public class BoardLayout : ScriptableObject
{
    [SerializeField] BoardSquareSetup[] boardSqaures;
}
