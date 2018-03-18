using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.Input
{
    internal class InputEvents
    {
        /// <summary>
        /// Gets the latest Unicode console key input.
        /// </summary>
        public char ConsoleInput { get; private set; }

        /// <summary>
        /// Gets the current inputs that are pressed.
        /// </summary>
        public IList<string> DownedInputs { get; private set; }

        /// <summary>
        /// Gets the value that indicates whether this input update is read-only.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        /// <summary>
        /// Gets inputs that have been pressed since the last state update.
        /// </summary>
        public IList<string> NewPresses { get; private set; }

        /// <summary>
        /// Gets inputs that have been released since the last state update.
        /// </summary>
        public IList<string> NewReleases { get; private set; }


        /// <summary>
        /// The collection of currently pressed inputs.
        /// </summary>
        private List<string> ActiveDownedInputs { get; set; }

        /// <summary>
        /// The collection of previously pressed inputs.
        /// </summary>
        private IList<string> LastDownedInputs { get; set; }


        /// <summary>
        /// Initializes a new instance of the InputEvents class.
        /// </summary>
        public InputEvents()
        {
            ActiveDownedInputs = new List<string>();
            ConsoleInput = char.MinValue;
            DownedInputs = new List<string>().AsReadOnly();
            IsReadOnly = false;
            LastDownedInputs = null;
            MousePosition = Vector2.Zero;
            NewPresses = new List<string>().AsReadOnly();
            NewReleases = new List<string>().AsReadOnly();
        }

        /// <summary>
        /// Initializes a new instance of the InputEvents class.
        /// </summary>
        /// <param name="lastDownedInputs">The downed inputs of the last input update.</param>
        /// <param name="lastMousePosition">The mouse position of the last input update.</param>
        public InputEvents(IList<string> lastDownedInputs, Vector2 lastMousePosition) : this()
        {
            ActiveDownedInputs = new List<string>(lastDownedInputs);
            LastDownedInputs = lastDownedInputs;
            MousePosition = lastMousePosition;
        }


        /// <summary>
        /// Performs a comparison of this input update versus the last, and finalizes the state so no further reports can be made.
        /// </summary>
        public void FinalizeForReporting()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This input update state is current read-only and cannot be modified.");

            // Set up downed inputs
            //
            DownedInputs = new List<string>(ActiveDownedInputs).AsReadOnly();

            // Set up new presses
            //
            var newPresses = LastDownedInputs == null ?
                ActiveDownedInputs :
                ActiveDownedInputs.Except(LastDownedInputs);

            NewPresses = new List<string>(newPresses).AsReadOnly();

            // Set up new releases
            //
            var newReleases = LastDownedInputs == null ?
                new List<string>() :
                LastDownedInputs.Except(ActiveDownedInputs);

            NewReleases = new List<string>(newReleases).AsReadOnly();


            IsReadOnly = true;
        }

        /// <summary>
        /// Reports a Unicode console input.
        /// </summary>
        /// <param name="input">The filtered Unicode input.</param>
        public void ReportConsoleInput(char input)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This input update state is current read-only and cannot be modified.");

            ConsoleInput = input;
        }

        /// <summary>
        /// Reports a mouse movement.
        /// </summary>
        /// <param name="x">The new mouse x-coordinate.</param>
        /// <param name="y">The new mouse y-coordinate.</param>
        public void ReportMouseMovement(float x, float y)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This input update state is currently read-only and cannot be modified.");

            MousePosition = new Vector2(x, y);
        }

        /// <summary>
        /// Reports an input press.
        /// </summary>
        /// <param name="input">The fully-qualified input name.</param>
        public void ReportPress(string input)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This input update state is currently read-only and cannot be modified.");

            if (!ActiveDownedInputs.Contains(input))
                ActiveDownedInputs.Add(input);
        }

        /// <summary>
        /// Reports an input release.
        /// </summary>
        /// <param name="input">The fully-qualified input name.</param>
        public void ReportRelease(string input)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("This input update state is current read-only and cannot be modified.");

            if (ActiveDownedInputs.Contains(input))
                ActiveDownedInputs.Remove(input);
        }
    }
}
