using System;
using System.Drawing;
using System.Windows.Forms;

namespace Berzerk2
{
    class player : Control
    {
        public int speed = 3;
        public double angle = Math.PI;
        public PictureBox body = new PictureBox();
        public int deltaX, deltaY;
        public int yDir;
        Point prevPoint = new Point(800, 400);

        public player()
        {
            Location = new Point(40, 40);
            body.BackColor = Color.Yellow;
            body.Location = new Point(40, 40);
            body.Size = new Size(10,10);
            deltaX = (int)(Math.Cos(angle) * speed);
            deltaY = (int)(Math.Sin(angle) * speed);
        }
        public void drawPlayer()
        {
            if (yDir != 0)
            {
                string collisions = calcCollisions();
                switch (collisions)
                {
                    case "n":
                        Location = new Point(Location.X + yDir * deltaX, Location.Y + yDir * deltaY);
                        body.Location = new Point(body.Location.X + yDir * deltaX, body.Location.Y + yDir * deltaY);
                        break;
                    case "v":
                        Location = new Point(Location.X + yDir * deltaX, Location.Y);
                        body.Location = new Point(body.Location.X + yDir * deltaX, body.Location.Y);
                        break;
                    case "h":
                        Location = new Point(Location.X, Location.Y + yDir * deltaY);
                        body.Location = new Point(body.Location.X, body.Location.Y + yDir * deltaY);
                        break;
                    case "b":
                        break;
                    default:
                        break;
                }
            }
        }
        public void rotatePlayer(Point point)
        {
            if (prevPoint.X < point.X) angle += 0.075;
            else if (prevPoint.X > point.X) angle -= 0.075;
            if (angle < 0) angle += (2 * Math.PI);
            if (angle > (2 * Math.PI)) angle -= (2 * Math.PI);

            deltaX = (int) (Math.Cos(angle) * speed);
            deltaY = (int)(Math.Sin(angle) * speed);

            prevPoint = point;
        }
        public string calcCollisions()
        {
            string collision = "n";
            /* n: None
             * v: Vertical Collision
             * h: Horizontal Collision
             * b: Both Collisions
             */
            int[] map = Berzerk2.map;
            int mapX = Berzerk2.mapX;
            int mapS = Berzerk2.mapS;
            if (yDir == 1)
            {
                if (map[(Location.Y / mapS) * mapX + (Location.X + 10) / mapS] == 1 && (angle < Math.PI / 2 || angle > 3 * Math.PI / 2)
                 || map[(Location.Y / mapS) * mapX + (Location.X - 10) / mapS] == 1 && (angle > Math.PI / 2 && angle < 3 * Math.PI / 2))
                {
                    collision = "h";
                }
                if (map[((Location.Y + 10) / mapS) * mapX + (Location.X) / mapS] == 1 && (angle < Math.PI || angle > 2 * Math.PI) // is facing down
                 || map[(Location.Y - 10) / mapS * mapX + (Location.X) / mapS] == 1 && (angle > Math.PI && angle < 2 * Math.PI)) // is looking up
                {
                    if (!collision.Equals("n")) collision = "b";
                    else collision = "v";
                }
            }
            else if (yDir == -1)
            {
                if (map[(Location.Y / mapS) * mapX + (Location.X + 10) / mapS] == 1 && !(angle < Math.PI / 2 || angle > 3 * Math.PI / 2)
                 || map[(Location.Y / mapS) * mapX + (Location.X - 10) / mapS] == 1 && !(angle > Math.PI / 2 && angle < 3 * Math.PI / 2))
                {
                    collision = "h";
                }
                if (map[((Location.Y + 10) / mapS) * mapX + (Location.X) / mapS] == 1 && !(angle < Math.PI || angle > 2 * Math.PI) // is facing down
                 || map[(Location.Y - 10) / mapS * mapX + (Location.X) / mapS] == 1 && !(angle > Math.PI && angle < 2 * Math.PI)) // is looking up
                {
                    if (!collision.Equals("n")) collision = "b";
                    else collision = "v";
                }
            }
            return collision;
        }
    }
}
