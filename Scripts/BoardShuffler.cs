﻿using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class BoardShuffler : MonoBehaviour
    {
        [SerializeField]
        private SceneBoard sceneBoard;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShufflePieces();
            }
        }

        private readonly LinkedList<Vector2Int> shuffledCoords = new LinkedList<Vector2Int>();
        public void ShufflePieces()
        {
            shuffledCoords.Clear();
            foreach (var coord in sceneBoard.Board)
            {
                if (Random.value > 0.5f)
                {
                    shuffledCoords.AddFirst(coord);
                }
                else
                {
                    shuffledCoords.AddLast(coord);
                }
            }

            foreach (var coord in sceneBoard.Board)
            {
                var randomCoord = shuffledCoords.First;
                shuffledCoords.RemoveFirst();
                // (Pieces[randomCoord.Value], Pieces[coord]) = (Pieces[coord], Pieces[randomCoord.Value]);
            }
        }
    }
}
