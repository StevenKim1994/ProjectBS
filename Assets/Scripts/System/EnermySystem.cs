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
        private Dictionary<string, ObjectPool<AbstractEnermy>> _enemyPools = new Dictionary<string, ObjectPool<AbstractEnermy>>();
        private HashSet<AbstractEnermy> _activeEnemies = new HashSet<AbstractEnermy>();
        
        // ✅ Enemy Collider 추적을 위한 리스트
        private List<Collider2D> _enemyColliders = new List<Collider2D>();

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
                using (var pooledSet = HashSetPool<AbstractEnermy>.Get(out var enemiesToRelease))
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
                _enemyColliders.Clear();
                _enemyPools.Clear();
                _isInitialize = false;
            }
        }

        /// <summary>
        /// 지정된 적 타입에 대한 적 풀을 생성하거나 가져옵니다
        /// </summary>
        /// <param name="enemyName">Resources의 적 프리팹 이름</param>
        /// <returns>지정된 적 타입에 대한 ObjectPool</returns>
        private ObjectPool<AbstractEnermy> GetOrCreatePool(string enemyName)
        {
            if (_enemyPools.ContainsKey(enemyName))
            {
                return _enemyPools[enemyName];
            }

            ObjectPool<AbstractEnermy> pool = new ObjectPool<AbstractEnermy>
             (
                    createFunc: () => OnCreateEnemy(enemyName),
                    actionOnGet: OnGetEnemy,
                    actionOnRelease: OnReleaseEnemy,
                    actionOnDestroy: OnDestroyEnemy
             );

            _enemyPools.Add(enemyName, pool);
            return pool;
        }

        private AbstractEnermy OnCreateEnemy(string enemyName)
        {
            var loadObject = ResourceSystem.Instance.GetLoadGameObject(enemyName);
            if (loadObject != null)
            {
                loadObject = UnityEngine.GameObject.Instantiate(loadObject);
                if (loadObject.TryGetComponent<AbstractEnermy>(out var result))
                {
                    loadObject.tag = Constrants.TAG_ENERMY;
                    if(_enemyPools.ContainsKey(enemyName))
                    {
                        result.SetParentPool(_enemyPools[enemyName]);
                    }
                    return result;
                }
            }

            Debug.LogError($"Failed to create enemy: {enemyName}");
            return null;
        }

        private void OnGetEnemy(AbstractEnermy enemy)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
                enemy.Initialize();
                _activeEnemies.Add(enemy);
                
                // ✅ 새 Enemy와 기존 모든 Enemy 간 충돌 무시
                if (enemy.Collider != null)
                {
                    IgnoreCollisionWithOtherEnemies(enemy.Collider);
                    _enemyColliders.Add(enemy.Collider);
                }
            }
        }

        private void OnReleaseEnemy(AbstractEnermy enemy)
        {
            if (enemy != null)
            {
                if (enemy.Collider != null)
                {
                    RestoreCollisionWithOtherEnemies(enemy.Collider);
                    _enemyColliders.Remove(enemy.Collider);
                }
                
                enemy.gameObject.SetActive(false);
                _activeEnemies.Remove(enemy);
            }
        }

        private void OnDestroyEnemy(AbstractEnermy enemy)
        {
            if (enemy != null)
            {
                if (enemy.Collider != null)
                {
                    _enemyColliders.Remove(enemy.Collider);
                }
                
                _activeEnemies.Remove(enemy);
                UnityEngine.GameObject.Destroy(enemy.gameObject);
            }
        }

        /// <summary>
        /// 특정 Enemy Collider와 다른 모든 Enemy Collider 간 충돌을 무시합니다
        /// </summary>
        private void IgnoreCollisionWithOtherEnemies(Collider2D newEnemyCollider)
        {
            foreach (var existingCollider in _enemyColliders)
            {
                if (existingCollider != null && existingCollider != newEnemyCollider)
                {
                    Physics2D.IgnoreCollision(newEnemyCollider, existingCollider, true);
                }
            }
        }

        /// <summary>
        /// 특정 Enemy Collider와 다른 모든 Enemy Collider 간 충돌 무시를 해제합니다
        /// </summary>
        private void RestoreCollisionWithOtherEnemies(Collider2D enemyCollider)
        {
            foreach (var otherCollider in _enemyColliders)
            {
                if (otherCollider != null && otherCollider != enemyCollider)
                {
                    Physics2D.IgnoreCollision(enemyCollider, otherCollider, false);
                }
            }
        }

        /// <summary>
        /// 풀에서 적을 가져옵니다
        /// </summary>
        /// <param name="enemyName">적 프리팹 이름</param>
        /// <param name="position">스폰 위치</param>
        /// <returns>스폰된 적 캐릭터</returns>
        public AbstractEnermy GetEnemy(string enemyName, Vector3 position)
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
        public AbstractEnermy GetEnemy(string enemyName, Vector3 position, Quaternion rotation)
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
        public void ReleaseEnemy(string enemyName, AbstractEnermy enemy)
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
        public void ReleaseEnemies(string enemyName, List<AbstractEnermy> enemies)
        {
            if (enemies == null) return;

            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    ReleaseEnemy(enemyName, enemy);
                }
            }

            ListPool<AbstractEnermy>.Release(enemies);
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
            _enemyColliders.Clear();
        }

        /// <summary>
        /// 모든 활성 적을 풀로 반환합니다
        /// </summary>
        public void ReleaseAllActiveEnemies()
        {
            using (var pooledList = ListPool<AbstractEnermy>.Get(out var enemiesToRelease))
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