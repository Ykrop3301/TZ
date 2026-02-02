using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Gallery
{
    public class PicturePopup : Popup
    {
        
        [SerializeField] private Image _image;

        private bool _isShowing = false;
        public void ShowPicture(Sprite sprite)
        {
            if (!_isShowing)
            {
                _isShowing = true;
                _image.sprite = sprite;
                Show().Forget();
            }
        }

        public void HidePicture()
        {
            _isShowing = false;
            Hide().Forget();
        }
    }
}