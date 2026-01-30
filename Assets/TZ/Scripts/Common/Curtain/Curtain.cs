using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Common.Curtain
{
    public class Curtain : MonoBehaviour, ICurtain
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _showDuration = 1f;

        public async UniTask Show()
        {
            gameObject.SetActive(true);

            await _canvasGroup.DOFade(1f, _showDuration).AsyncWaitForCompletion();
        }

        public async UniTask Hide()
        {
            await _canvasGroup.DOFade(0f, _showDuration).AsyncWaitForCompletion();
            
            gameObject.SetActive(false);
        }
    }
}