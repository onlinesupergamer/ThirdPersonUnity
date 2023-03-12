using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    Material material;
    public Texture texture;


    [MenuItem("Window/Test")]
    public static void ShowWindow() 
    {
        GetWindow<TestWindow>("Work Test Window");
    }

    private void OnGUI()
    {
        GUILayout.Label("Retarget Selected Objects' Texture");

        if (GUILayout.Button("Change Texture")) 
        {
            foreach (GameObject obj in Selection.gameObjects) 
            {
                material = obj.GetComponent<Renderer>().material;
                if (material != null) 
                {
                    material.mainTexture = texture;
                }
            }
        }
            

    }


}
