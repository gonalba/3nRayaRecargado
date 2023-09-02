using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IA para resolver jugar en un tablero dimensional
/// </summary>
public class AIPlayer_DimBoard
{
    class TicTacToeAI_heuristicas
    {
        int[,] board = new int[3, 3];
        int[] weights = new int[9] { 3, 2, 3, 2, 4, 2, 3, 2, 3 };
        int[] rows = new int[9] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
        int[] cols = new int[9] { 0, 1, 2, 0, 1, 2, 0, 1, 2 };
        int player = 1;
        int opponent = 2;
        int empty = 0;

        int Evaluate()
        {
            int score = 0;
            for (int i = 0; i < 8; i++)
            {
                int lineScore = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (board[rows[i], cols[i] + j] == player)
                        lineScore++;
                    else if (board[rows[i], cols[i] + j] == opponent)
                        lineScore--;
                }
                score += weights[i] * lineScore;
            }
            return score;
        }

        void MakeMove()
        {
            int bestScore = int.MinValue;
            int bestRow = -1;
            int bestCol = -1;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == empty)
                    {
                        board[row, col] = player;
                        int score = Evaluate();
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestRow = row;
                            bestCol = col;
                        }
                        board[row, col] = empty;
                    }
                }
            }
            if (bestRow != -1 && bestCol != -1)
            {
                board[bestRow, bestCol] = player;
                // Switch player and opponent
                int temp = player;
                player = opponent;
                opponent = temp;
            }
        }
    }
}
