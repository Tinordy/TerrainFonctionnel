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
    public class Section :Terrain
    {
        Vector2 �tendue { get; set; }
        Vector2 Extr�mit� { get; set; }
        double Norme�tendue { get; set; }
        bool estVisible;
        public bool EstVisible
        {
            get { return estVisible;}
            set
            {
                Enabled = value;
                
            }
        }
        List<GameComponent> Components { get; set; }
        public Section(Game game, Vector2 origine, Vector2 �tendue2,float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector3 �tendue,
                       string[] nomsTexturesTerrain, float intervalleMAJ)
            : base(game, origine,homoth�tieInitiale, rotationInitiale, positionInitiale, �tendue, nomsTexturesTerrain, intervalleMAJ)
        {
            �tendue = �tendue2;
        }

        public override void Initialize()
        {
            Extr�mit� = Coin + �tendue;
            Norme�tendue = Math.Sqrt(Math.Pow(�tendue.X, 2) + Math.Pow(�tendue.Y, 2));
            base.Initialize();
        }

        public void AddComponent(GameComponent x)
        {
            Components.Add(x);
        }
    }
}
