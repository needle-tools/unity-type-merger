using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var comp = gameObject.AddComponent<MyComponent>();
    } 

    // Update is called once per frame
    void Update()
    {
           
    }
}
