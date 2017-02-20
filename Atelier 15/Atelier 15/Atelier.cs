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
using System.Net.Sockets;
using System.Net;

namespace AtelierXNA
{
    public class Atelier : Microsoft.Xna.Framework.Game
    {
        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        SpriteBatch GestionSprites { get; set; }
        List<Section> Sections { get; set; }
        Caméra CaméraJeu { get; set; }
        InputManager GestionInput { get; set; }
        //DataPiste DonnéesPiste { get; set; }
        List<Section> ListeSections { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        TcpClient client;
        string IP = "127.0.0.1";
        int PORT = 5001;

        public Atelier()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            Vector3 positionCaméra = new Vector3(200, 10, 200);
            Vector3 cibleCaméra = new Vector3(10, 0, 10);
            ListeSections = new List<Section>();

            //serveur 

            client = new TcpClient();
            client.NoDelay = true;
            client.Connect(IP, PORT);

            //

            GestionInput = new InputManager(this);
            Components.Add(GestionInput);
            CaméraJeu = new CaméraSubjective(this, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            Components.Add(CaméraJeu);
            Sections = new List<Section>();
            Components.Add(new Afficheur3D(this));
            //Components.Add(new ArrièrePlanDéroulant(this, "CielÉtoilé", INTERVALLE_MAJ_STANDARD));
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    Section newSection = new Section(this, new Vector2(200 * i, 100 * j), new Vector2(200, 200), 1f, Vector3.Zero, Vector3.Zero, new Vector3(200, 25, 200), new string[] { "Herbe", "Sable" }, INTERVALLE_MAJ_STANDARD);
                    Sections.Add(newSection);
                    ListeSections.Add(newSection);
                }
            }
            foreach (Section s in ListeSections)
            {
                Components.Add(s);
            }
            
            //Components.Add(new Terrain(this, 1f, Vector3.Zero, Vector3.Zero, new Vector3(256, 25, 256), "GrandeCarte", "DétailsTerrain", 5, INTERVALLE_MAJ_STANDARD));
            //Components.Add(new Terrain(this, 1f, Vector3.Zero, Vector3.Zero, new Vector3(200, 25, 200), "CarteTest", "DétailsTerrain", 5, INTERVALLE_MAJ_STANDARD));
            Components.Add(new AfficheurFPS(this, "Arial20", Color.Red, INTERVALLE_CALCUL_FPS));
            Components.Add(new Piste(this, 1f, Vector3.Zero, Vector3.Zero, INTERVALLE_MAJ_STANDARD, 20000, 20000));

            //Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(RessourcesManager<Texture2D>), new RessourcesManager<Texture2D>(this, "Textures"));
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(DataPiste), new DataPiste("SplineX.txt", "SplineY.txt"));
            GestionSprites = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), GestionSprites);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GérerClavier();
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= INTERVALLE_MAJ_STANDARD)
            {
                foreach (Section s in Sections)
                {
                    BoundingFrustum boundFrustum = new BoundingFrustum(Matrix.Multiply(CaméraJeu.Vue,CaméraJeu.Projection));
                    if (CaméraJeu.Frustum.Intersects(s.SphereDeCollision))
                    {
                        if (CaméraJeu.Position.X - s.SphereDeCollision.Center.X < 20f || CaméraJeu.Position.Z 
                            -s.SphereDeCollision.Center.Z < 20f)
                        {
                            s.EstVisible = true;
                        }
                        else
                        {
                            s.EstVisible = false;
                        }
                    }
                    else
                    {
                        s.EstVisible = false;
                    }
                }
                TempsÉcouléDepuisMAJ = 0;
            }
            
        }

        private void GérerClavier()
        {
            if (GestionInput.EstEnfoncée(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}

