using Board.Controller;
using Board.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board.View
{
    public class BoardView : MonoBehaviour
    {
        #region VARIABLES
       
        [Header("--- BOARD SETTINGS ---")]
        [Space(5)]
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject[] emblemPrefabs;

        private Vector2Int _boardSize;
        public int visualPieceFallPosition => _boardSize.y;
        //Input
        private Plane _boardPlane;
        private Vector2Int touch;

        //Animations
        private List<IViewAnimation> _animations = new List<IViewAnimation>();
        private bool IsAnimating => _animations.Count > 0;

        public GameObject[] EmblemPrefabs { get => emblemPrefabs; set => emblemPrefabs = value; }

        private BoardController _controller;
        private List<EmblemView> _emblemViewList = new List<EmblemView>();
        
        #endregion

        public void Initialize(BoardController boardController, Vector2Int boardSize)
        {
            _boardSize = boardSize;
            _camera = Camera.main;
            _boardPlane = new Plane(Vector3.forward, Vector3.zero);
            _controller = boardController;

            _controller.OnEmblemMoved += OnEmblemMoved;
            _controller.OnEmblemDestroyed += OnEmblemDestroyed;
            _controller.OnEmblemCreated += OnEmblemCreated;
            _controller.OnColorChanged += OnColorChanged;

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
            if (Input.GetMouseButtonDown(0) && !IsAnimating)
            {
                touch = GetEmblemFromTouch(_boardPlane);
                _controller.InputSelected.ProcessInput(_controller, touch);
            }
        }

        #region ANIMATIONS

        private void OnEmblemMoved(Vector2Int origin, Vector2Int destination)
        {
            _animations.Add(new MoveEmblemAnimation(origin, destination));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private void OnEmblemDestroyed(Vector2Int emblemToDestroy)
        {
            _animations.Add(new DestroyEmblemAnimation(GetEmblemViewAtPosition(emblemToDestroy)));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private void OnEmblemCreated(Vector2Int emblemPosition, EmblemItem item)
        {
            _animations.Add(new CreateEmblemAnimation(emblemPosition, item));
            if (_animations.Count == 1)
            {
                StartCoroutine(ProcessAnimations());
            }
        }

        private void OnColorChanged(Vector2Int emblemPosition)
        {
            EmblemView emblemView = GetEmblemViewAtPosition(emblemPosition);
            _emblemViewList.Remove(emblemView);
            Destroy(emblemView.gameObject);
            GameObject emblem = Instantiate(EmblemPrefabs[_controller.GetEmblemColor(emblemPosition.x, emblemPosition.y)], new Vector3(emblemPosition.x, emblemPosition.y, 0), Quaternion.identity, transform);
            emblem.GetComponent<EmblemView>().Position = new Vector2Int(emblemPosition.x, emblemPosition.y);
            _emblemViewList.Add(emblem.GetComponent<EmblemView>());
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

        #region UTILITY METHODS
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
        #endregion
    }
}
