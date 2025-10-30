using System;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using BS.GameObjects;
using System.Threading;

namespace BS.System
{
    public class TimeSystem : ISystem
    {
        private static TimeSystem _instance;
        public static TimeSystem Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<TimeSystem>();
                }

                return _instance;
            }
        }

        private CancellationTokenSource _timeSpeedUpCancellationTokenSource;

        public void TimeSpeedUp(float speedMultiplier, float duration = -1f)
        {
            if(duration > 0f)
            {
                if(_timeSpeedUpCancellationTokenSource != null)
                {
                    _timeSpeedUpCancellationTokenSource.Cancel();
                    _timeSpeedUpCancellationTokenSource.Dispose();
                }

                _timeSpeedUpCancellationTokenSource = new CancellationTokenSource();

                TimeSpeedUpDurationAsync(speedMultiplier, duration, _timeSpeedUpCancellationTokenSource.Token).Forget();
            }
            else
            {
                Time.timeScale = speedMultiplier;
            }
        }

        private async UniTask TimeSpeedUpDurationAsync(float speedMultiplier, float duration, CancellationToken cancellationToken)
        {
            Time.timeScale = speedMultiplier;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), ignoreTimeScale: true, cancellationToken: cancellationToken, cancelImmediately : true);
            Time.timeScale = 1f;
        }
    }
}
