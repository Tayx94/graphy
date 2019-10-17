using UnityEngine;
using System.Collections;

public class RandomConsoleLogTest : MonoBehaviour {

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int rand = Random.Range(0, 3);

            switch (rand)
            {
                case 0:
                    Debug.Log("This is a test Log msg");
                    break;

                case 1:
                    Debug.LogWarning("This is a test Log warning");
                    break;

                case 2:
                    Debug.LogError("This is a test Log error");
                    break;
            }
        }
    }
}
