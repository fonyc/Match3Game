using MVC.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.View
{
    public class BoardView : MonoBehaviour
    {
        [Header("--- BOARD SETTINGS ---")]
        [Space(5)]
        [SerializeField] private Vector2Int _boardSize = new Vector2Int(6, 6);
        [SerializeField] private Camera _camera;
        private Plane _boardPlane;

        private BoardController _controller;

        private void Awake()
        {
            _boardPlane = new Plane(Vector3.forward, Vector3.zero);
            _controller = new BoardController(_boardSize.x, _boardSize.y);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (_boardPlane.Raycast(ray, out float hitDistance))
                {
                    Vector3 hitPosition = ray.GetPoint(hitDistance);
                    _controller.CheckInput(new Vector2Int((int)hitPosition.x, (int)hitPosition.y));
                }
            }
        }
    }
}
