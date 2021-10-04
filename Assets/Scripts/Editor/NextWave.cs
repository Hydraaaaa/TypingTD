using UnityEngine;
using UnityEditor;

public class NextWave
{
    [MenuItem("Typing TD/Next Wave")]
    static void StartNextWave()
    {
        if (Application.isPlaying)
        {
            WaveManager.StartNextWave();
        }
    }
}
