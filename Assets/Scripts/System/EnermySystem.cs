using UnityEngine;
using UnityEngine.Pool;
using BS.Common;
using BS.GameObjects;
using System;
using System.Collections.Generic;

namespace BS.System
{
    public class EnermySystem : ISystem
    {
        private static EnermySystem _instance;

        public static EnermySystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<EnermySystem>();
                }
                return _instance;
            }
        }

        private bool _isInitialize = false;
        private Dictionary<string, ObjectPool<AbstractCharacter>> _enemyPools = new Dictionary<string, ObjectPool<AbstractCharacter>>();

        private HashSet<AbstractCharacter> _activeEnemies = new HashSet<AbstractCharacter>();

        public void Load()
        {
            Initialize();
        }

        public void Unload()
        {
            Release();
        }

        private void Initialize()
        {
            if (!_isInitialize)
            {
                _isInitialize = true;
            }
        }

        private void Release()
        {
            if (_isInitialize)
            {
                // 풀을 클리어하기 전에 모든 활성 적 해제
                using (var pooledSet = HashSetPool<AbstractCharacter>.Get(out var enemiesToRelease))
                {
                    enemiesToRelease.UnionWith(_activeEnemies);

                    foreach (var enemy in enemiesToRelease)
                    {
                        if (enemy != null)
                        {
                            enemy.gameObject.SetActive(false);
                        }
                    }
                }

                _activeEnemies.Clear();
                _enemyPools.Clear();
                _isInitialize = false;
            }
        }

        /// <summary>
        /// 지정된 적 타입에 대한 적 풀을 생성하거나 가져옵니다
        /// </summary>
        /// <param name="enemyName">Resources의 적 프리팹 이름</param>
        /// <returns>지정된 적 타입에 대한 ObjectPool</returns>
        private ObjectPool<AbstractCharacter> GetOrCreatePool(string enemyName)
        {
            if (_enemyPools.ContainsKey(enemyName))
            {
                return _enemyPools[enemyName];
            }

            ObjectPool<AbstractCharacter> pool = new ObjectPool<AbstractCharacter>
             (
                   createFunc: () => OnCreateEnemy(enemyName),
           actionOnGet: OnGetEnemy,
           actionOnRelease: OnReleaseEnemy,
               actionOnDestroy: OnDestroyEnemy
             );

            _enemyPools.Add(enemyName, pool);
            return pool;
        }

        private AbstractCharacter OnCreateEnemy(string enemyName)
        {
            var loadObject = ResourceSystem.Instance.GetLoadGameObject(enemyName);
            if (loadObject != null)
            {
                loadObject = UnityEngine.GameObject.Instantiate(loadObject);
                if (loadObject.TryGetComponent<AbstractCharacter>(out var result))
                {
                    loadObject.tag = Constrants.TAG_ENERMY;
                    return result;
                }
            }

            Debug.LogError($"Failed to create enemy: {enemyName}");
            return null;
        }

        private void OnGetEnemy(AbstractCharacter enemy)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
                enemy.Initialize();
                _activeEnemies.Add(enemy);
            }
        }

        private void OnReleaseEnemy(AbstractCharacter enemy)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(false);
                _activeEnemies.Remove(enemy);
            }
        }

        private void OnDestroyEnemy(AbstractCharacter enemy)
        {
            if (enemy != null)
            {
                _activeEnemies.Remove(enemy);
                UnityEngine.GameObject.Destroy(enemy.gameObject);
            }
        }

        /// <summary>
        /// 풀에서 적을 가져옵니다
        /// </summary>
        /// <param name="enemyName">적 프리팹 이름</param>
        /// <param name="position">스폰 위치</param>
        /// <returns>스폰된 적 캐릭터</returns>
        public AbstractCharacter GetEnemy(string enemyName, Vector3 position)
        {
            var pool = GetOrCreatePool(enemyName);
            var enemy = pool.Get();

            if (enemy != null)
            {
                enemy.transform.position = position;
            }

            return enemy;
        }

        /// <summary>
        /// 회전값과 함께 풀에서 적을 가져옵니다
        /// </summary>
        /// <param name="enemyName">적 프리팹 이름</param>
        /// <param name="position">스폰 위치</param>
        /// <param name="rotation">스폰 회전값</param>
        /// <returns>스폰된 적 캐릭터</returns>
        public AbstractCharacter GetEnemy(string enemyName, Vector3 position, Quaternion rotation)
        {
            var enemy = GetEnemy(enemyName, position);

            if (enemy != null)
            {
                enemy.transform.rotation = rotation;
            }

            return enemy;
        }

        /// <summary>
        /// ListPool을 사용하여 메모리 효율적으로 여러 적을 한 번에 스폰합니다
        /// </summary>
        /// <param name="enemyName">적 프리팹 이름</param>
        /// <param name="positions">스폰 위치 배열</param>
        /// <returns>스폰된 적들의 리스트 (수동으로 풀에 반환해야 함)</returns>
        public List<AbstractCharacter> GetEnemies(string enemyName, Vector3[] positions)
        {
            var enemies = ListPool<AbstractCharacter>.Get();

            foreach (var position in positions)
            {
                var enemy = GetEnemy(enemyName, position);
                if (enemy != null)
                {
                    enemies.Add(enemy);
                }
            }

            return enemies;
        }

        /// <summary>
        /// 적을 풀로 반환합니다
        /// </summary>
        /// <param name="enemyName">적 프리팹 이름</param>
        /// <param name="enemy">반환할 적</param>
        public void ReleaseEnemy(string enemyName, AbstractCharacter enemy)
        {
            if (_enemyPools.ContainsKey(enemyName))
            {
                _enemyPools[enemyName].Release(enemy);
            }
            else
            {
                Debug.LogWarning($"Pool for enemy '{enemyName}' not found. Destroying enemy instead.");
                _activeEnemies.Remove(enemy);
                UnityEngine.GameObject.Destroy(enemy.gameObject);
            }
        }

        /// <summary>
        /// ListPool을 사용하여 여러 적을 풀로 반환합니다
        /// </summary>
        /// <param name="enemyName">적 프리팹 이름</param>
        /// <param name="enemies">반환할 적들의 리스트 (ListPool로 반환됨)</param>
        public void ReleaseEnemies(string enemyName, List<AbstractCharacter> enemies)
        {
            if (enemies == null) return;

            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    ReleaseEnemy(enemyName, enemy);
                }
            }

            ListPool<AbstractCharacter>.Release(enemies);
        }

        /// <summary>
        /// 현재 활성화된 모든 적을 가져옵니다
        /// </summary>
        /// <returns>활성 적들의 읽기 전용 컬렉션</returns>
        public IReadOnlyCollection<AbstractCharacter> GetActiveEnemies()
        {
            return _activeEnemies;
        }

        /// <summary>
        /// 활성 적의 수를 가져옵니다
        /// </summary>
        public int ActiveEnemyCount => _activeEnemies.Count;

        /// <summary>
        /// 특정 적 풀을 클리어합니다
        /// </summary>
        /// <param name="enemyName">클리어할 적 풀의 이름</param>
        public void ClearPool(string enemyName)
        {
            if (_enemyPools.ContainsKey(enemyName))
            {
                _enemyPools[enemyName].Clear();
                _enemyPools.Remove(enemyName);
            }
        }

        /// <summary>
        /// 모든 적 풀을 클리어합니다
        /// </summary>
        public void ClearAllPools()
        {
            foreach (var pool in _enemyPools.Values)
            {
                pool.Clear();
            }
            _enemyPools.Clear();
        }

        /// <summary>
        /// 모든 활성 적을 풀로 반환합니다
        /// </summary>
        public void ReleaseAllActiveEnemies()
        {
            using (var pooledList = ListPool<AbstractCharacter>.Get(out var enemiesToRelease))
            {
                enemiesToRelease.AddRange(_activeEnemies);

                foreach (var enemy in enemiesToRelease)
                {
                    if (enemy != null && enemy.gameObject != null)
                    {
                        // 이 적이 속한 풀 찾기
                        string enemyType = enemy.GetType().Name;
                        ReleaseEnemy(enemyType, enemy);
                    }
                }
            }
        }
    }
}