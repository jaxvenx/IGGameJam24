using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SceneController>();

                if (_instance == null)
                {
                    GameObject singleton = new(typeof(SceneController).ToString());
                    _instance = singleton.AddComponent<SceneController>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public async void LoadScene(int sceneIndexNew)
    {
        await SceneManager.LoadSceneAsync(sceneIndexNew);
        LightProbes.TetrahedralizeAsync();
    }
}
