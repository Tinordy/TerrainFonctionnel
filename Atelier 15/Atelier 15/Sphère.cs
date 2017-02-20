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
    public class Sphère : SpriteAnimé
    {
        const float CONVERSION_RADIANS = (float)(180 / Math.PI * 2);
        const int ANGLE_MIN = 15;
        const int ANGLE_MAX = 75;
        const float ZONE_SANS_APPARITION = 0.66f;
        const double INCRÉMENTATION_ANGLE = Math.PI;
        const int DÉPLACEMENT_PIXELS = 4;
        const int CERCLE_COMPLET_ANGLE = 360;
        float IntervalleMAJDéplacement { get; set; }
        Random Seed { get; set; }
        double Angle { get; set; }
        Vector2 PositionTampon { get; set; }
        float TempsÉcouléDepuisMAJDéplacement { get; set; }
        float DéplacementEnX { get; set; }
        float DéplacementEnY { get; set; }
        float HauteurImage { get; set; }
        float LargeurImage { get; set; }

        public Sphère(Game jeu, string nomImage, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImage,
                      float intervalleMAJAnimation, float intervalleMAJDéplacement)
            : base(jeu, nomImage, position, zoneAffichage, descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJDéplacement = intervalleMAJDéplacement;
        }

        public override void Initialize()
        {
            base.Initialize();
            Angle = Seed.Next(ANGLE_MIN, ANGLE_MAX + 1) * CONVERSION_RADIANS ;
            HauteurImage = RectangleSource.Height * Échelle;
            LargeurImage = RectangleSource.Width * Échelle;
            Position = new Vector2((float)Seed.NextDouble() * (MargeDroite - LargeurImage),
                                   (float)Seed.NextDouble() * MargeBas * ZONE_SANS_APPARITION);
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Seed = Game.Services.GetService(typeof(Random)) as Random;
        }

        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJDéplacement += tempsÉcoulé;

            if (TempsÉcouléDepuisMAJDéplacement >= IntervalleMAJDéplacement)
            {
                EffectuerMiseÀJour();
                CréerZoneCollision();
                TempsÉcouléDepuisMAJDéplacement = 0;
            }
            base.Update(gameTime);
        }

        void EffectuerMiseÀJour()
        {
            if (Position.X <= MargeGauche)
            {
                Angle = Angle % 2 * Math.PI < Math.PI ? INCRÉMENTATION_ANGLE - Angle : INCRÉMENTATION_ANGLE * 2 - Angle - Math.PI;
            }
            else
            {
                if (Position.X + LargeurImage>= MargeDroite)
                {
                    Angle = Angle % 2 * Math.PI < Math.PI / 2? INCRÉMENTATION_ANGLE - Angle : INCRÉMENTATION_ANGLE + (2 * INCRÉMENTATION_ANGLE - Angle);
                }
                else
                {
                    if(Position.Y + HauteurImage >= MargeBas)
                    {
                        Angle = Angle % 2 * Math.PI < Math.PI*3/2 ? INCRÉMENTATION_ANGLE*3/2 - Angle + INCRÉMENTATION_ANGLE/ 2 : INCRÉMENTATION_ANGLE*2- Angle;
                    }
                    else
                    {
                        if(Position.Y <= MargeHaut)
                        {
                             Angle = Angle % 2 * Math.PI < Math.PI / 2 ? 2 * INCRÉMENTATION_ANGLE - Angle : 2 * INCRÉMENTATION_ANGLE - Angle;
                        }
                    }
                }
            }
            Position = new Vector2(Math.Max(Position.X + DÉPLACEMENT_PIXELS * (float)Math.Cos(Angle), 0),
                                   Math.Max(Position.Y + DÉPLACEMENT_PIXELS * (float)Math.Sin(Angle), 0));
        }

    }
}
