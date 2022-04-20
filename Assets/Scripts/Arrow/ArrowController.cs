using System;
using Point;
using Transition;
using UnityEngine;

namespace Arrow
{
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] private RectTransform _arrowFooting = null;

        private bool _hasInput = false;
        private Vector3 _startPosition = Vector3.zero;
        private GamePoint _startGamePoint = null; 
        private GamePoint _endGamePoint = null; 

        private void Update()
        {
            SetupArrow();
            MoveArrow();
        }

        private void SetupArrow()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startGamePoint = CheckGamePoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)); 
                
                if (_startGamePoint != null && _startGamePoint.State == PointState.Union)
                {
                    _arrowFooting.gameObject.SetActive(true);
                    _hasInput = true;
                    _startPosition = Input.mousePosition;
                    _arrowFooting.position = _startPosition;
                }
            }
            
            if(Input.GetMouseButtonUp(0))
            {
                _arrowFooting.gameObject.SetActive(false);
                _hasInput = false;

                _endGamePoint = CheckGamePoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                
                if (_endGamePoint != null && _startGamePoint != null && _startGamePoint.State == PointState.Union)
                {
                    TransisionManager.Instance.TryMove(_startGamePoint, _endGamePoint);
                    _startGamePoint = null;
                    _endGamePoint = null;
                }
            }
        }

        private void MoveArrow()
        {
            if (!_hasInput)
            {
                return;
            }

            _arrowFooting.sizeDelta = new Vector2(_arrowFooting.sizeDelta.x, Vector2.Distance(Input.mousePosition, _startPosition));
            
            float angle = Mathf.Atan2(_startPosition.y - Input.mousePosition.y, _startPosition.x - Input.mousePosition.x) * Mathf.Rad2Deg -90;
            _arrowFooting.rotation = Quaternion.Euler(0,0, angle);

        }

        private GamePoint CheckGamePoint(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, -Vector2.up);
            
            if (hit.collider != null)
            {
                var gamePoint = hit.collider.gameObject.GetComponent<GamePoint>();

                if (gamePoint != null)
                {
                    return gamePoint;
                }
            }

            return null;
        }

    }
}
