//-----------------------------------------------------------------------
// <copyright file="BackendWrapper.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Command.Commands;
    using Command.Factory;
    using DrawableObject;
    using Serializer;
    using Storage;

    /// <summary>
    /// A <see cref="BackendWrapper"/> wraps up a <see cref="Drawing"/>,
    /// list of <see cref="DrawableObject"/>s, etc in order to allow those objects
    /// to easily be used by the GUI
    /// </summary>
    public class BackendWrapper
    {
        /// <summary>
        /// A <see cref="Stack{T}"/> of <see cref="ICommand"/>
        /// objects used to easily undo changes
        /// </summary>
        private Stack<ICommand> undoStack;

        /// <summary>
        /// A list of <see cref="DrawableObject"/>s that are rendered
        /// in a <see cref="Drawing"/>
        /// </summary>
        private DrawableObjectList drawableObjectList;

        /// <summary>
        /// The <see cref="Drawing"/> that will be rendered to the GUI
        /// </summary>
        private Drawing drawing;

        /// <summary>
        /// A <see cref="CommandFactory"/> used to create the <see cref="ICommand"/>
        /// objects that are requested by the GUI
        /// </summary>
        private CommandFactory commandFactory;

        /// <summary>
        /// The background image to be used for the <see cref="Drawing"/>
        /// </summary>
        private Image drawingBackground;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackendWrapper"/> class
        /// </summary>
        /// <param name="backgroundImage">The background image used in the <see cref="BackendWrapper"/>'s <see cref="Drawing"/></param>
        /// <param name="size">The <see cref="Size"/> the <see cref="BackendWrapper"/>'s <see cref="Drawing"/> should be</param>
        public BackendWrapper(Image backgroundImage, Size size)
        {
            this.drawableObjectList = DrawableObjectList.Create();
            this.drawingBackground = backgroundImage;
            this.drawing = Drawing.Create(backgroundImage, this.drawableObjectList, size);
            this.undoStack = new Stack<ICommand>();
            this.commandFactory = CommandFactory.Create();
        }

        /// <summary>
        /// Gets a graphical version of the <see cref="Drawing"/> object
        /// contained in the <see cref="Backe"/>
        /// </summary>
        public Image RenderedDrawing
        { 
            get
            {
                return this.drawing.RenderedDrawing;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BackendWrapper"/> class
        /// </summary>
        /// <param name="backgroundImage">The background image used in the <see cref="BackendWrapper"/>'s <see cref="Drawing"/></param>
        /// <param name="size">The <see cref="Size"/> the <see cref="BackendWrapper"/>'s <see cref="Drawing"/> should be</param>
        /// <returns>A new instance of the <see cref="BackendWrapper"/> class</returns>
        public static BackendWrapper Create(Image backgroundImage, Size size)
        {
            return new BackendWrapper(backgroundImage, size);
        }

        /// <summary>
        /// Gets a list of the drawing files that are currently saved
        /// in the cloud
        /// </summary>
        /// <returns>A list of drawings saved in the cloud</returns>
        public static List<string> ListCloudFiles()
        {
            try
            {
                return CloudManager.ListFiles();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Attempts to save the current <see cref="Drawing"/> to the cloud
        /// Returns whether it succeeded
        /// </summary>
        /// <param name="fileName">The name the file should have in the cloud</param>
        /// <param name="wrapper">The <see cref="BackendWrapper"/> containing the drawing being saved</param>
        /// <returns>Whether the upload succeeded</returns>
        public static bool SaveToCloud(string fileName, BackendWrapper wrapper)
        {
            try
            {
                string jsonVersionOfList = DrawableObjectSerializer.Serialize(wrapper.drawableObjectList);
                File.WriteAllText($"{fileName}.json", jsonVersionOfList);
                ((Bitmap)wrapper.drawingBackground).Save($"{fileName}.png", ImageFormat.Png);
                CloudManager.Upload($"{fileName}.json", $"{fileName}.png", fileName);
                File.Delete($"{fileName}.json");
                File.Delete($"{fileName}.png");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to download a <see cref="Drawing"/> from the cloud
        /// and then set the current drawing to that one
        /// </summary>
        /// <param name="fileName">The name of the file that should be downloaded</param>
        /// <returns>Whether the download succeeded</returns>
        public static BackendWrapper DownloadFromCloud(string fileName)
        {
            try
            {
                BackendWrapper wrapper = null;
                CloudManager.Download($"{fileName}.json", $"{fileName}.png", fileName);
                string jsonVersionOfList = File.ReadAllText($"{fileName}.json");
                DrawableObjectList list = DrawableObjectSerializer.Deserialize(jsonVersionOfList);
                Image background;
                using (FileStream myStream = new FileStream($"{fileName}.png", FileMode.Open))
                {
                    background = Image.FromStream(myStream);
                }

                Size size = background.Size;
                File.Delete($"{fileName}.json");
                File.Delete($"{fileName}.png");
                wrapper = BackendWrapper.Create(background, background.Size);
                wrapper.drawing.Dispose();
                wrapper.drawing = Drawing.Create(background, list, size);
                wrapper.drawableObjectList = list;
                return wrapper;
            }
            catch (Exception)
            {
                if (File.Exists($"{fileName}.json"))
                {
                    File.Delete($"{fileName}.json");
                }

                if (File.Exists($"{fileName}.png"))
                {
                    File.Delete($"{fileName}.png");
                }

                return null;
            }
        }

        /// <summary>
        /// Adds a <see cref="DrawableObject.DrawableObject"/> to the <see cref="Drawing"/>
        /// contained in the <see cref="BackendWrapper"/>
        /// </summary>
        /// <param name="imageType">The type of image the <see cref="DrawableObject.DrawableObject"/> should have</param>
        /// <param name="location">The location of the <see cref="DrawableObject.DrawableObject"/></param>
        /// <param name="size">The size of the <see cref="DrawableObject.DrawableObject"/></param>
        public void Add(ImageType imageType, Point location, Size size)
        {
            try
            {
                // Add the object
                DrawableObject.DrawableObject drawableObject = DrawableObject.DrawableObject.Create(imageType, location, size);
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Add;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = drawableObject;
                this.ExecuteCommand(parameters);

                // Select the object
                parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Select;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = drawableObject;
                this.ExecuteCommand(parameters);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Brings the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the front of the <see cref="Drawing"/>
        /// </summary>
        public void BringToFront()
        {
            try
            {
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.BringToIndex;
                parameters.DrawableObjectList = this.drawableObjectList;
                int startIndex = -1;

                for (int i = 0; i < this.drawableObjectList.Count; ++i)
                {
                    if (this.drawableObjectList.RenderOrder[i].Selected)
                    {
                        startIndex = i;
                        break;
                    }
                }

                // If this isn't true, nothing was selected
                if (startIndex >= 0)
                {
                    parameters.StartIndex = startIndex;
                    parameters.TargetIndex = this.drawableObjectList.Count - 1;
                    this.ExecuteCommand(parameters);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Sends the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the back of the <see cref="Drawing"/>
        /// </summary>
        public void SendToBack()
        {
            try
            {
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.BringToIndex;
                parameters.DrawableObjectList = this.drawableObjectList;
                int startIndex = -1;

                for (int i = 0; i < this.drawableObjectList.Count; ++i)
                {
                    if (this.drawableObjectList.RenderOrder[i].Selected)
                    {
                        startIndex = i;
                        break;
                    }
                }

                // If this isn't true, nothing was selected
                if (startIndex >= 0)
                {
                    parameters.StartIndex = startIndex;
                    parameters.TargetIndex = 0;
                    this.ExecuteCommand(parameters);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Deselect whatever <see cref="DrawableObject.DrawableObject"/>
        /// is currently selected
        /// </summary>
        public void Deselect()
        {
            try
            {
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Deselect;
                parameters.DrawableObjectList = this.drawableObjectList;
                this.ExecuteCommand(parameters);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Duplicates the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// with a small offset in location
        /// </summary>
        public void Duplicate()
        {
            try
            {
                // Duplicate the object
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Duplicate;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = this.drawableObjectList.SelectedObject;
                this.ExecuteCommand(parameters);

                // Move the object slightly (so it isn't in the exact same place)
                parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Move;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = this.drawableObjectList.RenderOrder[this.drawableObjectList.Count - 1];
                Point newLocation = new Point(parameters.AffectedDrawableObject.Location.X + 15, parameters.AffectedDrawableObject.Location.Y + 15);
                parameters.Location = newLocation;
                this.ExecuteCommand(parameters);

                // Select the new object
                parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Select;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = this.drawableObjectList.RenderOrder[this.drawableObjectList.Count - 1];
                this.ExecuteCommand(parameters);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Moves the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the given location
        /// </summary>
        /// <param name="location">The new location the selected item should move to</param>
        public void Move(Point location)
        {
            try
            { 
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Move;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = this.drawableObjectList.SelectedObject;
                parameters.Location = location;
                this.ExecuteCommand(parameters);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Removes the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// from the <see cref="Drawing"/>
        /// </summary>
        public void Remove()
        {
            try
            {
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Remove;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = this.drawableObjectList.SelectedObject;
                this.ExecuteCommand(parameters);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Scales the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the new <see cref="Size"/>
        /// </summary>
        /// <param name="size">The new <see cref="Size"/> of the currently selected <see cref="DrawableObject.DrawableObject"/></param>
        public void Scale(Size size)
        {
            try
            {
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Scale;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = this.drawableObjectList.SelectedObject;
                parameters.Size = size;
                this.ExecuteCommand(parameters);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Selects the topmost <see cref="DrawableObject.DrawableObject"/> at the given location
        /// in the drawing
        /// </summary>
        /// <param name="location">The location of the <see cref="DrawableObject.DrawableObject"/> to be selected</param>
        public void Select(Point location)
        {
            try
            {
                var parameters = CommandParameters.Create();
                parameters.CommandType = CommandType.Select;
                parameters.DrawableObjectList = this.drawableObjectList;
                parameters.AffectedDrawableObject = this.drawableObjectList.GetObjectFromLocation(location);
                if (parameters.AffectedDrawableObject != null)
                {
                    this.ExecuteCommand(parameters);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Undoes the last <see cref="ICommand"/> to be added to the undo stack
        /// </summary>
        public void Undo()
        {
            try
            {
                ICommand commandToUndo = this.undoStack.Pop();
                commandToUndo.Undo();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Clears the <see cref="Drawing"/>, sets a new background and size,
        /// clears the undo stack, etc.
        /// </summary>
        /// <param name="backgroundImage">The new background image</param>
        /// <param name="size">The new <see cref="Size"/></param>
        public void ClearAndStartNewDrawing(Image backgroundImage, Size size)
        {
            this.drawing.Dispose();
            this.drawableObjectList.Clear();
            this.drawingBackground = backgroundImage;

            this.drawing = Drawing.Create(backgroundImage, this.drawableObjectList, size);
            this.undoStack.Clear();
        }

        /// <summary>
        /// Gets the <see cref="Size"/> of the selected item in the
        /// <see cref="Drawing"/> if an item is selected
        /// </summary>
        /// <returns>The size of the selected item</returns>
        public Size? GetSizeOfSelectedItem()
        {
            if (this.drawableObjectList.SelectedObject != null)
            {
                return this.drawableObjectList.SelectedObject.Size;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the location of the selected item in the
        /// <see cref="Drawing"/> if an item is selected
        /// </summary>
        /// <returns>The location of the selected item</returns>
        public Point? GetLocationOfSelectedItem()
        {
            if (this.drawableObjectList.SelectedObject != null)
            {
                return this.drawableObjectList.SelectedObject.Location;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates and executes an <see cref="ICommand"/> based on the given
        /// <see cref="CommandFactory.CommandArguments"/> object. The command is
        /// then added to the undo stack
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="ICommand"/> object</param>
        private void ExecuteCommand(CommandParameters parameters)
        {
            ICommand command = this.commandFactory.CreateCommand(parameters);
            command.Execute();
            this.undoStack.Push(command);
        }
    }
}
