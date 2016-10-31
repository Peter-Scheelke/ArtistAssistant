//-----------------------------------------------------------------------
// <copyright file="ArtistAssistantForm.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using DrawableObject;

    /// <summary>
    /// An <see cref="ArtistAssistantForm"/> allows the user to add various
    /// objects to an image. That image can be saved to/downloaded from the
    /// cloud.
    /// </summary>
    public partial class ArtistAssistantForm : Form
    {
        /// <summary>
        /// The <see cref="BackendWrapper"/> that handles
        /// all of the commands, drawn objects, etc.
        /// </summary>
        private BackendWrapper backend;

        /// <summary>
        /// A list of menus that can be opened and closed by buttons on the <see cref="ArtistAssistantForm"/>
        /// Used to hide any that are open whenever a new menu gets opened
        /// </summary>
        private List<Panel> menus;

        /// <summary>
        /// The background image of the drawing
        /// </summary>
        private Bitmap backgroundImage;

        /// <summary>
        /// The current mode selected by the user for the <see cref="ArtistAssistantForm"/>
        /// </summary>
        private DrawingMode currentMode;

        /// <summary>
        /// The type of image items that are added to the <see cref="ArtistAssistantForm"/> should have
        /// </summary>
        private ImageType currentImageType;

        /// <summary>
        /// The color of things that are not selected
        /// </summary>
        private Color deselectionColor;

        /// <summary>
        /// The color of things that are selected
        /// </summary>
        private Color selectionColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistAssistantForm"/> class
        /// </summary>
        public ArtistAssistantForm()
        {
            this.InitializeComponent();
            this.backgroundImage = null;
            this.backend = null;
            this.SetupMenuList();
            this.HideMenus();
            this.currentMode = DrawingMode.None;
            this.currentImageType = ImageType.Cloud;
            this.selectionColor = Color.LightBlue;
            this.deselectionColor = Color.White;
        }

        /// <summary>
        /// Add all the menus of the <see cref="ArtistAssistantForm"/> to a list of menus.
        /// This list is used to hide all the menus easily
        /// </summary>
        private void SetupMenuList()
        {
            this.menus = new List<Panel>();
            this.menus.Add(this.createDrawingMenuPanel);
            this.menus.Add(this.addItemMenuPanel);
            this.menus.Add(this.scaleItemMenuPanel);
        }

        /// <summary>
        /// Hides all the menus in the <see cref="ArtistAssistantForm"/>
        /// </summary>
        private void HideMenus()
        {
            foreach (Panel menu in this.menus)
            {
                menu.Visible = false;
            }
        }

        /// <summary>
        /// Sets the back color of the <see cref="PictureBox"/> corresponding to the
        /// selected <see cref="ImageType"/> to a color indicating that it has been
        /// selected. Sets all the other ones to a color indicating that they aren't
        /// selected
        /// </summary>
        private void SetPictureBoxBackColors()
        {
            this.cloudPictureBox.BackColor = this.deselectionColor;
            this.mountainPictureBox.BackColor = this.deselectionColor;
            this.pinePictureBox.BackColor = this.deselectionColor;
            this.pondPictureBox.BackColor = this.deselectionColor;
            this.rainPictureBox.BackColor = this.deselectionColor;
            this.treePictureBox.BackColor = this.deselectionColor;

            switch (this.currentImageType)
            {
                case ImageType.Cloud:
                    this.cloudPictureBox.BackColor = this.selectionColor;
                    break;
                case ImageType.Mountain:
                    this.mountainPictureBox.BackColor = this.selectionColor;
                    break;
                case ImageType.Pine:
                    this.pinePictureBox.BackColor = this.selectionColor;
                    break;
                case ImageType.Pond:
                    this.pondPictureBox.BackColor = this.selectionColor;
                    break;
                case ImageType.Rain:
                    this.rainPictureBox.BackColor = this.selectionColor;
                    break;
                case ImageType.Tree:
                    this.treePictureBox.BackColor = this.selectionColor;
                    break;
            }
        }

        /// <summary>
        /// Colors the side panel buttons based on what the current mode is
        /// </summary>
        private void ColorBySelectedMode()
        {
            this.newObjectButton.BackColor = this.deselectionColor;
            this.selectButton.BackColor = this.deselectionColor;
            this.moveButton.BackColor = this.deselectionColor;

            switch (this.currentMode)
            {
                case DrawingMode.Add:
                    this.newObjectButton.BackColor = this.selectionColor;
                    break;
                case DrawingMode.Move:
                    this.moveButton.BackColor = this.selectionColor;
                    break;
                case DrawingMode.Select:
                    this.selectButton.BackColor = this.selectionColor;
                    break;
            }

            this.Refresh();
        }

        /// <summary>
        /// The event handler that handles MouseClick events for <see cref="drawingPictureBox"/>
        /// Figures out what the currently selected mode is and applies an appropriate action
        /// to itself
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void DrawingPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.backend == null)
            {
                return;
            }

            switch (this.currentMode)
            {
                case DrawingMode.Add:
                    this.backend.Add(this.currentImageType, e.Location, new Size((int)this.addWidthNumericUpDown.Value, (int)this.addHeightNumericUpDown.Value));
                    break;
                case DrawingMode.Move:
                    this.backend.Move(e.Location);
                    break;
                case DrawingMode.Select:
                    this.backend.Select(e.Location);
                    break;
            }

            this.drawingPictureBox.BackgroundImage = this.backend.RenderedDrawing;
            this.Refresh();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="newDrawingButton"/>
        /// Opens a menu for creating a new drawing
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void NewDrawingButton_Click(object sender, System.EventArgs e)
        {
            bool isVisible = this.createDrawingMenuPanel.Visible;
            this.HideMenus();
            this.createDrawingMenuPanel.Visible = !isVisible;
            this.ColorBySelectedMode();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="colorButton"/>
        /// Selects the color that the background of the new drawing should be
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void ColorButton_Click(object sender, System.EventArgs e)
        {
            DialogResult result = this.backgroundColorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Color color = this.backgroundColorDialog.Color;
                this.backgroundImage = new Bitmap(this.drawingPictureBox.Size.Width, this.drawingPictureBox.Size.Height);
                using (Graphics graphics = Graphics.FromImage(this.backgroundImage))
                {
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        graphics.FillRectangle(brush, 0, 0, this.drawingPictureBox.Size.Width, this.drawingPictureBox.Size.Height);
                    }
                }
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="startDrawingButton"/>
        /// Clears the <see cref="BackendWrapper"/> and sets it up with a new size and background
        /// image. Does nothing if the <see cref="backgroundImage"/> is null
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void StartDrawingButton_Click(object sender, System.EventArgs e)
        {
            if (this.backgroundImage != null)
            {
                if (this.backend == null)
                {
                    this.backend = BackendWrapper.Create(this.backgroundImage, this.drawingPictureBox.Size);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Starting a new image will delete the current on.", "Are You Sure?", MessageBoxButtons.YesNoCancel);
                    if (result != DialogResult.Yes)
                    {
                        return;
                    }

                    this.backend.ClearAndStartNewDrawing(this.backgroundImage, this.drawingPictureBox.Size);
                }

                this.drawingPictureBox.BackgroundImage = this.backend.RenderedDrawing;
                this.backgroundImage = null;
                this.createDrawingMenuPanel.Hide();
                this.Refresh();
            }
            else
            {
                MessageBox.Show("Please select either a background color or a background image.", "No Background Selected");
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="backgroundFileButton"/>
        /// Opens a file dialog and allows the user to select an image file to use as
        /// the background image
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void BackgroundFileButton_Click(object sender, System.EventArgs e)
        {
            DialogResult result = this.openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filePath = this.openFileDialog.FileName;
                try
                {
                    this.backgroundImage = (Bitmap)Image.FromFile(filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("Sorry, that file is not supported.", "Unsupported File");
                }
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="newObjectButton"/>
        /// Opens a menu that allows the user to add new objects to the drawing
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void NewObjectButton_Click(object sender, EventArgs e)
        {
            bool isVisible = this.addItemMenuPanel.Visible;
            this.HideMenus();
            this.currentMode = DrawingMode.Add;
            this.addItemMenuPanel.Visible = !isVisible;
            this.ColorBySelectedMode();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="cloudPictureBox"/>
        /// Selects a cloud as the image of items added to the <see cref="ArtistAssistantForm"/>
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void CloudPictureBox_Click(object sender, EventArgs e)
        {
            this.currentImageType = ImageType.Cloud;
            this.SetPictureBoxBackColors();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="mountainPictureBox"/>
        /// Selects a mountain as the image of items added to the <see cref="ArtistAssistantForm"/>
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void MountainPictureBox_Click(object sender, EventArgs e)
        {
            this.currentImageType = ImageType.Mountain;
            this.SetPictureBoxBackColors();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="pinePictureBox"/>
        /// Selects a pine tree as the image of items added to the <see cref="ArtistAssistantForm"/>
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void PinePictureBox_Click(object sender, EventArgs e)
        {
            this.currentImageType = ImageType.Pine;
            this.SetPictureBoxBackColors();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="pondPictureBox"/>
        /// Selects a pond as the image of items added to the <see cref="ArtistAssistantForm"/>
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void PondPictureBox_Click(object sender, EventArgs e)
        {
            this.currentImageType = ImageType.Pond;
            this.SetPictureBoxBackColors();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="rainPictureBox"/>
        /// Selects a rain cloud as the image of items added to the <see cref="ArtistAssistantForm"/>
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void RainPictureBox_Click(object sender, EventArgs e)
        {
            this.currentImageType = ImageType.Rain;
            this.SetPictureBoxBackColors();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="treePictureBox"/>
        /// Selects a tree as the image of items added to the <see cref="ArtistAssistantForm"/>
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void TreePictureBox_Click(object sender, EventArgs e)
        {
            this.currentImageType = ImageType.Tree;
            this.SetPictureBoxBackColors();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="moveButton"/>
        /// Sets the current mode to the move mode. If that mode is already selected,
        /// it opens the move menu
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void MoveButton_Click(object sender, EventArgs e)
        {
            this.currentMode = DrawingMode.Move;
            this.ColorBySelectedMode();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="selectButton"/>
        /// Sets the current mode to selection
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void SelectButton_Click(object sender, EventArgs e)
        {
            this.currentMode = DrawingMode.Select;
            this.ColorBySelectedMode();
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="duplicateButton"/>
        /// Duplicates the currently selected item
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void DuplicateButton_Click(object sender, EventArgs e)
        {
            if (this.backend != null)
            {
                this.backend.Duplicate();
                this.Refresh();
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="deleteButton"/>
        /// Deletes the currently selected item
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (this.backend != null)
            {
                this.backend.Remove();
                this.Refresh();
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="bringToFrontButton"/>
        /// Brings the selected item to the front of the drawing
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void BringToFrontButton_Click(object sender, EventArgs e)
        {
            if (this.backend != null)
            {
                this.backend.BringToFront();
                this.Refresh();
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="sendToBackButton"/>
        /// Sends the currently selected item to the back of the drawing
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void SendToBackButton_Click(object sender, EventArgs e)
        {
            if (this.backend != null)
            {
                this.backend.SendToBack();
                this.Refresh();
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="undoButton"/>
        /// Undoes the most recent change that hasn't been undone yet (doesn't undo undoes)
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (this.backend != null)
            {
                this.backend.Undo();
                this.Refresh();
            }
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="scaleButton"/>
        /// Opens the menu that allows the currently selected item to be scaled
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void ScaleButton_Click(object sender, EventArgs e)
        {
            bool isVisible = this.scaleItemMenuPanel.Visible;
            this.HideMenus();
            this.scaleItemMenuPanel.Visible = !isVisible;
        }

        /// <summary>
        /// The event handler that handles Click events for <see cref="scaleObjectButton"/>
        /// Scales the currently selected item to the values in the <see cref="NumericUpDown"/>
        /// boxes nearby
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void ScaleObjectButton_Click(object sender, EventArgs e)
        {
            if (this.backend != null)
            {
                int width = (int)this.scaleWidthNumericUpDown.Value;
                int height = (int)this.scaleHeightNumericUpDown.Value;
                this.backend.Scale(new Size(width, height));
                this.Refresh();
            }
        }

        /// <summary>
        /// The event handler that handles KeyDown events for the <see cref="ArtistAssistantForm"/> (and everything else)
        /// Used to catch key commands (such as control + z for undo)
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void ArtistAssistantForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Key Commands:
            // Control Z -> undo
            // Control F -> bring to front
            // Control B -> send to back
            // Control N -> click the new drawing button
            // Control V -> duplicate
            // S -> Click the select button
            // Delete -> Delete the currently selected item
            // M -> Click the move button
            // N -> Click the new object button
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                if (this.backend != null)
                {
                    this.backend.Undo();
                    this.Refresh();
                }
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.F)
            {
                if (this.backend != null)
                {
                    this.backend.BringToFront();
                    this.Refresh();
                }
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.B)
            {
                if (this.backend != null)
                {
                    this.backend.SendToBack();
                    this.Refresh();
                }
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                if (this.backend != null)
                {
                    this.backend.Duplicate();
                    this.Refresh();
                }
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.N)
            {
                this.newDrawingButton.PerformClick();
            }
            else if (e.KeyCode == Keys.S)
            {
                this.selectButton.PerformClick();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (this.backend != null)
                {
                    this.backend.Remove();
                    this.Refresh();
                }
            }
            else if (e.KeyCode == Keys.M)
            {
                this.moveButton.PerformClick();
            }
            else if (e.KeyCode == Keys.N)
            {
                this.newObjectButton.PerformClick();
            }
        }
    }
}
