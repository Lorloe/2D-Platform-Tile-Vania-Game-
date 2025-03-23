using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake() 
    {
        int numberOfScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numberOfScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }    
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
