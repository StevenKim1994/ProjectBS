using UnityEngine;
using UnityEngine.AddressableAssets;
using BS.Common;
using BS.GameObjects;
namespace BS.System
{
    public class ResourceSystem : ISystem
    {
        private static ResourceSystem _instance;
        public static ResourceSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<ResourceSystem>();
                }

                return _instance;
            }
        }

        public void Load()
        {
            Addressables.InitializeAsync().WaitForCompletion();
        }

        public void Unload()
        {

        }

        public GameObject GetLoadGameObject(string addressPath) 
        {
            var oper = Addressables.LoadAssetAsync<GameObject>(addressPath);
            oper.WaitForCompletion();
            return oper.Result;
        }
    }
}