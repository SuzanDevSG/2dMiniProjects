using UnityEditor;
using UnityEngine;
public class MissingRefScanner : EditorWindow
{
    [MenuItem("Tools/Scan for Missing References")]
    static void Scan()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            var components = obj.GetComponents<Component>();
            foreach (var comp in components)
            {
                if (comp == null)
                    Debug.LogWarning("Missing Component in: " + obj.name, obj);
            }
        }
    }
}