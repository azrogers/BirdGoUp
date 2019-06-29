using UnityEngine;

/// <summary>
/// Base class for singletons.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	/// <summary>
	/// Returns an instance of this singleton.
	/// </summary>
	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));

				if(instance == null)
				{
					//Try finding non-active scene object
					var objects = Resources.FindObjectsOfTypeAll<T>();
					if(objects.Length > 0)
					{
						instance = objects[0];
					}
				}

				if(instance == null)
				{
					Debug.LogError("An instance of " + typeof(T) +
					   " is needed in the scene, but there is none.");

				}
			}

			return instance;
		}
	}
}