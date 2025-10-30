using System;
using System.Collections.Generic;
using UnityEngine;
using BS.Common;
using BS.GameObjects;
using BS.UI;

namespace BS.System
{
    public class DataSystem : ISystem
    {
        private static DataSystem _instance;

        public static DataSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<DataSystem>();
                }
                return _instance;
            }
        }

        private int _playerHighScore;
        public int PlayerHighScore
        {
            get { return _playerHighScore; }
            set { _playerHighScore = value; }
        }

        private int _playerGold;
        public int PlayerGold
        {
            get { return _playerGold; }
            set { _playerGold = value; }
        }

        public void Load()
        {

        }

        public void Unload()
        {

        }

        public void AddReward(AbstractRewardableObject rewardableObject)
        {
            // TODO :: 보상아이템에 따른 보상 처리 
            if(rewardableObject is GoldCoinObject)
            {
                PlayerGold += 10; // 예시: 골드 코인 획득 시 골드 1 증가
                Debug.Log($"Gold Coin Collected! Total Gold: {PlayerGold}");
                var hud = UISystem.Instance.GetPresenter<HUDUIPresenter>(); 
                hud.UpdateGoldText(PlayerGold);
            }
            else
            {
                // TODO :: 일반 보상 아이템 처리
            }
        }
    }
}