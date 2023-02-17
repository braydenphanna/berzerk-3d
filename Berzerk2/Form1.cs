/*
 * BERZERK 3D
 * 
 * --------------------TO-DO--------------------     -: Not done     X: done     /:Kind of done or unimportant
 *  / Fix angle being inversed over the X axis
 *  X Move in the direction you're facing
 *  / Perfect movement delay
 *  X Collisions
 *  - Strafe left and right
 *  X Remove 2D view
 *  - Add sprites (enemies, other 2d things in the game)
 *  - add first person pov/shooting  (do this with graphics/ pixelated looking?)
 *  - levels, doors
 *  - Title Screen
 *  - UI
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Berzerk2
{
    public partial class Berzerk2 : Form
    {
        player p = new player();
        int X = 0;
        public static int mapX = 16, mapY = 8, mapS = 20;
        public static int[] map = new int[]
        {
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
            1,0,0,0,0,1,0,1,1,0,0,0,0,0,0,1,
            1,0,0,0,0,1,0,1,1,0,1,0,0,0,0,1,
            1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,
            1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,
            1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
        };


        static int rays = 60;
        bool[] shading = new bool[rays];
        int[,] rayPoints = new int[rays, 2];
        int[,] lineArr3d = new int[rays, 2];

        public Berzerk2()
        {
            InitializeComponent();
            this.Size = new Size(960,680);
            this.KeyPreview = true;
        }

        #region input methods
        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (Cursor.Position.X < this.Bounds.X + 50) X = this.Size.Width;
            else X = this.Bounds.X + 50;
            p.rotatePlayer(Cursor.Position);
            raycast();
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    p.yDir = 1;
                    break;
                case Keys.S:
                    p.yDir = -1;
                    break;
                case Keys.Oemtilde:
                    open2dView();
                    break;
                default:
                    break;
            }
        }
        private void keyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    p.yDir = 0;
                    break;
                case Keys.S:
                    p.yDir = 0;
                    break;
                default:
                    break;
            }
        }
        private void onMouseLeave(object sender, EventArgs e)
        {
            if (Cursor.Position.X < this.Bounds.X + 50 || Cursor.Position.X > this.Size.Width-50) 
                Cursor.Position = new Point(X, Cursor.Position.Y);
        }
        #endregion

        private void drawTick(object sender, EventArgs e)
        {
            p.drawPlayer();
            raycast();
        }
        public void raycast()
        {
            int mapArrayX, mapArrayY, mapArrayPos, depth;
            double rayX = 0, rayY = 0, rayAngle, xOffset = 0, yOffset = 0;
            double finalDistance = 0;

            double degree = 0.0174533 *(60/rays);
            rayAngle = p.angle- degree*30;
            if (rayAngle < 0) rayAngle += (2 * Math.PI);
            else if (rayAngle > (2 * Math.PI)) rayAngle -= (2 * Math.PI);

            for (int i = 0; i < rays; i++)
            {
                //check horizontal lines
                depth = 0;
                double hDistance = int.MaxValue, horRayX = rayX, horRayY = rayY;
                double aTan = -1 / Math.Tan(rayAngle);
                if (rayAngle > Math.PI) //looking up
                {
                    rayY = (((int)p.Location.Y / mapS) * mapS) - 0.0001; //round rays Y hit point to nearest 64th point
                    rayX = (p.Location.Y - rayY) * aTan + p.body.Location.X;
                    yOffset = -mapS;
                    xOffset = -yOffset * aTan;
                }
                if (rayAngle < Math.PI) //looking down
                {
                    rayY = (((int)p.Location.Y / mapS) * mapS) + mapS; //round rays Y hit point to nearest 64th point
                    rayX = (p.Location.Y - rayY) * aTan + p.body.Location.X;
                    yOffset = mapS;
                    xOffset = -yOffset * aTan;
                }
                if (rayAngle == 0 || rayAngle == Math.PI) //looking left or right
                {
                    rayX = p.body.Location.X;
                    rayY = p.body.Location.Y;
                    depth = 8;
                }
                while (depth < 8)
                {
                    mapArrayX = (int)(rayX / mapS);
                    mapArrayY = (int)(rayY / mapS);
                    mapArrayPos = mapArrayY * mapX + mapArrayX;
                    if (mapArrayPos < mapX * mapY && mapArrayPos > 0 && map[mapArrayPos] == 1) //wall hit!
                    {
                        depth = 8;  
                        horRayX = rayX;
                        horRayY= rayY;
                        hDistance = distance(p.Location.X, p.Location.Y, horRayX, horRayY);
                    }
                    else
                    {
                        rayX += xOffset;
                        rayY += yOffset;
                        depth += 1;
                    }
                    
                }

                //check Vertical lines
                depth = 0;
                double vDistance = int.MaxValue, vertRayX = rayX, vertRayY = rayY;
                double nTan = -Math.Tan(rayAngle);
                if (rayAngle > (Math.PI/2) && rayAngle < (3 * Math.PI / 2)) //looking left
                {
                    rayX = (((int)p.Location.X / mapS) * mapS) - 0.0001; //round rays Y hit point to nearest 64th point
                    rayY = (p.Location.X - rayX) * nTan + p.body.Location.Y;
                    xOffset = -mapS;
                    yOffset = -xOffset * nTan;
                }
                if (rayAngle < (Math.PI / 2) || rayAngle > (3 * Math.PI / 2)) //looking right
                {
                    rayX = (((int)p.Location.X / mapS) * mapS) + mapS; //round rays Y hit point to nearest 64th point
                    rayY = (p.Location.X - rayX) * nTan + p.body.Location.Y;
                    xOffset = mapS;
                    yOffset = -xOffset * nTan;
                }
                if (rayAngle == (Math.PI / 2) || rayAngle == (3 * Math.PI / 2)) //looking up or down
                {
                    rayX = p.body.Location.X;
                    rayY = p.body.Location.Y;
                    depth = 8;
                }
                while (depth < 8)
                {
                    mapArrayX = (int)(rayX / mapS);
                    mapArrayY = (int)(rayY / mapS);
                    mapArrayPos = mapArrayY * mapX + mapArrayX;
                    if (mapArrayPos < mapX * mapY && mapArrayPos > 0 && map[mapArrayPos] == 1) //wall hit!
                    {
                        depth = 8;
                        vertRayX = rayX;
                        vertRayY = rayY;
                        vDistance = distance(p.Location.X, p.Location.Y, vertRayX, vertRayY);
                        

                    }
                    else
                    {
                        rayX += xOffset;
                        rayY += yOffset;
                        depth += 1;
                    }

                }

                if (vDistance>hDistance)
                {
                    rayX = horRayX;
                    rayY = horRayY;
                    finalDistance = hDistance;

                    shading[i] = false;
                }
                else if (vDistance < hDistance)
                {
                    rayX = vertRayX;
                    rayY = vertRayY;
                    finalDistance = vDistance;
                    shading[i] = true;
                }

                rayAngle += degree;
                if (rayAngle < 0) rayAngle += (2 * Math.PI);
                else if (rayAngle > (2 * Math.PI)) rayAngle -= (2 * Math.PI);

                rayPoints[i, 0] = (int)rayX;
                rayPoints[i, 1] = (int)rayY;

                //draw 3d lines
                double fixedAngle = p.angle - rayAngle;
                if (fixedAngle < 0) fixedAngle += 2 * Math.PI;
                if (fixedAngle > 2 * Math.PI) fixedAngle -= 2 * Math.PI;
                finalDistance = finalDistance * Math.Cos(fixedAngle);

                double lineHeight = (mapS *640) / finalDistance;
                if (lineHeight > 640) lineHeight = 640;
                double lineOffset = 320 - lineHeight / 2;

                lineArr3d[i, 0] = (int)lineHeight;
                lineArr3d[i, 1] = (int)lineOffset;

            }
            drawGFX();
        }
        public double distance(double ax, double ay, double bx, double by)
        {
            return Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
        }

        private void onMouseClick(object sender, MouseEventArgs e)
        {
            drawGFX();
        }

        public void drawGFX()
        {
            Bitmap bmp = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                int j = 0;
                int Move = 0;
                while (j < (rayPoints.Length - 2) / 2)
                {
                    Pen wallPen = new Pen(Color.Red, 1);
                    if (shading[j] == true) wallPen = new Pen(Color.FromArgb(0, 0, 158), 1);
                    else if (shading[j] == false) wallPen = new Pen(Color.FromArgb(0, 0, 128), 1);
                    Brush wallBrush = new SolidBrush(Color.Red);
                    if (shading[j] == true) wallBrush = new SolidBrush(Color.FromArgb(0, 0, 158));
                    else if (shading[j] == false) wallBrush = new SolidBrush(Color.FromArgb(0, 0, 128));

                    Rectangle wall = new Rectangle(Move, lineArr3d[j, 1], 16, lineArr3d[j, 0]);
                    g.DrawRectangle(wallPen, wall);
                    g.FillRectangle(wallBrush, wall);
                    Move += 16;
                    j++;
                }
                /*
                Brush gunBrush = new SolidBrush(Color.FromArgb(0, 255, 0));
                Brush gunBrushDark = new SolidBrush(Color.FromArgb(0, 225, 0));
                PointF[] points = {
                    new PointF(Width/2+210, Height),
                    new PointF(Width/2+260, Height),
                    new PointF(Width/2+140, Height-215),
                    new PointF(Width/2+120, Height-215)};
                g.FillPolygon(gunBrush, points);
                PointF[] points2 = {
                    new PointF(Width/2+180, Height),
                    new PointF(Width/2+210, Height),
                    new PointF(Width/2+120, Height-215),
                    new PointF(Width/2+120, Height-175)};
                g.FillPolygon(gunBrushDark, points2);
                */
            }
            using (var g = CreateGraphics()) // or ‘e.Graphics’ in Paint event
            {
                g.DrawImageUnscaled(bmp, 0, 0);
            }
            
        }
        public void open2dView()
        {
            Form level = new Form();
            level.BackColor = Color.Black;
            level.Size = new Size(mapS * mapX + 15, mapS * mapY + 35);

            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    if (map[y * mapX + x] == 1)
                    {
                        PictureBox pb = new PictureBox();
                        pb.Location = new Point(x * mapS, y * mapS);
                        pb.Size = new Size(mapS, mapS);
                        pb.BackColor = Color.White;
                        level.Controls.Add(pb);
                    }
                }
            }

            level.Controls.Add(p);
            level.Controls.Add(p.body);
            level.Show();

        }
    }
}
