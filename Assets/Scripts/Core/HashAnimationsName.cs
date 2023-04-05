using UnityEngine;

namespace VitaliyNULL.Core
{
    public class HashAnimationsName
    {
        public int Stand => Animator.StringToHash("stand");
        public int Run => Animator.StringToHash("run");
        public int Dead => Animator.StringToHash("dead");
        public int Hit => Animator.StringToHash("hit");
    }
}