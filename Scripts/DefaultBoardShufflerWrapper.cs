using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class BoardShuffler
    {
        public abstract void Shuffle(IBoard board);
    }

    public class DefaultBoardShuffler : BoardShuffler
    {
        private readonly LinkedList<Vector2Int> shuffledCoords = new LinkedList<Vector2Int>();

        public override void Shuffle(IBoard board)
        {
            shuffledCoords.Clear();
            foreach (var coord in board)
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

            foreach (var coord in board)
            {
                var randomCoord = shuffledCoords.First.Value;
                shuffledCoords.RemoveFirst();
                (board[coord], board[randomCoord]) = (board[randomCoord], board[coord]);
            }
        }
    }

    public readonly struct PieceCoordChange
    {
        public readonly IPiece piece;
        public readonly Vector2Int newCoord;

        public PieceCoordChange(IPiece piece, Vector2Int newCoord)
        {
            this.piece = piece;
            this.newCoord = newCoord;
        }
    }

    public readonly struct RefreshPiecesPositionsCommand : IBoardCommand
    {
        private readonly SceneBoard sceneBoard;
        private readonly List<PieceCoordChange> pieceCoordChanges;

        public RefreshPiecesPositionsCommand(SceneBoard sceneBoard, List<PieceCoordChange> pieceCoordChanges)
        {
            this.sceneBoard = sceneBoard;
            this.pieceCoordChanges = pieceCoordChanges;
        }

        public IEnumerator Execute()
        {
            foreach (var change in pieceCoordChanges)
            {
                var scenePiece = sceneBoard.GetScenePiece(change.piece);
                if (scenePiece)
                {
                    scenePiece.MoveTo(sceneBoard.CoordToWorld(change.newCoord));
                }
                else
                {
                    Debug.LogError("ZNOWU SCENE PIECE NULL");
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    public class GeneralPiecesMover : MonoBehaviour
    {
    }

    public abstract class BoardShufflerWrapper : MonoBehaviour
    {
        public abstract void ShufflePieces();
    }

    public class DefaultBoardShufflerWrapper : BoardShufflerWrapper
    {
        [SerializeField]
        private SceneBoard sceneBoard;
        [SerializeField]
        private BoardController boardController;

        private BoardShuffler boardShuffler;

        private void Awake()
        {
            boardShuffler = new DefaultBoardShuffler();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShufflePieces();
            }
        }

        [ContextMenu("Shuffle")]
        public override void ShufflePieces()
        {
            boardShuffler.Shuffle(sceneBoard.Board);
            var changes = new List<PieceCoordChange>();
            foreach (var coord in sceneBoard.Board)
            {
                changes.Add(new PieceCoordChange(
                    piece: sceneBoard.Board[coord],
                    newCoord: coord));
            }

            var refreshCommand = new RefreshPiecesPositionsCommand(sceneBoard, changes);
            boardController.RequestCommand(refreshCommand);
        }
    }
}
