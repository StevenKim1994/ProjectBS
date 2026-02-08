using System;
using System.Collections.Generic;
using UnityEngine;
using BS.GameObjects;
using BS.Dialogue;

namespace BS.System
{
    public class DialogueSystem : ISystem
    {
        private static DialogueSystem _instance;
        public static DialogueSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<DialogueSystem>();
                }
                return _instance;
            }
        }

        private DialogueState _currentState = DialogueState.Idle;
        public DialogueState CurrentState => _currentState;

        private GameStepState _previousGameState;

        public event Action<DialogueState> OnDialogueStateChanged;

        public void Load()
        {
            _currentState = DialogueState.Idle;
        }

        public void Unload()
        {
            
        }

        /// <summary>
        /// NPC와의 대화를 시작합니다
        /// </summary>
        public void StartDialogue(string npcId, string dialogueDataPath)
        {
            if (_currentState == DialogueState.InDialogue)
            {
                Debug.LogWarning("Already in dialogue!");
                return;
            }

            // 게임 상태 일시정지
            _previousGameState = GameSequenceSystem.Instance.CurrentState;
            GameSequenceSystem.Instance.SetGameStepState(GameStepState.Paused);
            InputControlSystem.Instance.IsInput = false;

            _currentState = DialogueState.InDialogue;
            OnDialogueStateChanged?.Invoke(_currentState);

            // 대화 데이터 로드 및 시작
        }

        /// <summary>
        /// 현재 대화를 종료합니다
        /// </summary>
        public void EndDialogue()
        {
            if (_currentState != DialogueState.InDialogue)
                return;

            _currentState = DialogueState.Idle;
            OnDialogueStateChanged?.Invoke(_currentState);

            // 게임 상태 복원
            GameSequenceSystem.Instance.SetGameStepState(_previousGameState);
            InputControlSystem.Instance.IsInput = true;
        }

        /// <summary>
        /// 다음 대화 노드로 진행
        /// </summary>
        public void ProgressDialogue()
        {
            if (_currentState != DialogueState.InDialogue)
                return;

        }

        /// <summary>
        /// 선택지 선택
        /// </summary>
        public void SelectChoice(int choiceIndex)
        {
            if (_currentState != DialogueState.InDialogue)
                return;
        }

        /// <summary>
        /// 현재 대화 노드 정보 조회
        /// </summary>
        public DialogueNode GetCurrentNode()
        {
            return null; // TODO :: 수정필요
        }

        /// <summary>
        /// 현재 선택지 목록 조회
        /// </summary>
        public List<DialogueChoice> GetCurrentChoices()
        {
            return null; // TODO :: 수정필요
        }

        /// <summary>
        /// 대화 중인지 확인
        /// </summary>
        public bool IsInDialogue()
        {
            return _currentState == DialogueState.InDialogue;
        }
    }
}