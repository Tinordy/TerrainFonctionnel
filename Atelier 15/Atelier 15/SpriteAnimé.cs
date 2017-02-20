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
    public class SpriteAnimé : Sprite, IDestructible
    {
        protected Vector2 DescriptionImage { get; set; }
        protected float IntervalleMAJAnimation { get; private set; }
        protected Rectangle RectangleSource { get; set; }
        protected Point Delta { get; private set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        protected int MargeGauche { get; set; }
        protected int MargeDroite { get; set; }
        protected int MargeHaut { get; set; }
        protected int MargeBas { get; set; }
        public bool ADétruire { get; set; }

        public SpriteAnimé(Game jeu, string nomImage, Vector2 position,
                           Rectangle zoneAffichage, Vector2 descriptionImage,
                           float intervalleMAJAnimation)
            : base(jeu, nomImage, position, zoneAffichage)
        {
            DescriptionImage = descriptionImage;
            IntervalleMAJAnimation = intervalleMAJAnimation;
        }
        public override void Initialize()
        {
            base.Initialize();
            TempsÉcouléDepuisMAJ = 0;
            MargeGauche = 0;
            MargeHaut = 0;
            Échelle = MathHelper.Min((float)ZoneAffichage.Width / (float)Delta.X, (float)ZoneAffichage.Height / (float)Delta.Y);
            RectangleSource = new Rectangle(0, 0, Delta.X, Delta.Y);
            ZoneCollision = new Rectangle((int)Position.X, (int)Position.Y,
                                          (int)(Échelle * Delta.X), (int)(Échelle * Delta.Y));

        }
        protected override void CalculerMarges()
        {
            Delta = new Point((int)(Image.Width / DescriptionImage.X),
                              (int)(Image.Height / DescriptionImage.Y));
            MargeDroite = Game.Window.ClientBounds.Width;
            MargeBas = Game.Window.ClientBounds.Height;
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;

            if (TempsÉcouléDepuisMAJ >= IntervalleMAJAnimation)
            {

                RectangleSource = new Rectangle((RectangleSource.X + Delta.X) % Image.Width,
                                                (int)((RectangleSource.Y + Math.Floor((double)(RectangleSource.X + Delta.X) / Image.Width) * Delta.Y) % Image.Height)
                                                , Delta.X, Delta.Y);

                TempsÉcouléDepuisMAJ = 0;
            }
        }

        protected void UpdateAnimationMobile(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;

            if (TempsÉcouléDepuisMAJ >= IntervalleMAJAnimation)
            {

                RectangleSource = new Rectangle((RectangleSource.X + Delta.X) % Image.Width,
                                                0, Delta.X, Delta.Y);

                TempsÉcouléDepuisMAJ = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(Image, new Vector2(ZoneCollision.X, ZoneCollision.Y), RectangleSource, Color.White, 0,
                                new Vector2(0, 0), Échelle, SpriteEffects.None, 0);
        }
    }
}
