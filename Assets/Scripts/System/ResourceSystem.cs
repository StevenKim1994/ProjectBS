using UnityEngine;
using UnityEngine.AddressableAssets;
using BS.Common;

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

        public UnityEngine.GameObject GetLoadGameObject(string addressPath) 
        {
            var oper = Addressables.LoadAssetAsync<UnityEngine.GameObject>(Constrants.STR_DEFAULT_ADDRESSABLE_PATH + addressPath + Constrants.STR_POSTFIX_PREFAB);
            oper.WaitForCompletion();
            return oper.Result;
        }
    }
}