using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.AssetsProvider
{
    public class AddressablesAssetsProvider : IAssetsProvider
    {
        public async UniTask<List<T>> LoadAllAsync<T>(string label) where T : Object
        {
            var handle = Addressables.LoadAssetsAsync<T>(label, null);
            var assets = await handle.Task;
            return assets.ToList();
        }

        public async UniTask<T> LoadAsync<T>(string id) where T : Object
        {
            return await Addressables.LoadAssetAsync<T>(id);
        }
    }
}
