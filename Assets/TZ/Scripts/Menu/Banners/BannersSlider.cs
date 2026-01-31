using Common.AssetsProvider;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Menu.Banners
{
    public class BannersSlider : MenuElement
    {
        [SerializeField] private Transform _container;
        [SerializeField] private float _contentMoveDuration = 0.3f;
        [SerializeField] private float _swipeCooldown = 5f;
        [SerializeField] private List<Transform> _activeVisualDots;
        [SerializeField] private Transform _activeDot;
        [SerializeField] private Transform _leftBorder;
        [SerializeField] private Transform _rightBorder;
        [SerializeField] private Transform _upBorder;
        [SerializeField] private Transform _downBorder;


        private IAssetsProvider _assetsProvider;
        private List<GameObject> _banners;
        private List<Vector2> _positions;
        private int _currentPositionId = 0;

        private float _currentTime = 0f;
        private Vector2 _startTouchPosition;
        private bool _swipeStarted = false;
        private float _swipeThreshold = 50f;

        public override async UniTask Initialize(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;

            await LoadBanners();
            SetBannersPosition();
        }

        private async UniTask LoadBanners()
        {
            List<GameObject> bannersPrefabs = await _assetsProvider.LoadAllAsync<GameObject>("MenuBanner");
            _banners = new List<GameObject>();
            _banners.AddRange(bannersPrefabs.Select(x => Instantiate(x, _container)));
        }

        private void SetBannersPosition()
        {
            _positions = new List<Vector2>();
            for (int i = 0; i < _banners.Count; i++)
            {
                float rectWidth = transform.GetComponent<RectTransform>().rect.width;
                _banners[i].transform.localPosition = new Vector2(rectWidth * i, 0);

                Vector3 targetPos = new Vector2(-1 * rectWidth * i, 0);
                _positions.Add(targetPos);
            }
        }


        private void Update()
        {
            if (_banners != null && _positions?.Count > 0)
            {
                HandleInput();
                CheckAutoSwipe();
            }
        }

        private void HandleInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (IsIn(touch.position))
                    {
                        _startTouchPosition = touch.position;
                        _swipeStarted = true;
                    }
                    else _swipeStarted = false;
                }

                if (_swipeStarted && touch.phase == TouchPhase.Ended)
                {
                    var swipeDelta = (Vector2)(touch.position - _startTouchPosition);

                    if (swipeDelta.x < -_swipeThreshold)
                    {
                        _currentTime = 0f;
                        MoveNext();
                    }
                    else if (swipeDelta.x > _swipeThreshold)
                    {
                        _currentTime = 0f;
                        MovePrevious();
                    }
                }
            }
        }

        private bool IsIn(Vector2 position)
        {
            return position.x > _leftBorder.position.x &&
                position.x < _rightBorder.position.x &&
                position.y < _upBorder.position.y &&
                position.y > _downBorder.position.y;
        }

        private void CheckAutoSwipe()
        {
            if (_currentTime >= _swipeCooldown)
            {
                _currentTime = 0f;
                MoveNext();
            }
            else _currentTime += Time.deltaTime;
        }

        private void MoveNext()
        {
            _currentPositionId = _currentPositionId + 1 >= _positions.Count ? 0 : _currentPositionId + 1;
            MoveContentTo(_positions[_currentPositionId]);
        }

        private void MovePrevious()
        {
            _currentPositionId = _currentPositionId - 1 < 0 ? _positions.Count - 1 : _currentPositionId - 1;
            MoveContentTo(_positions[_currentPositionId]);
        }

        private void MoveContentTo(Vector2 position)
        {
            Transform dotNext = _activeVisualDots[_currentPositionId];
            Vector3 activePos = dotNext.position;
            int activeDotId = _activeVisualDots.IndexOf(_activeDot);
            _activeVisualDots[_currentPositionId].position = _activeDot.position;
            _activeDot.position = activePos;

            _activeVisualDots[activeDotId] = dotNext;
            _activeVisualDots[_currentPositionId] = _activeDot;

            _container
                .DOLocalMove(position, _contentMoveDuration)
                .SetEase(Ease.InOutSine);
        }
    }
}
