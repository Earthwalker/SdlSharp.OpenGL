//-----------------------------------------------------------------------
// <copyright file="GLContext.cs" company="Leamware">
//     Copyright (c) Leamware. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SdlSharp.OpenGL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using SDL2;
    using SharpGL;
    using SharpGL.Version;
    using StarMathLib;
    using static Utility;

    /// <summary>
    /// Viewport style.
    /// </summary>
    public enum Viewport
    {
        Full,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    /// <summary>
    /// Renderer
    /// </summary>
    public class OpenGLRenderer : IRenderer
    {
        private readonly Window window;
        private bool disposedValue;

        /// <summary>
        /// Sdl handle.
        /// </summary>
        private IntPtr handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLRenderer" /> class.
        /// </summary>
        /// <param name="window">The window where rendering is displayed.</param>
        /// <param name="openGLVersion">The OpenGL version.</param>
        /// <param name="bitDepth">The bit depth.</param>
        public OpenGLRenderer(Window window, OpenGLVersion openGLVersion, int bitDepth)
        {
            this.window = window;

            handle = SDL.SDL_GL_CreateContext(window.Handle);

            GL = new OpenGL();

            GL.Create(openGLVersion, RenderContextType.NativeWindow, window.Size.X(), window.Size.Y(), bitDepth, handle);
            //gl.CreateFromExternalContext(openGLVersion, window.Size.X(), window.Size.Y(), bitDepth, window.handle,)
        }

        /// <summary>
        /// OpenGL handle.
        /// </summary>
        /// <value>
        /// The OpenGL handle.
        /// </value>
        public OpenGL GL { get; set; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            SDL.SDL_GL_SwapWindow(window.Handle);
        }

        

        /// <summary>
        /// Sets the viewport.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="FOV">The fov.</param>
        /// <param name="near">The near.</param>
        /// <param name="far">The far.</param>
        public void SetViewport(Viewport viewport, int FOV, double near, double far)
        {
            var viewportSize = window.Size;

            // set the viewport
            switch (viewport)
            {
                case Viewport.Full:
                    GL.Viewport(0, 0, window.Size.X(), window.Size.Y());
                    break;
                case Viewport.Top:
                    GL.Viewport(0, 0, window.Size.X(), window.Size.Y() / 2);
                    viewportSize[1] /= 2;
                    break;
                case Viewport.Bottom:
                    GL.Viewport(0, window.Size.Y() / 2, window.Size.X(), window.Size.Y());
                    viewportSize[1] /= 2;
                    break;
                case Viewport.TopLeft:
                    GL.Viewport(0, 0, window.Size.X() / 2, window.Size.Y() / 2);
                    viewportSize[0] /= 2;
                    viewportSize[1] /= 2;
                    break;
                case Viewport.TopRight:
                    GL.Viewport(window.Size.X() / 2, 0, window.Size.X(), window.Size.Y() / 2);
                    viewportSize[0] /= 2;
                    viewportSize[1] /= 2;
                    break;
                case Viewport.BottomLeft:
                    GL.Viewport(0, window.Size.Y() / 2, window.Size.X() / 2, window.Size.Y());
                    viewportSize[0] /= 2;
                    viewportSize[1] /= 2;
                    break;
                case Viewport.BottomRight:
                    GL.Viewport(window.Size.X() / 2, window.Size.Y() / 2, window.Size.X(), window.Size.Y());
                    viewportSize[0] /= 2;
                    viewportSize[1] /= 2;
                    break;
            }

            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.LoadIdentity();

            // set perspective
            GL.Perspective(FOV / 2, viewportSize.X() / viewportSize.Y(), near, far);
        }

        /// <summary>
        /// Draws a cube of the specified size and color.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>topColor
        public void DrawCube(double[] size, byte[] color)
        {
            DrawCube(size, new List<byte[]> { color, color, color, color, color, color });
        }

        /// <summary>
        /// Draws a cube of the specified size and colors.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="colors">The colors.</param>
        public void DrawCube(double[] size, IList<byte[]> colors)
        {
            // begin drawing a quad
            GL.Begin(OpenGL.GL_QUADS);

            // top side
            if (size.X() > 0 && size.Z() > 0)
            {
                GL.Color(colors[0].Red(), colors[0].Green(), colors[0].Blue(), colors[0].Alpha());
                GL.Vertex(size.X(), size.Y(), -size.Z());
                GL.Vertex(-size.X(), size.Y(), -size.Z());
                GL.Vertex(-size.X(), size.Y(), size.Z());
                GL.Vertex(size.X(), size.Y(), size.Z());
            }

            // bottom side
            if (size.X() > 0 && size.Y() > 0 && size.Z() > 0)
            {
                GL.Color(colors[1].Red(), colors[1].Green(), colors[1].Blue(), colors[1].Alpha());
                GL.Vertex(size.X(), -size.Y(), size.Z());
                GL.Vertex(-size.X(), -size.Y(), size.Z());
                GL.Vertex(-size.X(), -size.Y(), -size.Z());
                GL.Vertex(size.X(), -size.Y(), -size.Z());
            }

            // front side
            if (size.X() > 0 && size.Y() > 0)
            {
                GL.Color(colors[2].Red(), colors[2].Green(), colors[2].Blue(), colors[2].Alpha());
                GL.Vertex(size.X(), size.Y(), size.Z());
                GL.Vertex(-size.X(), size.Y(), size.Z());
                GL.Vertex(-size.X(), -size.Y(), size.Z());
                GL.Vertex(size.X(), -size.Y(), size.Z());
            }

            // back side
            if (size.X() > 0 && size.Y() > 0 && size.Z() > 0)
            {
                GL.Color(colors[3].Red(), colors[3].Green(), colors[3].Blue(), colors[3].Alpha());
                GL.Vertex(-size.X(), size.Y(), -size.Z());
                GL.Vertex(size.X(), size.Y(), -size.Z());
                GL.Vertex(size.X(), -size.Y(), -size.Z());
                GL.Vertex(-size.X(), -size.Y(), -size.Z());
            }

            // left side
            if (size.Y() > 0 && size.Z() > 0)
            {
                GL.Color(colors[4].Red(), colors[4].Green(), colors[4].Blue(), colors[4].Alpha());
                GL.Vertex(-size.X(), size.Y(), size.Z());
                GL.Vertex(-size.X(), size.Y(), -size.Z());
                GL.Vertex(-size.X(), -size.Y(), -size.Z());
                GL.Vertex(-size.X(), -size.Y(), size.Z());
            }

            // right side
            if (size.X() > 0 && size.Y() > 0 && size.Z() > 0)
            {
                GL.Color(colors[5].Red(), colors[5].Green(), colors[5].Blue(), colors[5].Alpha());
                GL.Vertex(size.X(), size.Y(), -size.Z());
                GL.Vertex(size.X(), size.Y(), size.Z());
                GL.Vertex(size.X(), -size.Y(), size.Z());
                GL.Vertex(size.X(), -size.Y(), -size.Z());
            }
            GL.End();
        }

        /// <summary>
        /// Draws a cube of the specified size and texture.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="texture">The texture.</param>
        public void DrawCube(double[] size, GLTexture texture)
        {
            // ensure we have a texture
            if (texture == null)
                return;

            GL.Enable(OpenGL.GL_TEXTURE_2D);

            // bind the texture
            texture.Bind();

            // begin drawing a quad
            GL.Begin(OpenGL.GL_QUADS);

            // top side
            if (size.X() > 0 && size.Z() > 0)
            {
                GL.TexCoord(0.0, 0.0); GL.Vertex(size.X(), size.Y(), -size.Z());
                GL.TexCoord(1.0, 0.0); GL.Vertex(-size.X(), size.Y(), -size.Z());
                GL.TexCoord(1.0, 1.0); GL.Vertex(-size.X(), size.Y(), size.Z());
                GL.TexCoord(0.0, 1.0); GL.Vertex(size.X(), size.Y(), size.Z());
            }

            // bottom side
            if (size.X() > 0 && size.Y() > 0 && size.Z() > 0)
            {
                GL.TexCoord(0.0, 0.0); GL.Vertex(size.X(), -size.Y(), size.Z());
                GL.TexCoord(1.0, 0.0); GL.Vertex(-size.X(), -size.Y(), size.Z());
                GL.TexCoord(1.0, 1.0); GL.Vertex(-size.X(), -size.Y(), -size.Z());
                GL.TexCoord(0.0, 1.0); GL.Vertex(size.X(), -size.Y(), -size.Z());
            }

            // front side
            if (size.X() > 0 && size.Y() > 0)
            {
                GL.TexCoord(0.0, 0.0); GL.Vertex(size.X(), size.Y(), size.Z());
                GL.TexCoord(1.0, 0.0); GL.Vertex(-size.X(), size.Y(), size.Z());
                GL.TexCoord(1.0, 1.0); GL.Vertex(-size.X(), -size.Y(), size.Z());
                GL.TexCoord(0.0, 1.0); GL.Vertex(size.X(), -size.Y(), size.Z());
            }

            // back side
            if (size.X() > 0 && size.Y() > 0 && size.Z() > 0)
            {
                GL.TexCoord(0.0, 0.0); GL.Vertex(-size.X(), size.Y(), -size.Z());
                GL.TexCoord(1.0, 0.0); GL.Vertex(size.X(), size.Y(), -size.Z());
                GL.TexCoord(1.0, 1.0); GL.Vertex(size.X(), -size.Y(), -size.Z());
                GL.TexCoord(0.0, 1.0); GL.Vertex(-size.X(), -size.Y(), -size.Z());
            }

            // left side
            if (size.Y() > 0 && size.Z() > 0)
            {
                GL.TexCoord(0.0, 0.0); GL.Vertex(-size.X(), size.Y(), size.Z());
                GL.TexCoord(1.0, 0.0); GL.Vertex(-size.X(), size.Y(), -size.Z());
                GL.TexCoord(1.0, 1.0); GL.Vertex(-size.X(), -size.Y(), -size.Z());
                GL.TexCoord(0.0, 1.0); GL.Vertex(-size.X(), -size.Y(), size.Z());
            }

            // right side
            if (size.X() > 0 && size.Y() > 0 && size.Z() > 0)
            {
                GL.TexCoord(0.0, 0.0); GL.Vertex(size.X(), size.Y(), -size.Z());
                GL.TexCoord(1.0, 0.0); GL.Vertex(size.X(), size.Y(), size.Z());
                GL.TexCoord(1.0, 1.0); GL.Vertex(size.X(), -size.Y(), size.Z());
                GL.TexCoord(0.0, 1.0); GL.Vertex(size.X(), -size.Y(), -size.Z());
            }

            GL.End();

            GL.Disable(OpenGL.GL_TEXTURE_2D);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    SDL.SDL_GL_DeleteContext(handle);

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
