using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BS.Common;
using BS.GameObjects;
using BS.System;
using DG.Tweening;

namespace BS.GameObjects
{
    public class RewardableObjectSystem : ISystem
    {
        private static RewardableObjectSystem _instance;
        public static RewardableObjectSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<RewardableObjectSystem>();
                }
                return _instance;
            }
        }

        private Dictionary<string, AbstractRewardableObject> _originAddresableRewardDict = new Dictionary<string, AbstractRewardableObject>();
        
        private Dictionary<string , ObjectPool<AbstractRewardableObject>> _rewardableObjectPoolDict = new Dictionary<string, ObjectPool<AbstractRewardableObject>>();

        public void Load()
        {

        }

        public void Unload()
                    {
            foreach(var pool in _rewardableObjectPoolDict.Values)
            {
                pool.Clear();
            }
            _rewardableObjectPoolDict.Clear();
            _originAddresableRewardDict.Clear();
        }

        public void SpawnRewardableObject(string addressablePath, Vector3 position)
        {
            if(!_originAddresableRewardDict.ContainsKey(addressablePath))
            {
                var loadGameObject = ResourceSystem.Instance.GetLoadGameObject(addressablePath);
                if(loadGameObject != null)
                {
                    if(loadGameObject.TryGetComponent<AbstractRewardableObject>(out var loadedGameObject))
                    {
                        _originAddresableRewardDict[addressablePath] = loadedGameObject;
                    }
                }
            }
            if(_originAddresableRewardDict.ContainsKey(addressablePath))
            {
                if(!_rewardableObjectPoolDict.ContainsKey(addressablePath))
                {
                    var originalPrefab = _originAddresableRewardDict[addressablePath];
                    var newPool = new ObjectPool<AbstractRewardableObject>(
                        createFunc: () => GameObject.Instantiate(originalPrefab),
                        actionOnGet: (obj) => obj.gameObject.SetActive(true),
                        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                        actionOnDestroy: (obj) => GameObject.Destroy(obj.gameObject),
                        collectionCheck: false,
                        defaultCapacity: 5,
                        maxSize: 50
                    );
                    _rewardableObjectPoolDict[addressablePath] = newPool;
                }
                var rewardableObject = _rewardableObjectPoolDict[addressablePath].Get();
                if (rewardableObject != null)
                {
                    rewardableObject
                        .SetParentPool(_rewardableObjectPoolDict[addressablePath])
                        .SetPosition(position)
                        .Spawn();
                }
            }
        }

        public void PlayGainRewardEffect(AbstractRewardableObject rewardableObject, Vector2 destScreenPosition)
        {
            rewardableObject.transform.DOMove(destScreenPosition, 0.5f).SetEase(Ease.InQuad)
                .OnComplete(()=>
                {
                    rewardableObject.Release();
                });
        }
    }
}