using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelierXNA
{
    class Maison : PrimitiveDeBaseAnimée
    {
        const int NB_TRIANGLES_MURS = 8;
        const int NB_SOMMETS_PAR_TRIANGLE = 3;
        const int NB_TRIANGLES_TOIT = 4;
        int NbSommetsMurs { get; set; }
        int NbSommetsToit { get; set; }
        Vector3 Origine { get; set; }
        VertexPositionTexture[] SommetsMurs { get; set; }
        VertexPositionTexture[] SommetsToit { get; set; }
        Vector2[] PtsTextureMurs { get; set; }
        Vector2[] PtsTextureToit { get; set; }
        Vector3 Étendue { get; set; }
        BlendState GestionAlpha { get; set; }
        string NomTextureMurs { get; set; }
        Texture2D TextureMurs;
        string NomTextureToit { get; set; }
        Texture2D TextureToit { get; set; }
        RessourcesManager<Texture2D> gestionnaireDeTextures;
        protected BasicEffect EffetDeBase { get; private set; }

        public Maison(Game jeu, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float rayon, Vector2 charpente, string nomTexture, float intervalleMAJ)
    : base(jeu, échelleInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            NomTextureMurs = nomTexture;
            Étendue = new Vector3(rayon, rayon, rayon);
            Origine = new Vector3(-Étendue.X / 2, 0, Étendue.Z / 2);//sur le sol
        }
        public override void Initialize()
        {
            NbSommets = NB_TRIANGLES_MURS + 2;
            NbSommetsMurs = NB_TRIANGLES_MURS + 2;
            NbSommetsToit = NB_TRIANGLES_TOIT * NB_SOMMETS_PAR_TRIANGLE;
            CréerTableauSommets();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            gestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            TextureMurs = gestionnaireDeTextures.Find(NomTextureMurs);
            InitialiserParamètresEffetDeBase();
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            BlendState oldBlendState = GraphicsDevice.BlendState;
            GraphicsDevice.BlendState = GestionAlpha;
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, SommetsMurs, 0, NB_TRIANGLES_MURS);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, SommetsToit, 0, NB_TRIANGLES_TOIT);
            }
            base.Draw(gameTime);
            GraphicsDevice.BlendState = oldBlendState;
        }
        private void CréerTableauSommets()
        {
            PtsTextureMurs = new Vector2[NbSommetsMurs];
            SommetsMurs = new VertexPositionTexture[NbSommetsMurs];
            PtsTextureToit = new Vector2[NB_SOMMETS_PAR_TRIANGLE];
            SommetsToit = new VertexPositionTexture[NbSommetsToit];
            CréerTableauPointsTexture();
        }
        private void CréerTableauPointsTexture()
        {
            //points murs
            for (int i = 0; i < NbSommetsMurs; ++i)
            {
                PtsTextureMurs[i] = new Vector2((float)(i / 2) / 4, (i + 1) % 2);
            }
            //points toit
            PtsTextureToit[0] = new Vector2(0, 1);
            PtsTextureToit[1] = new Vector2(1f / 2, 0);
            PtsTextureToit[2] = new Vector2(1, 1);
        }
        protected override void InitialiserSommets()
        {
            //sommets murs
            int i = 0;
            SommetsMurs[i] = new VertexPositionTexture(Origine, PtsTextureMurs[i++]);
            SommetsMurs[i] = new VertexPositionTexture(new Vector3(Origine.X, Origine.Y + Étendue.Y, Origine.Z), PtsTextureMurs[i++]);
            while (i < NbSommetsMurs)
            {
                SommetsMurs[i] = new VertexPositionTexture(new Vector3(Origine.X + (1 - i / 6) * Étendue.X, Origine.Y + (i % 2) * Étendue.Y, Origine.Z - (i / 4) % 2 * Étendue.Z), PtsTextureMurs[i]);
                ++i;
            }
            //sommets toit
            i = 0;
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X, Origine.Y + Étendue.Y, Origine.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X, Origine.Y + Étendue.Y, Origine.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X / 2, Origine.Y + 3f / 2 * Étendue.Y, Origine.Z - Étendue.Z / 2), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X, Origine.Y + Étendue.Y, Origine.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X, Origine.Y + Étendue.Y, Origine.Z - Étendue.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X / 2, Origine.Y + 3f / 2 * Étendue.Y, Origine.Z - Étendue.Z / 2), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X, Origine.Y + Étendue.Y, Origine.Z - Étendue.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X, Origine.Y + Étendue.Y, Origine.Z - Étendue.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X / 2, Origine.Y + 3f / 2 * Étendue.Y, Origine.Z - Étendue.Z / 2), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X, Origine.Y + Étendue.Y, Origine.Z - Étendue.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X, Origine.Y + Étendue.Y, Origine.Z), PtsTextureToit[i % 3]);
            SommetsToit[i++] = new VertexPositionTexture(new Vector3(Origine.X + Étendue.X / 2, Origine.Y + 3f / 2 * Étendue.Y, Origine.Z - Étendue.Z / 2), PtsTextureToit[i % 3]);


        }
        private void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.TextureEnabled = true;
            EffetDeBase.Texture = TextureMurs;
            GestionAlpha = BlendState.AlphaBlend;
        }
    }
}
