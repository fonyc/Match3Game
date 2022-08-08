using System.Collections;
using UnityEngine;

public class Emblem : MonoBehaviour
{
    public Vector2Int posIndex;
    private Vector2Int previousPosition;
    [SerializeField] private EmblemColor emblemColor;
    private OrientationAttack orientationAttack = OrientationAttack.None;
    public GameObject VFX_explosion;

    public Vector2Int PosIndex { get => posIndex; set => posIndex = value; }
    public bool isMatched;

    private Board board;
    private Vector2 firstTouchPos;
    private Vector2 lastTouchPos;
    private float swipeAngle = 0;

    private Emblem otherEmblem;

    public Vector2Int PreviousPosition { get => previousPosition; set => previousPosition = value; }
    public EmblemColor EmblemColor { get => emblemColor; set => emblemColor = value; }
    public OrientationAttack OrientationAttack { get => orientationAttack; set => orientationAttack = value; }

    private void Update()
    {
        PieceAnimation();
    }

    #region INPUT
    private void OnMouseDown()
    {
        if (board.currentState != BoardStates.Move) return;
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;

        //Check if the emblem is being selected as skill target
        board.skillManager.SelectEmblemToPerformSkill(this);
    }

    private void OnMouseUp()
    {
        if (board.currentState != BoardStates.Move) return;

        lastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;
        swipeAngle = CalculateAngle(firstTouchPos, lastTouchPos);

        //Avoid unvoluntary clicking
        if (Vector3.Distance(firstTouchPos, lastTouchPos) < board.TouchSensibility) return;

        MovePieces();
    }
    #endregion

    private float CalculateAngle(Vector2 origin, Vector2 destination)
    {
        return Mathf.Atan2(destination.y - origin.y, destination.x - origin.x) * 180 / Mathf.PI;
    }

    public void SetUpEmblem(Vector2Int pos, Board board)
    {
        posIndex = pos;
        this.board = board;
    }

    private void MovePieces()
    {
        previousPosition = posIndex;

        //Right Swap 
        if (swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.Width - 1)
        {
            otherEmblem = board.BoardStatus[posIndex.x + 1, posIndex.y];
            if (otherEmblem == null) return;
            otherEmblem.posIndex.x--;
            posIndex.x++;
        }
        //Swipe up
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.Height - 1)
        {
            otherEmblem = board.BoardStatus[posIndex.x, posIndex.y + 1];
            if (otherEmblem == null) return;
            otherEmblem.posIndex.y--;
            posIndex.y++;
        }
        //Swipe down
        else if (swipeAngle >= -135 && swipeAngle < -45 && posIndex.y > 0)
        {
            otherEmblem = board.BoardStatus[posIndex.x, posIndex.y - 1];
            if (otherEmblem == null) return;
            otherEmblem.posIndex.y++;
            posIndex.y--;
        }
        //Swipe Left
        if ((swipeAngle > 135 || swipeAngle < -135) && posIndex.x > 0)
        {
            otherEmblem = board.BoardStatus[posIndex.x - 1, posIndex.y];
            if (otherEmblem == null) return;
            otherEmblem.posIndex.x++;
            posIndex.x--;
        }

        //In case player swaps outside the board
        if (otherEmblem == null) return;

        //Notify board of swipe changes
        board.BoardStatus[posIndex.x, posIndex.y] = this;
        board.BoardStatus[otherEmblem.posIndex.x, otherEmblem.posIndex.y] = otherEmblem;

        StartCoroutine(CheckSwipe_Coro());
    }

    private void PieceAnimation()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            //Sprite and board are in different positions. Lerp to sprite to board pos
            transform.position = Vector2.Lerp(transform.position, posIndex, board.EmblemSpeed * Time.deltaTime);
        }
        else
        {
            //Make sure the emblem is in the correct position
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.BoardStatus[posIndex.x, posIndex.y] = this;
        }
    }

    private IEnumerator CheckSwipe_Coro()
    {
        board.currentState = BoardStates.Wait;

        //After the swap, wait to make the check and swap back (or not)
        yield return new WaitForSeconds(board.SwipeBackTime);

        //Double check matches (this move is costy)
        board.MatchFinder.FindAllMatches();

        if (otherEmblem != null)
        {
            //The swap is invalid
            if (!isMatched && !otherEmblem.isMatched)
            {
                //Return values to emblems
                otherEmblem.posIndex = posIndex;
                posIndex = previousPosition;

                //Notify board of swipe changes
                board.BoardStatus[posIndex.x, posIndex.y] = this;
                board.BoardStatus[otherEmblem.posIndex.x, otherEmblem.posIndex.y] = otherEmblem;

                yield return new WaitForSeconds(board.SwipeBackTime);

                board.currentState = BoardStates.Move;
            }
            //Swipe is valid, destroy the matches
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
