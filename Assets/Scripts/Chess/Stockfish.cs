using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class Stockfish 
{
    Process p;

    public Stockfish()
    {
        p = new Process();
        p.StartInfo.FileName = Application.dataPath + "/stockfish/stockfish.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        p.StandardInput.WriteLine("setoption name Skill Level value 5");
    }

    public void setPosition(string fenPosition) {
        p.StandardInput.WriteLine("position fen " + fenPosition);
    }

    public void setDif(int dif){
        if (dif >20 || dif < 1){
            return;
        }
        p.StandardInput.WriteLine("setoption name Skill Level value {0}",dif);
    }

    public string getBestMove() {
        p.StandardInput.WriteLine("go movetime 4500");
        string bestMove;
        while (!(bestMove = p.StandardOutput.ReadLine()).StartsWith("bestmove") );
        return bestMove;
    }
}
