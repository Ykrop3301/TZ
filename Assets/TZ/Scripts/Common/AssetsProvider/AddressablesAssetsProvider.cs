using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace Common.AssetsProvider
{
    public class AddressablesAssetsProvider : IAssetsProvider
    {
        private readonly Dictionary<string, Sprite> _remoteSpriteCache = new Dictionary<string, Sprite>();

        public async UniTask<List<T>> LoadAllAsync<T>(string label) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetsAsync<T>(label, null);
            var assets = await handle.Task;
            return assets.ToList();
        }

        public async UniTask<T> LoadAsync<T>(string id, CancellationToken cancellationToken = default) where T : UnityEngine.Object
        {
            if (typeof(T) == typeof(Sprite) && id.StartsWith("remote:"))
            {
                string url = id.Substring(7);

                lock (_remoteSpriteCache)
                {
                    if (_remoteSpriteCache.TryGetValue(url, out var cached))
                    {
                        return cached as T;
                    }
                }

                var downloadHandler = new DownloadHandlerTexture(false); // false = НЕ уничтожать текстуру
                using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
                {
                    request.downloadHandler = downloadHandler;

                    await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        throw new Exception($"Ошибка загрузки: {request.error}");
                    }

                    Texture2D texture = downloadHandler.texture;
                    if (texture == null) throw new Exception("Null texture");

                    Sprite sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100f
                    );

                    // Защищаем от выгрузки при загрузке сцен
                    texture.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    sprite.hideFlags = HideFlags.DontUnloadUnusedAsset;

                    // ... кэширование ...
                    return sprite as T;
                } 
            }
            else
            {
                return await Addressables.LoadAssetAsync<T>(id).ToUniTask(cancellationToken: cancellationToken);
            }
        }
    }
}

