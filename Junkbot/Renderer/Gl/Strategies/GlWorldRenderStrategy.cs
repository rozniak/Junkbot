using Junkbot.Game;
using Junkbot.Game.World.Actors;
using Junkbot.Game.World.Actors.Animation;
using Junkbot.Helpers;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Drawing;

namespace Junkbot.Renderer.Gl.Strategies
{
    /// <summary>
    /// Renders the game world using the OpenGL API.
    /// </summary>
    internal sealed class GlWorldRenderStrategy : GlRenderStrategy
    {
        /// <summary>
        /// Gets or sets the top left origin for rendering the scene.
        /// </summary>
        public Point Origin;


        /// <summary>
        /// The actor sprite atlas.
        /// </summary>
        private GlSpriteAtlas ActorAtlas;

        /// <summary>
        /// The Junkbot game engine.
        /// </summary>
        private JunkbotGame Game;

        /// <summary>
        /// The size of the scene.
        /// </summary>
        private Size SceneSize;

        /// <summary>
        /// The ID of the OpenGL Shader Program to use.
        /// </summary>
        private uint GlProgramId;
        

        /// <summary>
        /// Releases all resources used by this <see cref="GlWorldRenderStrategy"/>.
        /// </summary>
        public override void Dispose()
        {
            ActorAtlas?.Dispose();
        }

        /// <summary>
        /// Initializes this <see cref="GlWorldRenderStrategy"/>.
        /// </summary>
        /// <param name="gameReference">
        /// A reference to the Junkbot game engine.
        /// </param>
        /// <returns>True if the initialization process was successful.</returns>
        public override bool Initialize(JunkbotGame gameReference)
        {
            ActorAtlas = GlSpriteAtlas.FromFileSet(
                Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas"
                );

            Game = gameReference;
            GlProgramId = Resources.GetShaderProgram("SimpleUVs");
            Origin = Point.Empty;
            SceneSize = Game.GameState.Scene.Size;

            return true;
        }

        /// <summary>
        /// Renders a portion of the next frame.
        /// </summary>
        public override void RenderFrame()
        {
            var sb = new GlSpriteBatch(
                ActorAtlas,
                GlProgramId
                );

            // Render immobile actors
            //
            foreach (BrickActor brick in Game.GameState.Scene.ImmobileBricks)
            {
                ActorAnimationFrame currentFrame = brick.Animation.GetCurrentFrame();
                Rectanglei blitRect = ActorAtlas.GetSpriteUV(currentFrame.SpriteName);
                Point pointLoc = brick.Location
                    .Product(Game.GameState.Scene.CellSize)
                    .Add(currentFrame.Offset).Add(Origin.Product(Game.GameState.Scene.CellSize));
                Vector2i drawLoc = new Vector2i(
                    pointLoc.X, pointLoc.Y
                    );

                sb.Draw(
                    currentFrame.SpriteName,
                    new Rectanglei(
                        drawLoc,
                        blitRect.Size
                        )
                    );
            }

            sb.Finish();
        }
    }
}
