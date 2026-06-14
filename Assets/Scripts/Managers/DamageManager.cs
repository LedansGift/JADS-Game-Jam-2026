using UnityEngine;

namespace Managers
{
    public enum DamageType
    {
        Piercing = 0,   // Substat: Penetration (Rounded down)
        Impact = 1,     // Substat: Stun Time
        Explosive = 2,  // Substat: Splash Radius
        Fire = 3,       // Substat: Scorch (Undecided)
        Poison = 4,     // Substat: DoT
        Ice = 5,        // Substat: Slowdown
        Degraded = 6,   // Substat: No function
        None = 7        // Substat: Damage Reduction? No function planned
    }

    public static class DamageManager
    {
        
    }
}