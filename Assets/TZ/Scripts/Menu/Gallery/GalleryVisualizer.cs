using Common.AssetsProvider;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace Menu.Gallery
{
    public class GalleryVisualizer : MenuElement
    {
        [SerializeField] private Transform _topPoint;
        [SerializeField] private int _startCellsCount = 10;
        [SerializeField] private Transform _content;
        private IAssetsProvider _assetsProvider;
        private GalleryCell _galleryCellPrefab;
        private List<GalleryCell> _cells;
        private int _cellsCount = 0;
        private CompositeDisposable disposables = new CompositeDisposable();

        public override async UniTask Initialize(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
            _galleryCellPrefab = (await _assetsProvider.LoadAsync<GameObject>("GalleryCell")).GetComponent<GalleryCell>();

            _cells = new List<GalleryCell>();
            var tasks = new List<UniTask>();

            for (int i = 0; i < _startCellsCount; i++)
            {
                tasks.Add(LoadNewCell());
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask LoadNewCell()
        {
            GalleryCell newCell = null;
            _cellsCount++;
            Sprite sprite = null;
            try
            {
                sprite = await _assetsProvider.LoadAsync<Sprite>("remote:" + $"http://data.ikppbb.com/test-task-unity-data/pics/{_cellsCount}.jpg");
            }
            catch
            {
                _cellsCount--;
                return;
            }
            AsyncInstantiateOperation<GalleryCell> handle = GameObject.InstantiateAsync(_galleryCellPrefab, 1, _content);
            await handle;
            newCell = handle.Result[0];
            _cells.Add(newCell);

            newCell.Init(sprite, _topPoint.position);
            newCell.OnOverScreen.Subscribe(_ =>
            {
                for (int i = 0; i < 2; i++)
                {
                    LoadNewCell().Forget();
                }
            });
        }

        private void OnDisable()
        {
            disposables.Dispose();
        }

        public void SortByEven()
        {
            ShowAll();
            for (int i = 0; i < _cells.Count; i++)
            {
                if (i % 2 != 0)
                    _cells[i].gameObject.SetActive(false);
            }
        }
        public void SortByOdd()
        {
            ShowAll();
            for (int i = 0; i < _cells.Count; i++)
            {
                if (i % 2 == 0)
                    _cells[i].gameObject.SetActive(true);
            }
        }
        public void ShowAll()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].gameObject.SetActive(true);
            }
        }
    }
}