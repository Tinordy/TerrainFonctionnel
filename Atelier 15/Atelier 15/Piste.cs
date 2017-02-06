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
    public class Piste : PrimitiveDeBaseAnim�e
    {
        DataPiste Donn�esPiste { get; set; }
        int HAUTEUR_INITIALE = 0;

        Vector3 Origine { get; set; }
        List<Vector2> PointsBordureExt { get; set; }
        List<Vector2> PointsBordureInt { get; set; }
        Color CouleurPiste { get; set; }
        int NbDeTriangles { get; set; }
        int NbDeSommets { get; set; }
        VertexPositionColor[] Sommets { get; set; }
        BasicEffect EffetDeBase { get; set; }
        int NbColonnes { get; set; }
        int NbRang�es { get; set; }
       public Piste(Game jeu, float homoth�tieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ, int nbColonnes, int nbRang�es)
           : base(jeu, homoth�tieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            NbColonnes = nbColonnes;
            NbRang�es = nbRang�es;
        }
        public override void Initialize()
        {
            Origine = new Vector3(-NbColonnes / 2, 0, -NbRang�es / 2);
            Donn�esPiste = Game.Services.GetService(typeof(DataPiste)) as DataPiste;
            CouleurPiste = Color.Black;
            ObtenirDonn�esPiste();
            NbDeSommets = PointsBordureExt.Count + PointsBordureInt.Count + 2;
            NbDeTriangles = NbDeSommets - 2;
            Cr�erTableauSommets();
            base.Initialize();
        }
        void Cr�erTableauSommets()
        {
            Sommets = new VertexPositionColor[NbDeSommets];
        }
        protected override void InitialiserSommets()
        {
            for (int i = 0; i < NbDeSommets - 3; i += 2)
            {
                float posXExt = Origine.X + PointsBordureExt[i / 2].X;
                float posZExt = Origine.Z + PointsBordureExt[i / 2].Y;
                float posXInt = Origine.X + PointsBordureInt[i / 2].X;
                float posZInt = Origine.Z + PointsBordureInt[i / 2].Y;

                Sommets[i + 1] = new VertexPositionColor(new Vector3(posXExt, HAUTEUR_INITIALE, posZExt), CouleurPiste);
                Sommets[i] = new VertexPositionColor(new Vector3(posXInt, HAUTEUR_INITIALE, posZInt), CouleurPiste);
            }
            Sommets[NbDeSommets - 2] = Sommets[0];
            Sommets[NbDeSommets - 1] = Sommets[1];
        }


        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParam�tresEffetDeBase();
            base.LoadContent();
        }
        void ObtenirDonn�esPiste()
        {
            PointsBordureExt = Donn�esPiste.GetBordureExt�rieure();
            PointsBordureInt = Donn�esPiste.GetBordureInt�rieur();
        }
        void InitialiserParam�tresEffetDeBase()
        {
            EffetDeBase.VertexColorEnabled = true;
        }
        public override void Draw(GameTime gameTime)
        {
            DepthStencilState ancienDepthStencilState = GraphicsDevice.DepthStencilState;
            DepthStencilState temporaire = new DepthStencilState();
            temporaire.DepthBufferEnable = false;
            GraphicsDevice.DepthStencilState = temporaire;
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = Cam�raJeu.Vue;
            EffetDeBase.Projection = Cam�raJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, 0, NbDeTriangles);
            }
            GraphicsDevice.DepthStencilState = ancienDepthStencilState;
        }
    }
}
