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
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent, ICollisionable
    {
        float EvaluationCollisionX { get; set; }
        float EvaluationCollisionY { get; set; }
        string NomImage { get; set; }
        protected float Échelle { get; set; }
        public  Vector2 Position { get; protected set; }
        protected Texture2D Image { get; private set; }
        protected Rectangle ZoneAffichage { get; set; }
        protected SpriteBatch GestionSprites { get; private set; }
        protected RessourcesManager<Texture2D> GestionnaireDeTextures { get; private set; }
        protected Rectangle ZoneCollision { get; set; }
        public Sprite(Game jeu, string nomImage, Vector2 position, Rectangle zoneAffichage)
            : base(jeu)
        {
            NomImage = nomImage;
            Position = position;
            ZoneAffichage = zoneAffichage;
        }
        protected override void LoadContent()
        {
            EvaluationCollisionX = 
            EvaluationCollisionY = Game.Window.ClientBounds.Height / 5;
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            ChargerImage(NomImage);
            Image = GestionnaireDeTextures.Find(NomImage);
            if(!(this is IDestructible))
            {
                Échelle = MathHelper.Min((float)ZoneAffichage.Width / (float)Image.Width,
                                         (float)ZoneAffichage.Height / (float)Image.Height);
                ZoneCollision = new Rectangle((int)Position.X, (int)Position.Y,
                                               (int)(Échelle * Image.Width),(int)(Échelle * Image.Height));
            }
            CalculerMarges();
        }

        protected void ChargerImage(string nomImage)// même si le paramètre est obsolète ici, 
                                                    // on doit le garder, puisqu'il permet d'appeler la méthode ailleurs
        {
            Image = GestionnaireDeTextures.Find(nomImage);
        }
        protected virtual void CalculerMarges() { }

        protected void CréerZoneCollision()
        {
            ZoneCollision = new Rectangle((int)Position.X, (int)Position.Y, ZoneCollision.Width, ZoneCollision.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(Image, ZoneCollision, Color.White);
        }

        public bool EstEnCollision(Object autreObjet)
        {
            bool retour = false;
            Sprite autre = autreObjet as Sprite;
            Vector2 posRes = Position - autre.Position;
            if((Math.Abs(posRes.X) < ZoneCollision.Width && Math.Abs(posRes.Y) < ZoneCollision.Height)||
                (posRes.X > - ZoneCollision.Width && Math.Abs(posRes.Y) < ZoneCollision.Height))

            {
                if (ZoneCollision.Intersects((autre).ZoneCollision)) { retour = true; }
            }
            
            return retour;
        }
    }
}
