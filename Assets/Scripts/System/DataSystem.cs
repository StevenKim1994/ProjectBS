using System;
using System.Collections.Generic;
using UnityEngine;
using BS.Common;
using BS.GameObjects;

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

        }
    }
}