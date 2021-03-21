using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Audio/AudioEventList")]
public class AudioEventListenerList : ScriptableObject
{
    public AudioEventListener[] audioEvents;
    private AudioManager _manager;

    public void RegisterEvents(AudioManager manager)
    {
        _manager = manager;

        for (int i = 0; i < audioEvents.Length; i++)
        {
            audioEvents[i].gameEvent.RegisterListener(audioEvents[i]);
        }
    }

    public void RaiseEvent(AudioEventListener listener)
    {
        switch (listener.type)
        {
            case AudioEventListener.AudioType.OneShot:
                _manager.PlayAudioShot(listener);
                break;
            case AudioEventListener.AudioType.Music:
                _manager.PlayMusic(listener);
                break;
            case AudioEventListener.AudioType.RandomOneShot:
                _manager.PlayAudioShot(listener, true);                
                break;
            default:
                break;
        }
    }

    public void OnDisable()
    {
        for (int i = 0; i < audioEvents.Length; i++)
        {
            audioEvents[i].gameEvent.Unregister(audioEvents[i]);
        }

/*        for (int i = 0; i < audioEvents.Length; i++)
        {
            audioEvents[i].gameEvent.ClearAudioListeners();
        }*/
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioEventListenerList))]
public class AudioEventListenerListEditor : Editor
{
    AudioEventListenerList _base;
    private const string _unassigned = "Unassigned Audio Event";
    private Vector2 _scrollPos;

    private void OnEnable()
    {
        _base = (AudioEventListenerList)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ShowList(serializedObject.FindProperty("audioEvents"));

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowList(SerializedProperty prop)
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, "box");

        prop.isExpanded = true;

        EditorGUILayout.LabelField(prop.displayName + " (" + prop.arraySize + ")", new GUIStyle("boldLabel"));

        EditorGUI.indentLevel++;

        for (int i = 0; i < prop.arraySize; i++)
        {
            EditorGUILayout.BeginVertical("box");

            string displayName = "";

            if (prop.GetArrayElementAtIndex(i).FindPropertyRelative("gameEvent").objectReferenceValue != null)
            {
                displayName = ((AudioEventListener.AudioType)prop.GetArrayElementAtIndex(i).FindPropertyRelative("type").enumValueIndex).ToString();
                displayName += " for " + prop.GetArrayElementAtIndex(i).FindPropertyRelative("gameEvent").objectReferenceValue.name + " game event";
            }
            else
            {
                displayName = _unassigned;
            }

            EditorGUILayout.BeginHorizontal();
            prop.GetArrayElementAtIndex(i).isExpanded = EditorGUILayout.Foldout(prop.GetArrayElementAtIndex(i).isExpanded, new GUIContent(displayName), true);

            if (GUILayout.Button("X", GUILayout.Width(50)))
            {
                prop.DeleteArrayElementAtIndex(i);
                return;
            }

            EditorGUILayout.EndHorizontal();

            if (displayName == _unassigned)
            {
                EditorGUILayout.HelpBox("Assign a game event!", MessageType.Warning);
            }

            GUILayout.Space(5f);

            if (prop.GetArrayElementAtIndex(i).isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i).FindPropertyRelative("type"));
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i).FindPropertyRelative("gameEvent"));
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i).FindPropertyRelative("audioClips"));
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i).FindPropertyRelative("replayTreshold"));
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i).FindPropertyRelative("delay"));
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i).FindPropertyRelative("volume"));
            }

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(20f);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add"))
        {
            prop.InsertArrayElementAtIndex(prop.arraySize);
        }

        if (GUILayout.Button("Remove"))
        {
            if (prop.arraySize > 0)
                prop.DeleteArrayElementAtIndex(prop.arraySize - 1);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }
}

#endif
