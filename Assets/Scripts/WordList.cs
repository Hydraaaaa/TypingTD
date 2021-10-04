using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WordList : ScriptableObject
{
    public string GetRandomWord(int a_Length)
    {
        int _RandomIndex = Random.Range(0, m_WordList[a_Length].List.Count);

        return m_WordList[a_Length].List[_RandomIndex];
    }

    [SerializeField] List<ListContainer> m_WordList;

    // Lists of lists aren't serializable, so we wrap the 2nd layer in a class
    [System.Serializable]
    public class ListContainer
    {
        public List<string> List = new List<string>();
    }
}
