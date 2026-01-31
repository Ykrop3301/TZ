using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Common.AssetsProvider
{
    public interface IAssetsProvider
    {
        public UniTask<T> LoadAsync<T>(string id, CancellationToken cancellationToken = default) where T : UnityEngine.Object;
        public UniTask<List<T>> LoadAllAsync<T>(string label) where T : Object;
    }
}
