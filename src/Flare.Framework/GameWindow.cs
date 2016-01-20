#region License
/* Flare - A Framework by Developers, for Developers.
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

        [DefaultValue(false)]
        public abstract bool AllowUserResizing
        {
            get;
            set;
        }

        public abstract Rectangle ClientBounds
        {
            get;
        }

        public abstract IntPtr Handle
        {
            get;
        }

        public abstract string ScreenDeviceName
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

        /// <summary>
        /// Determines whether the border of the window is visible.
        /// </summary>
        /// <exception cref="System.NotImplementedException">
        /// Thrown when trying to use this property on an unsupported platform.
        /// </exception>
        public virtual bool IsBorderlessEXT
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Private Variables

        private string _title;

        #endregion

        #region Protected Constructors

        protected GameWindow()
        {
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> ClientSizeChanged;
        public event EventHandler<EventArgs> OrientationChanged;
        public event EventHandler<EventArgs> ScreenDeviceNameChanged;

        #endregion

        #region Public Methods

        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

        public abstract void EndScreenDeviceChange(
            string screenDeviceName,
            int clientWidth,
            int clientHeight
        );

        public void EndScreenDeviceChange(string screenDeviceName)
        {
            EndScreenDeviceChange(
                screenDeviceName,
                ClientBounds.Width,
                ClientBounds.Height
            );
        }

        #endregion

        #region Protected Methods

        protected void OnActivated()
        {
        }

        protected void OnClientSizeChanged()
        {
            if (ClientSizeChanged != null)
            {
                ClientSizeChanged(this, EventArgs.Empty);
            }
        }

        protected void OnDeactivated()
        {
        }

        protected void OnOrientationChanged()
        {
            if (OrientationChanged != null)
            {
                OrientationChanged(this, EventArgs.Empty);
            }
        }

        protected void OnPaint()
        {
        }

        protected void OnScreenDeviceNameChanged()
        {
            if (ScreenDeviceNameChanged != null)
            {
                ScreenDeviceNameChanged(this, EventArgs.Empty);
            }
        }

        protected abstract void SetTitle(string title);

        #endregion
    }
}