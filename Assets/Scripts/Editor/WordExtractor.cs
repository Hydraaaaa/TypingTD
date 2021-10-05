using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class WordExtractor : EditorWindow
{
    //[SerializeField] WordList m_WordList;
    //[SerializeField] string m_TextFilePath = "wordlist.txt";

    //[MenuItem("Window/Word Extractor")]
    //static void Init()
    //{
        //WordExtractor _Window = EditorWindow.GetWindow<WordExtractor>();

        //_Window.Show();
    //}

    //void OnGUI()
    //{
        //SerializedObject _SO = new SerializedObject(this);

        //EditorGUILayout.PropertyField(_SO.FindProperty("m_WordList"));
        //EditorGUILayout.PropertyField(_SO.FindProperty("m_TextFilePath"));

        //_SO.ApplyModifiedProperties();

        //if (GUILayout.Button("Extract"))
        //{
            //string[] _Words = File.ReadAllLines(Application.dataPath + "/" + m_TextFilePath);

            //m_WordList.m_WordList = new List<WordList.ListContainer>();

            //for (int i = 0; i < _Words.Length; i++)
            //{
                //int _Length = _Words[i].Length;

                //while (_Length > m_WordList.m_WordList.Count - 1)
                //{
                    //m_WordList.m_WordList.Add(new WordList.ListContainer());
                //}

                //m_WordList.m_WordList[_Length].List.Add(_Words[i]);
            //}
        //}
    //}
}
