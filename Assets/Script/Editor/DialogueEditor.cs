using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueScriptableObj))]
public class DialogueEditor : Editor
{
    //SerializedProperty quest;
    private void OnEnable()
    {
        //quest = serializedObject.FindProperty("quest");
    }
    public override void OnInspectorGUI()
    {
        //int maxWidth = 130;
        base.OnInspectorGUI();
        /*DialogueScriptableObj obj = (DialogueScriptableObj)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("對話名稱", "對話名稱只是用於開發分類，在腳本中沒有用途。"), GUILayout.MaxWidth(maxWidth));
        obj.dialogueName = EditorGUILayout.TextField(obj.dialogueName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("NPC名稱", "請選擇執行這個對話的NPC，非常重要。"), GUILayout.MaxWidth(maxWidth));
        //obj.NpcName = EditorGUILayout.EnumPopup(obj.NpcName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("對話是否不斷重複", "若勾選不斷重複，請將本對話放在NPC陣列中的最後一項。"), GUILayout.MaxWidth(maxWidth));
        //obj.repeatedly = EditorGUILayout.Toggle(obj.repeatedly);

        EditorGUILayout.LabelField(new GUIContent("是否給予任務", "勾選才會顯示任務詳細列表"), GUILayout.MaxWidth(maxWidth));
        obj.haveQuest = EditorGUILayout.Toggle(obj.haveQuest);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (obj.haveQuest)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("NPC名稱", "請選擇執行這個對話的NPC，非常重要。"), GUILayout.MaxWidth(maxWidth));
            //EditorGUILayout.BoundsField(quest);
            EditorGUILayout.EndHorizontal();
        }*/
    }
}

