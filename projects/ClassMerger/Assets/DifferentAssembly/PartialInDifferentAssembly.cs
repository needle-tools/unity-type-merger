



using UnityEngine;

namespace DifferentAssembly
{
    public class PartialInDifferentAssembly : MonoBehaviour
    {
        public int MyInt = 42;
        // public List<Component> List;

        [ContextMenu("Test In Other Assembly")]
        private void ContextMenuTest()
        {
            
        }
        
    } 
}
