using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections;
 
 [InitializeOnLoad]
public static class SimpleEditorUtils
{
	// click command-0 to go to the prelaunch scene and then play
		
	[MenuItem("Edit/Inicio #z")]
	public static void PlayFromPrelaunchScene()
	{
		if (EditorApplication.isPlaying)
		{
			EditorApplication.isPlaying = false;
			EditorSceneManager.OpenScene("Assets/Scenes/"+LastScene);
			return;
		}
		else
		{
			LastScene =  EditorSceneManager.GetActiveScene().name;
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene("Assets/Scenes/Inicio.unity");
			EditorApplication.isPlaying = true;
		}
	}

	private static string LastScene;
}