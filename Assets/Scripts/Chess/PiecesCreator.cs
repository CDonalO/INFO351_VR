using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PiecesCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material whiteMaterial;
    private Dictionary<string, GameObject> nameToPieceDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var piece in piecesPrefabs)
        {
            Debug.Log("PieceName " + piece.name);
            nameToPieceDict.Add(piece.name, piece);
        }
    }

    public GameObject CreatePiece(Type type,string team)
    {
        Debug.Log(type.ToString());
        GameObject prefab = nameToPieceDict[team + type.ToString()];
        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab);
            if(team.Equals("Black")){
                newPiece.transform.Rotate(0f,180f,0f,Space.Self);
            }
            return newPiece;
        }
        return null;
    }


    public Material GetTeamMaterial(TeamColor team)
    {
        return team == TeamColor.White ? whiteMaterial : blackMaterial;
    }
}