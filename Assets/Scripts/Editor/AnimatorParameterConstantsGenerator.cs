using UnityEditor.Animations;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEditor;
namespace BS.Editor
{
    public class AnimatorParameterConstantsGenerator : AssetPostprocessor
    {
        private const string ANIM_PARAM_CONSTANTS_FILE_PATH = "Assets/Scripts/AnimParamConstants.cs";
        // 검색 루트 폴더: 하위까지 포함
        private const string SEARCH_ROOT = "Assets/AddressResource/Characters/ResourceAsset";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            return;
            bool hasAnimatorChanges = false;

            foreach (string path in importedAssets)
            {
                if (path.EndsWith(".controller"))
                {
                    hasAnimatorChanges = true;
                    break;
                }
            }

            if (!hasAnimatorChanges)
            {
                foreach (string path in deletedAssets)
                {
                    if (path.EndsWith(".controller"))
                    {
                        hasAnimatorChanges = true;
                        break;
                    }
                }
            }

            if (hasAnimatorChanges)
            {
                GenerateAnimatorParameterConstants();
            }
        }

        [MenuItem("ProjectBS/Tools/Generate Animator Parameter Constants")]
        public static void GenerateAnimatorParameterConstantsMenu()
        {
            GenerateAnimatorParameterConstants();
            Debug.Log("Animator Parameter and State Name Constants generated successfully!");
        }

        /// <summary>
        /// 모든 AnimatorController의 파라미터와 상태 이름을 수집하고 AnimParamConstants.cs 생성
        /// </summary>
        private static void GenerateAnimatorParameterConstants()
        {
            if (!AssetDatabase.IsValidFolder(SEARCH_ROOT))
            {
                Debug.LogWarning($"AnimatorParameterConstantsGenerator: 대상 폴더를 찾지 못했습니다: {SEARCH_ROOT}");
            }

            // 폴더 범위로 검색(하위 포함). 대소문자/경로 이슈 방지
            var guids = new List<string>();
            guids.AddRange(AssetDatabase.FindAssets("t:AnimatorController", new[] { SEARCH_ROOT }));
            guids.AddRange(AssetDatabase.FindAssets("t:AnimatorOverrideController", new[] { SEARCH_ROOT }));
            guids = guids.Distinct().ToList();

            var allParameters = new HashSet<string>(StringComparer.Ordinal);
            var allStateNames = new HashSet<string>(StringComparer.Ordinal);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                // 우선 기본 AnimatorController 로드
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);

                // 오버라이드 컨트롤러인 경우, 기반 컨트롤러 추출
                if (controller == null)
                {
                    var overrideCtrl = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(path);
                    controller = overrideCtrl?.runtimeAnimatorController as AnimatorController;
                }

                if (controller == null)
                    continue;

                // 파라미터 수집
                foreach (var param in controller.parameters)
                {
                    if (!string.IsNullOrEmpty(param.name))
                    {
                        allParameters.Add(param.name);
                    }
                }

                // 상태 이름 수집
                CollectStateNames(controller, allStateNames);
            }

            // AnimParamConstants.cs 파일 생성/업데이트
            GenerateAnimParamConstantsFile(allParameters, allStateNames);
        }

        private static void CollectStateNames(AnimatorController controller, HashSet<string> stateNames)
        {
            foreach (var layer in controller.layers)
            {
                CollectStateNamesFromStateMachine(layer.stateMachine, stateNames);
            }
        }

        private static void CollectStateNamesFromStateMachine(AnimatorStateMachine stateMachine, HashSet<string> stateNames)
        {
            if (stateMachine == null)
                return;

            foreach (var state in stateMachine.states)
            {
                if (state.state != null && !string.IsNullOrEmpty(state.state.name))
                {
                    stateNames.Add(state.state.name);
                }
            }

            foreach (var subStateMachine in stateMachine.stateMachines)
            {
                CollectStateNamesFromStateMachine(subStateMachine.stateMachine, stateNames);
            }
        }

        private static void GenerateAnimParamConstantsFile(HashSet<string> parameters, HashSet<string> stateNames)
        {
            StringBuilder fileContent = new StringBuilder();

            fileContent.AppendLine("// Auto-generated file. Do not modify manually.");
            fileContent.AppendLine("// This file is automatically generated from AnimatorController assets.");
            fileContent.AppendLine();
            fileContent.AppendLine("namespace BS.Common");
            fileContent.AppendLine("{");
            fileContent.AppendLine("    /// <summary>");
            fileContent.AppendLine("    /// Animator Parameter Constants");
            fileContent.AppendLine("    /// Auto-generated from all AnimatorController assets in the project");
            fileContent.AppendLine("    /// </summary>");
            fileContent.AppendLine("    public static class AnimParamConstants");
            fileContent.AppendLine("    {");

            var sortedParams = parameters.OrderBy(p => p).ToList();
            foreach (string param in sortedParams)
            {
                string constName = ConvertToConstantName(param);
                fileContent.AppendLine($"        public const string {constName} = \"{param}\";");
            }

            fileContent.AppendLine("    }");
            fileContent.AppendLine();
            fileContent.AppendLine("    /// <summary>");
            fileContent.AppendLine("    /// Animation State Name Constants");
            fileContent.AppendLine("    /// Auto-generated from all AnimatorController assets in the project");
            fileContent.AppendLine("    /// </summary>");
            fileContent.AppendLine("    public static class AnimStateConstants");
            fileContent.AppendLine("    {");

            var sortedStates = stateNames.OrderBy(s => s).ToList();
            foreach (string stateName in sortedStates)
            {
                string constName = ConvertToConstantName(stateName);
                fileContent.AppendLine($"        public const string {constName} = \"{stateName}\";");
            }

            fileContent.AppendLine("    }");
            fileContent.AppendLine("}");

            string directory = Path.GetDirectoryName(ANIM_PARAM_CONSTANTS_FILE_PATH);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(ANIM_PARAM_CONSTANTS_FILE_PATH, fileContent.ToString());
            AssetDatabase.Refresh();

            Debug.Log($"Generated AnimParamConstants.cs with {parameters.Count} animator parameter constants and {stateNames.Count} state name constants");
        }

        private static string ConvertToConstantName(string paramName)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < paramName.Length; i++)
            {
                char c = paramName[i];

                if (char.IsUpper(c) && i > 0 && char.IsLower(paramName[i - 1]))
                {
                    result.Append('_');
                }

                result.Append(char.ToUpper(c));
            }

            return result.ToString();
        }
    }
}
