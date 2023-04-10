using Fusion;

namespace VitaliyNULL.Core
{
    public interface IDamageable
    {
        void TakeDamage(int damage,PlayerRef playerRef);
    }
}