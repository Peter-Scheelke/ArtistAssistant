using ArtistAssistant.DrawableObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtistAssistant
{
    public partial class Form1 : Form
    {
        DrawableObjectList list;

        Drawing drawing;

        public Form1()
        {
            InitializeComponent();
            this.list = DrawableObjectList.Create();
            this.drawing = Drawing.Create(ImagePool.GetImage(ImageType.Mountain), this.list, new Size(this.Width/2, this.Height/2));
            this.BackgroundImage = this.drawing.RenderedDrawing;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            drawing.Size = this.Size;
            //Point p = e.Location;
            //DrawableObject.DrawableObject obj = DrawableObject.DrawableObject.Create(ImageType.Cloud, p, new Size(40, 40));
            //this.list.Add(obj);
            this.BackgroundImage = drawing.RenderedDrawing;
            this.Refresh();
        }
    }
}
