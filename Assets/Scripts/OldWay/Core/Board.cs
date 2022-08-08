using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("BOARD SIZE")]
    [Space(5)]
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("BOARD PREFABS")]
    [Space(5)]
    [SerializeField] private GameObject bgTilePrefab;
    [SerializeField] private Emblem[] emblemDB;

    [Header("BOARD ANIMATION TIMES")]
    [Space(5)]
    [SerializeField][Range(1f, 10f)] private float emblemSpeed = 7f;
    [SerializeField][Range(0f, 1f)] private float swipeBackTime = .5f;
    [SerializeField][Range(0f, 1f)] private float refillTime = .1f;
    [SerializeField][Range(0f, 1f)] private float columnColapse = .2f;

    [Header("AVOID INITIAL MATCHES")]
    [Space(5)]
    [SerializeField] private int maxIterations = 100;

    [Header("INPUT SETTINGS")]
    [Space(5)]
    [SerializeField][Range(0.1f, 1f)] private float touchSensibility = 0.5f;

    [Header("SKILL SETTINGS")]
    [Space(5)]
    [SerializeField] public SkillManager skillManager;

    private MatchFinder matchFinder;
    private Emblem[,] boardStatus;

    public BoardStates currentState = BoardStates.Move;

    #region PROPERTIES
    public float TouchSensibility { get => touchSensibility; set => touchSensibility = value; }
    public Emblem[,] BoardStatus { get => boardStatus; set => boardStatus = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public float EmblemSpeed { get => emblemSpeed; set => emblemSpeed = value; }
    public float SwipeBackTime { get => swipeBackTime; set => swipeBackTime = value; }
    public MatchFinder MatchFinder { get => matchFinder; set => matchFinder = value; }
    public Emblem[] EmblemDB { get => emblemDB; set => emblemDB = value; }
    #endregion

    private void Awake()
    {
        MatchFinder = GetComponent<MatchFinder>();
        skillManager = GetComponent<SkillManager>();
    }

    private void Start()
    {
        BoardStatus = new Emblem[Width, Height];
        SetUp();
    }

    private void SetUp()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, position, Quaternion.identity);
                bgTile.transform.parent = transform;
                bgTile.name = "BG Tile - (" + x + ", " + y + ")";

                int randomEmblem = GenerateRandomEmblem();
                int currentIterations = 0;

                //Prevent repeated adjacent emblems 
                while (MatchesAt(new Vector2Int(x, y), EmblemDB[randomEmblem]) && currentIterations < maxIterations)
                {
                    randomEmblem = GenerateRandomEmblem();
                    currentIterations++;
                }

                SpawnEmblem(new Vector2Int(x, y), EmblemDB[randomEmblem]);
            }
        }
    }

    private int GenerateRandomEmblem()
    {
        return Random.Range(0, EmblemDB.Length);
    }

    private void SpawnEmblem(Vector2Int position, Emblem emblemToSpawn)
    {
        //if (boardStatus[position.x, position.y] != null) return;
        Vector3 position3d = new Vector3(position.x, position.y + height, 0);
        Emblem emblem = Instantiate(emblemToSpawn, position3d, Quaternion.identity, transform);
        emblem.transform.parent = transform;
        emblem.name = "Emblem - (" + position.x + ", " + position.y + ")";

        //Add emblem to board
        BoardStatus[position.x, position.y] = emblem;

        //Add a position to tile
        emblem.SetUpEmblem(position, this);
    }

    private bool MatchesAt(Vector2Int pos, Emblem emblem)
    {
        if (pos.x > 1)
        {
            if (boardStatus[pos.x - 1, pos.y].EmblemColor == emblem.EmblemColor
                && boardStatus[pos.x - 2, pos.y].EmblemColor == emblem.EmblemColor)
            {
                return true;
            }
        }

        if (pos.y > 1)
        {
            if (boardStatus[pos.x, pos.y - 1].EmblemColor == emblem.EmblemColor
                && boardStatus[pos.x, pos.y - 2].EmblemColor == emblem.EmblemColor)
            {
                return true;
            }
        }
        return false;
    }

    private void DestroyMatchedEmblemAt(Vector2Int position)
    {
        Emblem emblem = boardStatus[position.x, position.y];
        if (emblem != null)
        {
            if (emblem.isMatched)
            {
                Destroy(emblem.gameObject);
                emblem = null;
            }
        }
    }

    public void DestroyMatches()
    {
        for (int x = 0; x < matchFinder.CurrentMatches.Count; x++)
        {
            if (matchFinder.CurrentMatches[x] != null)
            {
                DestroyMatchedEmblemAt(matchFinder.CurrentMatches[x].PosIndex);
            }
        }
        StartCoroutine(DecreaseRow_Coro());
    }

    private IEnumerator DecreaseRow_Coro()
    {
        yield return new WaitForSeconds(columnColapse);

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //There is empty space
                if (boardStatus[x, y] == null)
                {
                    nullCounter++;
                }
                //there is a gem
                else if (nullCounter > 0)
                {
                    //Substract the null emblems encountered to the first emblem 
                    boardStatus[x, y].posIndex.y -= nullCounter;
                    boardStatus[x, y - nullCounter] = boardStatus[x, y];

                    boardStatus[x, y] = null;

                }
            }
            nullCounter = 0;
        }

        StartCoroutine(FillBoard_Coro());
    }

    private IEnumerator FillBoard_Coro()
    {
        yield return new WaitForSeconds(refillTime);

        RefillBoard();

        yield return new WaitForSeconds(refillTime);

        //changes current matches in match finder so DestroyMatches has something to actually destroy
        matchFinder.FindAllMatches();

        //New emblem generation created new matches
        if (matchFinder.CurrentMatches.Count > 0)
        {
            yield return new WaitForSeconds(.75f);
            DestroyMatches();
        }
        else
        {
            yield return new WaitForSeconds(refillTime);
            currentState = BoardStates.Move;
        }
    }

    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (boardStatus[x, y] == null)
                {
                    int randomEmblem = GenerateRandomEmblem();
                    Vector2Int position = new Vector2Int(x, y);
                    SpawnEmblem(position, EmblemDB[randomEmblem]);
                }
            }
        }
        CheckMisplacedEmblems();
    }

    private void CheckMisplacedEmblems()
    {
        List<Emblem> foundEmblems = new();

        foundEmblems.AddRange(FindObjectsOfType<Emblem>());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (foundEmblems.Contains(boardStatus[x, y]))
                {
                    foundEmblems.Remove(boardStatus[x, y]);
                }
            }
        }

        foreach (Emblem emblem in foundEmblems)
        {
            Destroy(emblem.gameObject);
        }

    }
}
