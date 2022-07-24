using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private GameObject bgTilePrefab;

    public Emblem[] emblemDB;

    private MatchFinder matchFinder;

    private Emblem[,] boardStatus;

    [SerializeField][Range(1f, 10f)] private float emblemSpeed = 7f;
    [SerializeField][Range(0f, 1f)] private float swipeBackTime = .5f;
    [SerializeField][Range(0f, 1f)] private float columnColapse = .2f;
    public int maxIterations = 100;
    [SerializeField][Range(0.1f, 1f)] private float touchSensibility = 0.5f;

    public float TouchSensibility { get => touchSensibility; set => touchSensibility = value; }
    public Emblem[,] BoardStatus { get => boardStatus; set => boardStatus = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public float EmblemSpeed { get => emblemSpeed; set => emblemSpeed = value; }
    public float SwipeBackTime { get => swipeBackTime; set => swipeBackTime = value; }
    public MatchFinder MatchFinder { get => matchFinder; set => matchFinder = value; }

    private void Awake()
    {
        MatchFinder = GetComponent<MatchFinder>();
    }

    private void Start()
    {
        BoardStatus = new Emblem[Width, Height];
        SetUp();
    }

    private void Update()
    {
        MatchFinder.FindAllMatches();
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

                while (MatchesAt(new Vector2Int(x, y), emblemDB[randomEmblem]) && currentIterations < maxIterations)
                {
                    randomEmblem = GenerateRandomEmblem();
                    currentIterations++;
                    Debug.Log(currentIterations + "/" + maxIterations);
                }

                SpawnEmblem(new Vector2Int(x, y), emblemDB[randomEmblem]);
            }
        }
    }

    private int GenerateRandomEmblem()
    {
        return Random.Range(0, emblemDB.Length);
    }

    private void SpawnEmblem(Vector2Int position, Emblem emblemToSpawn)
    {
        //if (boardStatus[position.x, position.y] != null) return;
        Vector3 position3d = new Vector3(position.x, position.y, 0);
        Emblem emblem = Instantiate(emblemToSpawn, position3d, Quaternion.identity);
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
            //Check left and left-1 tiles
            if (boardStatus[pos.x - 1, pos.y].EmblemType == emblem.EmblemType
                && boardStatus[pos.x - 2, pos.y].EmblemType == emblem.EmblemType)
            {
                return true;
            }
        }

        if (pos.y > 1)
        {
            //Check left and left-1 tiles
            if (boardStatus[pos.x, pos.y - 1].EmblemType == emblem.EmblemType
                && boardStatus[pos.x, pos.y - 2].EmblemType == emblem.EmblemType)
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
    /// <summary>
    /// Iterates through match list in matchfinder and destroy every match
    /// </summary>
    public void DestroyMatches()
    {
        for (int x = 0; x < matchFinder.CurrentMatches.Count; x++)
        {
            if (matchFinder.CurrentMatches[x] != null)
            {
                DestroyMatchedEmblemAt(matchFinder.CurrentMatches[x].PosIndex);
            }
        }
        StartCoroutine(IncreaseRow_Coro());
    }

    private IEnumerator IncreaseRow_Coro()
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
    }

}
