/**
 * PlayingLevelGameState.cs - Playing Level Game State
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Logic;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.State
{
    /// <summary>
    /// Represents the in-game level game state.
    /// </summary>
    internal class PlayingLevelGameState : GameState
    {
        /// <summary>
        /// The rectangle that defines the bounds of the scene in the viewport.
        /// </summary>
        private static Rectangle SceneRect =
            new Rectangle(0, 0, 530, 420);
    
    
        /// <inheritdoc />
        public override InputFocalMode FocalMode
        {
            get { return InputFocalMode.Always; }
        }
        
        /// <inheritdoc />
        public override string Name
        {
            get { return "PlayingLevel"; }
        }
        
        
        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Game { get; set; }

        /// <summary>
        /// The level game scene.
        /// </summary>
        private Scene LevelScene { get; set; }
        
        /// <summary>
        /// The user interface shell.
        /// </summary>
        private UxShell Shell { get; set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingLevelGameState"/> class.
        /// </summary>
        /// <param name="game">
        /// The running Junkbot game instance.
        /// </param>
        /// <param name="buildingIndex">
        /// The index of the building the level belongs to.
        /// </param>
        /// <param name="levelIndex">
        /// The index of the level within the building.
        /// </param>
        public PlayingLevelGameState(
            JunkbotGame game,
            byte        buildingIndex,
            byte        levelIndex
        )
        {
            Game       = game;
            LevelScene = Scene.FromLevel(
                             game.Levels.GetLevelSource(buildingIndex, levelIndex),
                             game.Animations
                         );
                         
            LevelScene.GameplayEnded += LevelScene_GameplayEnded;
        }


        /// <inheritdoc />
        public override void Dispose()
        {
            // TODO: Implement this
        }
        
        /// <inheritdoc />
        public override void RenderFrame(
            IGraphicsController graphics
        )
        {
            graphics.ClearViewport(Color.CornflowerBlue);

            LevelScene.RenderFrame(graphics);
            
            // TODO: Shell renderframe
        }
        
        /// <inheritdoc />
        public override void Update(
            TimeSpan    deltaTime,
            InputEvents inputs
        )
        {
            // TODO: Shell handle inputs

            PointF mousePos =
                Game.EngineHost.Renderer.PointToViewport(
                    inputs.MousePosition
                );
                
            if (Collision.PointInRect(mousePos, SceneRect))
            {
                LevelScene.Update(
                    Game,
                    deltaTime,
                    inputs
                );
            }
            else
            {
                LevelScene.Update(
                    Game,
                    deltaTime
                );
            }
        }
        
        
        /// <summary>
        /// (Event) Handles when gameplay has ended in the level.
        /// </summary>
        private void LevelScene_GameplayEnded(
            object    sender,
            EventArgs e
        )
        {
            Dispose();

            Game.CurrentGameState = new SelectLevelGameState(Game);
        }
    }
}
