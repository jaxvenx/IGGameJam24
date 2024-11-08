using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Unity.Jobs;
using UnityEngine.SceneManagement;

public class ShaderManager : MonoBehaviour
{
    public bool EnableShaderCollectionMode;
	[SerializeField] private GraphicsStateCollection _graphicsStateCollection;
    [SerializeField] private GameObject[] _preloadObjects;

    public JobHandle inputJobHandle;

    struct PostWarmUpJob : IJob
    {
        public readonly void Execute()
        {
            Debug.Log("WarmUp is complete");
            SceneController.Instance.LoadScene(1);
        }
    }

    private void OnEnable()
    {

        if (EnableShaderCollectionMode)
        {
            DontDestroyOnLoad(gameObject);
            
        	if(_graphicsStateCollection == null) _graphicsStateCollection = new();
        }
        else
        {
            JobHandle handle = _graphicsStateCollection.WarmUp(inputJobHandle);

        var job = new PostWarmUpJob();
        job.Schedule(handle);
        }
    }
	
    private void Awake()
    {
        foreach (GameObject preloadObject in _preloadObjects) { 
            GameObject shaderPreloadObject = Instantiate(preloadObject, new Vector3(0f, 5.36800003f, 22.7019997f), Quaternion.identity);
            Destroy(shaderPreloadObject, 5f);
        }
    }
    public void SaveTrace()
	{
		if(EnableShaderCollectionMode) {
			_graphicsStateCollection.EndTrace();
            _graphicsStateCollection.SaveToFile(Application.persistentDataPath + "/_graphicsStateCollection.graphicsstate");
        }
	}
}
