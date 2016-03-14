using UnityEngine;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

namespace Vexe.Runtime.Types
{
	/// <summary>
	/// A convenient script to load a selected scene
	/// Useful when hooked up to a delegate for remote scene loading
	/// when a certain event is fired like a UI button click etc
	/// </summary>
	
	public class SceneTransition : BaseBehaviour
	{
		[SelectScene, Comment("Note: scene will only load if it was included in the build settings and the player is running")]
		public string scene;
		
		[Show]
		public void LoadScene()
		{
#if UNITY_5_3
            SceneManager.LoadScene(scene);
#else
            Application.LoadLevel(scene);
#endif
        }
	}
}