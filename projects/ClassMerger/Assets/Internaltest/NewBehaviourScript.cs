using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;

namespace Internaltest
{
    public class NewBehaviourScript
    {
        [RegisterPlugins]
        private static IEnumerable<PluginDesc> Reg(BuildTarget target)
        {
            yield return new PluginDesc();
        }
    }
}
