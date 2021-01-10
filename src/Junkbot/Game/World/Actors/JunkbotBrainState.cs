/**
 * JunkbotBrainState.cs - Junkbot Actor FSM Brain State Enumeration
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Specifies constants defining the possible brain states for the Junkbot actor.
    /// </summary>
    internal enum JunkbotBrainState
    {
        /// <summary>
        /// Junkbot is currently being lifted in an air lift/vent.
        /// </summary>
        AirLift,
        
        /// <summary>
        /// Junkbot is dying due to coming into contact with a hazard.
        /// </summary>
        DyingGeneric,
        
        /// <summary>
        /// Junkbot is dying from short-circuiting due to coming into coming with a
        /// water hazard.
        /// </summary>
        DyingShortCircuit,
        
        /// <summary>
        /// Junkbot is dead.
        /// </summary>
        Dead,
        
        /// <summary>
        /// Junkbot is currently eating a bin.
        /// </summary>
        EatingBin,
        
        /// <summary>
        /// Junkbot is currently falling.
        /// </summary>
        Falling,
        
        /// <summary>
        /// Junkbot is losing the shield powerup.
        /// </summary>
        ShieldPowerDown,
        
        /// <summary>
        /// Junkbot is gaining the shield powerup.
        /// </summary>
        ShieldPowerUp,
        
        /// <summary>
        /// Junkbot is currently teleporting.
        /// </summary>
        Teleporting,
        
        /// <summary>
        /// Junkbot is currently walking.
        /// </summary>
        Walking
    }
}