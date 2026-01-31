using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Gallery
{
    public class GalleryCell : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private Vector2 _topPoint;
        private Transform _transform;
        private bool _invoked = false;
        private bool _inited = false;

        private readonly Subject<Unit> _onOverScreen = new Subject<Unit>();
        public IObservable<Unit> OnOverScreen => _onOverScreen.AsObservable();

        public void Init(Sprite sprite, Vector2 topPoint)
        {
            _image.sprite = sprite;
            _topPoint = topPoint;
            _transform = GetComponent<Transform>();
            _inited = true;
        }

        public void Update()
        {
            if (!_invoked && _inited)
            {
                if (_transform.position.y > _topPoint.y)
                {
                    _onOverScreen.OnNext(Unit.Default);
                    _onOverScreen.OnCompleted();
                    _invoked = true;
                }
            }
        }
    }
}