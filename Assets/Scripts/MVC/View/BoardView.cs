using MVC.Controller;
using MVC.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.View
{
    public class BoardView : MonoBehaviour
    {
        #region VARIABLES
       
        [Header("--- BOARD SETTINGS ---")]
        [Space(5)]
        [SerializeField] private Vector2Int _boardSize = new Vector2Int(6, 6);
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject[] emblemPrefabs;
        public int visualPieceFallPosition => _boardSize.y;

        //Input
        private Plane _boardPlane;
        private Vector2Int[] touches = new Vector2Int[2];

        //Animations
        private List<IViewAnimation> _animations = new List<IViewAnimation>();
        private bool IsAnimating => _animations.Count > 0;

        public GameObject[] EmblemPrefabs { get => emblemPrefabs; set => emblemPrefabs = value; }

        private BoardController _controller;
        private List<EmblemView> _emblemViewList = new List<EmblemView>();
        
        #endregion

        private void Awake()
        {
            _boardPlane = new Plane(Vector3.forward, Vector3.zero);
            _controller = new BoardController(_boardSize.x, _boardSize.y);

            _controller.OnEmblemMoved += OnEmblemMoved;
            _controller.OnEmblemColapse += OnEmblemColapse;
            _controller.OnWrongEmblemMoved += OnWrongEmblemMoved;
            _controller.OnEmblemDestroyed += OnEmblemDestroyed;
            _controller.OnEmblemCreated += OnEmblemCreated;
        }

        private void Start()
        {
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            for (int y = 0; y < _boardSize.y; y++)
            {
                for (int x = 0; x < _boardSize.x; x++)
                {
                    GameObject emblem = Instantiate(EmblemPrefabs[_controller.GetEmblemColor(x,y)], new Vector3(x, y, 0), Quaternion.identity, transform);
                    emblem.GetComponent<EmblemView>().Position = new Vector2Int(x, y);
                    _emblemViewList.Add(emblem.GetComponent<EmblemView>());
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                touches[0] = GetEmblemFromTouch(_boardPlane);
            }

            if (Input.GetMouseButtonUp(0))
            {
                float swipeAngle = CalculateAngle(touches[0], GetEmblemFromTouch(_boardPlane));
                touches[1] = GetSecondEmblemByAngle(swipeAngle, touches[0]);

                _controller.CheckInput(touches[0], touches[1]);
            }
        }

        #region ANIMATIONS

        private void OnEmblemMoved(EmblemModel origin, EmblemModel destination)
        {
            _animations.Add(new MoveEmblemAnimation(origin.Position, destination.Position));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private void OnEmblemColapse(EmblemModel origin, EmblemModel destination)
        {
            _animations.Add(new ColapseEmblemAnimation(origin.Position, destination.Position));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private void OnWrongEmblemMoved(EmblemModel origin, EmblemModel destination)
        {
            _animations.Add(new WrongSwapEmblemAnimation(origin.Position, destination.Position));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private void OnEmblemDestroyed(EmblemModel tileToDetroy)
        {
            _animations.Add(new DestroyEmblemAnimation(GetEmblemViewAtPosition(tileToDetroy.Position)));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private void OnEmblemCreated(EmblemModel emblemDestroyed, EmblemItem item)
        {
            _animations.Add(new CreateEmblemAnimation(GetEmblemViewAtPosition(emblemDestroyed.Position).Position, item));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private IEnumerator ProcessAnimations()
        {
            while (IsAnimating)
            {
                yield return _animations[0].PlayAnimation(this);
                _animations.RemoveAt(0);
            }
        }

        #endregion

        #region INPUT PROCESSING

        private Vector2Int GetSecondEmblemByAngle(float swipeAngle, Vector2Int firstTouch)
        {
            Vector2Int secondTouch = firstTouch;

            //Right Swap 
            if (swipeAngle < 45 && swipeAngle > -45)
            {
                secondTouch = new Vector2Int(secondTouch.x + 1, secondTouch.y);
            }
            //Swipe up
            else if (swipeAngle > 45 && swipeAngle <= 135)
            {
                secondTouch = new Vector2Int(secondTouch.x , secondTouch.y + 1);
            }
            //Swipe down
            else if (swipeAngle >= -135 && swipeAngle < -45)
            {
                secondTouch = new Vector2Int(secondTouch.x, secondTouch.y - 1);
            }
            //Swipe Left
            if ((swipeAngle > 135 || swipeAngle < -135))
            {
                secondTouch = new Vector2Int(secondTouch.x - 1, secondTouch.y);
            }
            return secondTouch;
        }

        private float CalculateAngle(Vector2 origin, Vector2 destination)
        {
            return Mathf.Atan2(destination.y - origin.y, destination.x - origin.x) * 180 / Mathf.PI;
        }

        #endregion

        private Vector2Int GetEmblemFromTouch(Plane plane)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float hitDistance))
            {
                Vector3 hitPosition = ray.GetPoint(hitDistance);
                return new Vector2Int(Mathf.RoundToInt(hitPosition.x), Mathf.RoundToInt(hitPosition.y));
            }
            else return Vector2Int.zero;
        }

        public EmblemView GetEmblemViewAtPosition(Vector2Int position)
        {
            return _emblemViewList.Find(emblem => emblem.Position == position);
        }

        public void AddEmblemView(EmblemView emblem)
        {
            _emblemViewList.Add(emblem);
        }

        public void RemoveEmblemView(EmblemView emblem)
        {
            _emblemViewList.Remove(emblem);
        }
    }
}
