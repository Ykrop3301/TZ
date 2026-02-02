using Cysharp.Threading.Tasks;

namespace Menu.Gallery
{
    public class PremiumPopup : Popup
    {
        private bool _isShowing = false;
        public void ShowPopup()
        {
            if (!_isShowing)
            {
                _isShowing = true;
                Show().Forget();
            }
        }

        public void HidePopup()
        {
            _isShowing = false;
            Hide().Forget();
        }
    }
}