using Junkbot.Game.UI.Controls;
using System.Collections.Generic;

namespace Junkbot.Game.UI
{
    /// <summary>
    /// Represents a user interface system for the Junkbot game engine.
    /// </summary>
    internal sealed class JunkbotInterface
    {
        /// <summary>
        /// The user interface controls inside this interface.
        /// </summary>
        private Dictionary<string, Control> Controls;


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotInterface"/> class.
        /// </summary>
        public JunkbotInterface()
        {
            Controls = new Dictionary<string, Control>();
        }


        /// <summary>
        /// Adds a control to this interface.
        /// </summary>
        public void AddControl()
        {
            // TODO: Code this
        }
    }
}
