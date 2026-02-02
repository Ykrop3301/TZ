using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Gallery
{
    public class GalleryCell : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _premiumFlag;
        private Vector2 _topPoint;
        private Transform _transform;
        private PicturePopup _picturePopup;
        private PremiumPopup _premiumPopup;
        private bool _invoked = false;
        private bool _inited = false;
        private bool _isPremium = false;

        private readonly Subject<Unit> _onOverScreen = new Subject<Unit>();
        public IObservable<Unit> OnOverScreen => _onOverScreen.AsObservable();

        public void Init(Sprite sprite, Vector2 topPoint, PicturePopup picturePopup, PremiumPopup premiumPopup, bool isPremium)
        {
            _image.sprite = sprite;
            _topPoint = topPoint;
            _transform = GetComponent<Transform>();
            _picturePopup = picturePopup;
            _premiumPopup = premiumPopup;
            _inited = true;
            _isPremium = isPremium;
            _premiumFlag.SetActive(_isPremium);
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

        public void OnClick()
        {
            if (_isPremium)
            {
                _premiumPopup.ShowPopup();
            }
            else
            {
                _picturePopup.ShowPicture(_image.sprite);
            }
        }
    }
}