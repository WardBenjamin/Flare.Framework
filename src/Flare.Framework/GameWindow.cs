#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare
{
    public abstract class GameWindow
    {
        #region Public Properties

        public abstract Rectangle ClientBounds
        {
            get;
        }

        public abstract IntPtr Handle
        {
            get;
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    SetTitle(value);
                    _title = value;
                }
            }
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public abstract bool IsBorderless
        {
            get;
            set;
        }

        public abstract bool IsFullscreen
        {
            get;
            set;
        }

        /// <summary>
        /// The file name of the Icon. If SDL_image is present, this can be a PNG. Otherwise, this will load only bitmap images.
        /// </summary>
        public abstract string Icon
        {
            get;
            set;
        }

        #endregion

        #region Private Variables

        private string _title;

        #endregion

        #region Protected Constructors

        protected GameWindow(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> ClientSizeChanged;

        #endregion

        #region Public Methods

        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

        public abstract void EndScreenDeviceChange(int clientWidth, int clientHeight);

        public void EndScreenDeviceChange()
        {
            EndScreenDeviceChange(ClientBounds.Width, ClientBounds.Height);
        }

        #endregion

        #region Protected Methods

        protected void OnClientSizeChanged()
        {
            if (ClientSizeChanged != null)
            {
                ClientSizeChanged(this, EventArgs.Empty);
            }
        }

        protected abstract void SetTitle(string title);

        #endregion
    }
}