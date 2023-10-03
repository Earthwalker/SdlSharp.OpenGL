//-----------------------------------------------------------------------
// <copyright file="GLTexture.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SdlSharp.OpenGL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Interfaces;
    using SDL2;
    using SharpGL;
    using static Utility;

    /// <summary>
    /// GLTexture
    /// </summary>
    public class GLTexture : ITexture
    {
        /// <summary>
        /// The renderer.
        /// </summary>
        private readonly OpenGLRenderer renderer;

        /// <summary>
        /// The texture identifier.
        /// </summary>
        private readonly uint id;

        /// <summary>
        /// Whether we have been disposed.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="GLTexture"/> class.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="surface">The surface.</param>
        public GLTexture(OpenGLRenderer renderer, Surface surface)
        {
            this.renderer = renderer;
            var gl = renderer.GL;

            var textures = new uint[1];
            gl.GenTextures(1, textures);
            id = textures[0];

            Bind();

            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);

            var format = (SDL.SDL_PixelFormat)Marshal.PtrToStructure(surface.SurfaceStruct.format, typeof(SDL.SDL_PixelFormat));

            // map the surface to the texture in video memory, according to whether the texture has alpha
            if (format.Amask == 0)
                gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGB, surface.Size.X(), surface.Size.Y(), 0, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, surface.SurfaceStruct.pixels);
            else
                gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA, surface.Size.X(), surface.Size.Y(), 0, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, surface.SurfaceStruct.pixels);

            Size = surface.Size;
        }

        public int[] Size { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GLTexture" /> class.
        /// </summary>
        /// <param name="textureManager">The texture manager.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// The new <see cref="ITexture" />.
        /// </returns>
        public static GLTexture Create(TextureManager textureManager, OpenGLRenderer renderer, string fileName)
        {
            // check if the texture has already been loaded
            var texture = textureManager.GetTextureFromName(fileName);

            if (texture == null)
            {
                // load the image to a surface and create the texture
                using (var surface = new Surface(fileName))
                    return (GLTexture)textureManager.AddTexture(new GLTexture(renderer, surface), fileName);
            }

            return (GLTexture)texture;
        }

        /// <summary>
        /// Binds this instance.
        /// </summary>
        public void Bind()
        {
            renderer.GL.BindTexture(OpenGL.GL_TEXTURE_2D, id);
        }

        /// <summary>
        /// Draws this instance at the specified position, rotation, and scale, relative to the origin.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="origin">The relative point to draw and rotate around. Null for the same as position.</param>
        public void Draw(double[] position, double[] rotation, double[] scale, int[] origin)
        {
            var GL = renderer.GL;

            GL.Enable(OpenGL.GL_TEXTURE_2D);

            GL.PushMatrix();

            // bind the texture
            Bind();

            var drawPosition = Vector(position.X() - (scale.X() * origin.X()),
                                      position.Y() - (scale.Y() * origin.Y()));

            GL.Translate(drawPosition.X(), drawPosition.Y(), 0);

            GL.Rotate(1, rotation.X(), rotation.Y(), rotation.Z());

            GL.Begin(OpenGL.GL_QUADS);
            GL.TexCoord(1.0, 1.0); GL.Vertex(Size.X() * scale.X(), Size.Y() * scale.Y());   // top right
            GL.TexCoord(0.0, 1.0); GL.Vertex(0, Size.Y() * scale.Y());                      // top left
            GL.TexCoord(0.0, 0.0); GL.Vertex(0, 0);                                         // bottom left
            GL.TexCoord(1.0, 0.0); GL.Vertex(Size.X() * scale.X(), 0);                      // bottom right
            GL.End();

            GL.PopMatrix();

            GL.Disable(OpenGL.GL_TEXTURE_2D);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    renderer.GL.DeleteTextures(1, new uint[] { id });

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
