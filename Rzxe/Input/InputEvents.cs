using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Oddmatics.Rzxe.Input
{
    public sealed class InputEvents
    {
        public char ConsoleInput { get; private set; }
        
        public IList<string> DownedInputs { get; private set; }
        
        public bool IsReadOnly { get; private set; }
        
        public PointF MousePosition { get; private set; }
        
        public IList<string> NewPresses { get; private set; }
        
        public IList<string> NewReleases { get; private set; }

        
        private List<string> ActiveDownedInputs { get; set; }
        
        private IList<string> LastDownedInputs { get; set; }

        
        public InputEvents()
        {
            ActiveDownedInputs = new List<string>();
            ConsoleInput       = char.MinValue;
            DownedInputs       = new List<string>().AsReadOnly();
            IsReadOnly         = false;
            LastDownedInputs   = null;
            MousePosition      = PointF.Empty;
            NewPresses         = new List<string>().AsReadOnly();
            NewReleases        = new List<string>().AsReadOnly();
        }
        
        public InputEvents(
            IList<string> lastDownedInputs,
            PointF lastMousePosition
        ) : this()
        {
            ActiveDownedInputs = new List<string>(lastDownedInputs);
            LastDownedInputs   = lastDownedInputs;
            MousePosition      = lastMousePosition;
        }

        
        public void FinalizeForReporting()
        {
            AssertNotReadOnly();

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
        
        public void ReportConsoleInput(char input)
        {
            AssertNotReadOnly();

            ConsoleInput = input;
        }
        
        public void ReportMouseMovement(float x, float y)
        {
            AssertNotReadOnly();

            MousePosition = new PointF(x, y);
        }
        
        public void ReportPress(string input)
        {
            AssertNotReadOnly();

            if (!ActiveDownedInputs.Contains(input))
            {
                ActiveDownedInputs.Add(input);
            }
        }
    
        public void ReportRelease(string input)
        {
            AssertNotReadOnly();

            if (ActiveDownedInputs.Contains(input))
            {
                ActiveDownedInputs.Remove(input);
            }
        }

        
        private void AssertNotReadOnly()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(
                    "This input update state is currently read-only and cannot be modified."
                    );
            }
        }
    }
}
