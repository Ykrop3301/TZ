using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Menu.Gallery
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected float _showDuration = 0.5f;
        protected async UniTask Show()
        {
            gameObject.SetActive(true);

            await _canvasGroup.DOFade(1f, _showDuration).From(0).AsyncWaitForCompletion();
        }

        protected async UniTask Hide()
        {
            await _canvasGroup.DOFade(0f, _showDuration).AsyncWaitForCompletion();

            gameObject.SetActive(false);
        }
    }
}
