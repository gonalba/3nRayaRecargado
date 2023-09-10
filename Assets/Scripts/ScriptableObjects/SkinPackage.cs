using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skin
{
    [CreateAssetMenu(fileName = "skinpackage", menuName = "3nRaya/Skin Pack", order = 1)]
    public class SkinPackage : ScriptableObject
    {
        public Sprite empty = null;
        public Sprite x = null;
        public Sprite o = null;
    }
}