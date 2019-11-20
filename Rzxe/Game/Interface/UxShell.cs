using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Logic;
using Oddmatics.Rzxe.Util.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Rzxe.Game.Interface
{
    public class UxShell
    {
        protected SortedList2<UxComponent> Components { get; set; }


        public UxShell()
        {
            Components = new SortedList2<UxComponent>(new ZIndexComparer());
        }


        public bool HandleInputs(InputEvents inputs)
        {
            UxComponent component = MouseHitTest(inputs.MousePosition);

            if (component != null)
            {
                //
                // TODO: Do input handling here
                //

                return true;
            }

            return false;
        }


        private UxComponent MouseHitTest(PointF mousePos)
        {
            IEnumerable<UxComponent> components = Components.Reverse();

            foreach (UxComponent component in components)
            {
                if (Collision.PointInRect(mousePos, component.Bounds))
                {
                    return component;
                }
            }

            return null;
        }
    }
}
