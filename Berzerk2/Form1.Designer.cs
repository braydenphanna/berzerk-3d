
namespace Berzerk2
{
    partial class Berzerk2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.drawTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // drawTimer
            // 
            this.drawTimer.Enabled = true;
            this.drawTimer.Interval = 50;
            this.drawTimer.Tick += new System.EventHandler(this.drawTick);
            // 
            // Berzerk2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Name = "Berzerk2";
            this.Text = "Berzerk2";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.onMouseClick);
            this.MouseLeave += new System.EventHandler(this.onMouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.onMouseMove);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer drawTimer;
    }
}

