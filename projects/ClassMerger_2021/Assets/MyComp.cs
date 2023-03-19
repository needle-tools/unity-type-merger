using System.Collections;
using System.Collections.Generic;
using Needle;
using UnityEngine;

[MergeClass("MyPartial")]
public partial class MyComp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


internal class MyPartial
{
    public List<string> PartialList;
}