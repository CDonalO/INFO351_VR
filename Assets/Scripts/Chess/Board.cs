using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;

    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    private Piece selectedPiece;
    private ChessGameController chessController;
    private SquareSelectorCreator squareSelector;

    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void SetDependencies(ChessGameController chessController)
    {
        this.chessController = chessController;
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / squareSize) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x, y);
    }

    public Piece GetPieceByID(int id) {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                if (grid[i, x] != null && grid[i,x].objId == id)
                {
                    return grid[i, x];
                }
            }
        }
        return null;
    }

    public void OnSquareSelected(Vector2Int coords)
    {
        Piece piece = GetPieceOnSquare(coords);
        if(piece == null){
            Debug.Log("null piece");
        }
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && selectedPiece != piece && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
            else if (selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords, selectedPiece);
        }
        else
        {
            if (piece != null && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
        }
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        OnSquareSelected(coords);
    }

    public void OnSquareSelected(GameObject obj)
    {


        Piece piece = GetPieceByID(obj.GetInstanceID());
        if (piece == null)
        {
            Debug.Log("null piece");
        }
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && selectedPiece != piece && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
/*            else if (selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords, selectedPiece);*/
        }
        else
        {
            if (piece != null && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
        }
    }

    private void SelectPiece(Piece piece)
    {
        chessController.RemoveMovesEnablingAttakOnPieceOfType<King>(piece);
        selectedPiece = piece;
        List<Vector2Int> selection = selectedPiece.avaliableMoves;
        ShowSelectionSquares(selection);
    }

    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squaresData.Add(position, isSquareFree);
        }
        squareSelector.ShowSelection(squaresData);
    }

    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }
    private void OnSelectedPieceMoved(Vector2Int coords, Piece piece)
    {
        TryToTakeOppositePiece(coords);
        UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
        selectedPiece.MovePiece(coords);
        DeselectPiece();
        EndTurn();
    }

    private void EndTurn()
    {
        chessController.EndTurn();
    }

    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }


    public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE){
            return false;
        }
        return true;
    }

    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                    return true;
            }
        }
        return false;
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }

    private void TryToTakeOppositePiece(Vector2Int coords)
    {
        Piece piece = GetPieceOnSquare(coords);
        if (piece && !selectedPiece.IsFromSameTeam(piece))
        {
            TakePiece(piece);
        }
    }

    private void TakePiece(Piece piece)
    {
        if (piece)
        {
            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            chessController.OnPieceRemoved(piece);
            Destroy(piece.gameObject);
        }
    }


    public void PromotePiece(Piece piece)
    {
        TakePiece(piece);
        chessController.CreatePieceAndInitialize(piece.occupiedSquare, piece.team, typeof(Queen));
    }

    public string ToFenNotation() {
        StringBuilder fen = new StringBuilder();
        int emptySquares;
        for (int i = BOARD_SIZE - 1; i >= 0; i--) {
            emptySquares = 0;
            for (int j = 0; j < BOARD_SIZE; j++) {
                if (grid[j,i] == null) {
                    emptySquares++;
                } else {
                    if (emptySquares > 0) {
                        fen.Append(emptySquares);
                        emptySquares = 0;
                    }
                    fen.AppendFormat(grid[j,i].ToString());
                }
            }
            if (emptySquares > 0) {
                fen.Append(emptySquares);
            }
            fen.Append("/");
        }
        fen.Remove(fen.Length - 1, 1);

        fen.AppendFormat(" {0} ", chessController.IsTeamTurnActive(TeamColor.White) ? "w" : "b");

        StringBuilder castling = new StringBuilder();
        King whiteKing = chessController.GetPlayer(TeamColor.White).GetKing();
        King blackKing = chessController.GetPlayer(TeamColor.Black).GetKing();
        castling.Append(whiteKing.CanCastleRight() ? "K" : "");
        castling.Append(whiteKing.CanCastleLeft() ? "Q" : "");
        castling.Append(blackKing.CanCastleRight() ? "k" : "");
        castling.Append(blackKing.CanCastleLeft() ? "q" : "");
        fen.Append(castling.Length > 0 ? castling : "-");

        fen.Append(" - 0");

        fen.AppendFormat(" {0}", chessController.GetMoveCounter());



        Debug.Log(fen.ToString());
        return fen.ToString();
    }

    internal void OnGameRestarted()
    {
        selectedPiece = null;
        CreateGrid();
    }

}