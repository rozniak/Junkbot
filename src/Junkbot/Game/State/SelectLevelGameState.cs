/**
 * SelectLevelGameState.cs - Select Level Game State
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.Interface;
using Junkbot.Game.World.Level;
using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.State
{
    /// <summary>
    /// Represents the select level game state.
    /// </summary>
    public class SelectLevelGameState : GameState
    {
        /// <inheritdoc />
        public override InputFocalMode FocalMode
        {
            get { return InputFocalMode.Always; }
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return "SelectLevel"; }
        }


        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Game { get; set; }
        
        /// <summary>
        /// The user interface shell.
        /// </summary>
        private UxShell Shell { get; set; }


        #region Shell Components
        
        /// <summary>
        /// The level list.
        /// </summary>
        private JunkbotUxLevelList LevelList { get; set; }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="SelectLevelGameState"/>
        /// class.
        /// </summary>
        /// <param name="game">
        /// The running Junkbot game instance.
        /// </param>
        public SelectLevelGameState(
            JunkbotGame game
        )
        {
            Game = game;
            
            InitializeShell();
        }
        
        
        /// <inheritdoc />
        public override void Dispose()
        {
            Shell.Dispose();
        }
        
        /// <inheritdoc />
        public override void RenderFrame(
            IGraphicsController graphics
        )
        {
            graphics.ClearViewport(Color.CornflowerBlue);

            Shell.RenderFrame(graphics);
        }
        
        /// <inheritdoc />
        public override void Update(
            TimeSpan    deltaTime,
            InputEvents inputs
        )
        {
            Shell.Update(deltaTime, inputs);
        }
        
        
        /// <summary>
        /// Initializes the shell for the select level screen.
        /// </summary>
        private void InitializeShell()
        {
            Shell = new UxShell();

            // LevelList
            //
            LevelList =
                new JunkbotUxLevelList(Game)
                {
                    Location = new Point(0, 0)
                };
                
            LevelList.LevelSelected += LevelList_LevelSelected;

            Shell.Components.Add(LevelList);
        }
        
        
        /// <summary>
        /// (Event) Handles a level being selected.
        /// </summary>
        private void LevelList_LevelSelected(
            object                 sender,
            LevelSelectedEventArgs e
        )
        {
            Game.CurrentGameState =
                new PlayingLevelGameState(
                    Game,
                    Game.Levels.GetLevel(e.BuildingIndex, e.LevelIndex)
                );

            Dispose();
        }
    }
}
