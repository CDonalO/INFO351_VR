using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : ChessPlayer {
    Stockfish stockfish;

    private Vector2Int pieceCoords;
    private Vector2Int squareCoords;
    private bool availableMove = false;

    public AIPlayer(TeamColor team, Board board, Stockfish stockfish): base(team, board) {
        this.stockfish = stockfish;
    }

    public void OnActivate() {
        stockfish.setPosition(board.ToFenNotation());
        string bestMove = stockfish.getBestMove();
        Debug.Log(bestMove);
        string moveCoords = bestMove.Split(" ")[1];
        pieceCoords = new Vector2Int((int) moveCoords[0] - (int) 'a',  (int) moveCoords[1] - (int) '1');
        squareCoords = new Vector2Int((int) moveCoords[2] - (int) 'a',  (int) moveCoords[3] - (int) '1');
        Debug.Log(pieceCoords);
        Debug.Log(squareCoords);
        availableMove = true;
    }

    public bool IsMoveAvailable() {
        return availableMove;
    }

    public void doMove() {
        availableMove = false;
        board.OnSquareSelected(pieceCoords);
        Debug.Log("YEET YEET YEET");
        board.OnSquareSelected(squareCoords);
    }
}
