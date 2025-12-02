
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneEditor 
{
    
    [MenuItem("Scenes/StartScene", false, 1)]
    static void OpenStartScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/StartScene.unity");
    }

    [MenuItem("Scenes/ClassroomScene", false, 2)]
    static void OpenClassRoomSceneScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ClassroomScene.unity");

    }
   
}
