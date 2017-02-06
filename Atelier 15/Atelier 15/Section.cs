using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Section : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Vector2 Position { get; set; }
        Vector2 �tendue { get; set; }
        Vector2 Extr�mit� { get; set; }
        double Norme�tendue { get; set; }
        bool estVisible;
        public bool EstVisible
        {
            get { return estVisible;}
            set
            {
                bool tampon = estVisible;
                estVisible = value;
                if(tampon != estVisible)
                {
                    foreach (GameComponent x in Components)
                    {
                        x.Enabled = !x.Enabled;
                    }
                }
            }
        }
        List<GameComponent> Components { get; set; }
        public Section(Game game, Vector2 position, Vector2 �tendue)
            : base(game)
        {
            Position = position;
            �tendue = �tendue;
        }

        public override void Initialize()
        {
            Extr�mit� = Position + �tendue;
            Norme�tendue = Math.Sqrt(Math.Pow(�tendue.X, 2) + Math.Pow(�tendue.Y, 2));
            base.Initialize();
        }

        public void IsItIn(Vector2 position, Vector2 direction)
        {
            if (CheckIfIn(position) || CheckIfIn(position + direction))
            {
                EstVisible = true;
            }
            else
            {
                EstVisible = false;
            }
        }

        public bool CheckIfIn(Vector2 position)
        {
            bool value = false;
            if(position.X <= Extr�mit�.X && position.X >= Position.X && position.Y <= Extr�mit�.Y && position.Y >= Position.Y)
            {
                value = true;
            }
            return value;
        }
        public void AddComponent(GameComponent x)
        {
            Components.Add(x);
        }
    }
}
