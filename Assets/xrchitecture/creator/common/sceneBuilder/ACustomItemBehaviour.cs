using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal abstract class ACustomItemBehaviour : MonoBehaviour
    {
        internal delegate void CustomParameterChanged(ItemCustomArgs itemCustomPar);
        internal CustomParameterChanged OnCustomParameterChanged;

        public List<ItemCustomArgs> CustomArgsList;
        public abstract void Initialize(List<ItemCustomArgs> args);

        //TODO itemName is not used i Think
        public abstract void UpdateCustomArgs(string itemName, string key, string value);
    }
}