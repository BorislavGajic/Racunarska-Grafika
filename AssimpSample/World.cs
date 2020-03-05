// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using System.Windows.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        /// <summary>
        ///	 Ugao rotacije Meseca
        /// </summary>
        private float m_moonRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije Zemlje
        /// </summary>
        private float m_earthRotation = -1.0f;

        private bool animacija = false;

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 7000.0f;

        private float skaliranje_stapa = 1f;

        private float rotacija_kugli = 0f;
        private float boja_svetla = 0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        private enum TextureObjects { Stap = 0, Tepih };
        private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;
        private uint[] m_textures = null;

        private string[] m_textureFiles = { "..//..//3D Models//Bilijar//bili//wood3.jpg", "..//..//3D Models//Bilijar//venice_1084.jpg" };


        private float[] ambijentalna_komponenta = new float[] { 0.93f, 0.17f, 0.17f, 1.0f };

        private bool m_animation;
        private float[] _amb = { 0.3f, 0, 0, 1 };
        private OpenGL gl;



        private DispatcherTimer timer1;
        private bool prvi_put_udara = true;
        private bool drugi = true;
        private bool treci = true;
        private bool cetvrti = true;
        private bool peti = true;
        private bool sesti = true;
        private bool sedmi = true;
        private bool osmi = true;
        private bool deveti = true;
        private bool deseti = true;
        private bool jedanaesti = true;
        private bool dvanaesti = true;
        private bool trinaesti = true;
        private bool cetrnaesti = true;
        private bool petnaesti = true;
        private bool sesnaesti = true;
        private bool sedamnaesti = true;
        private bool osamnaesti = true;
        private bool devetnaesti = true;
        private bool dvadeseti = true;
        private bool dvajedan = true;
        private bool dvadva = true;
        private bool dvatri = true;
        private bool dvacetiri = true;
        private bool dvapet = true;

        private float[] pozicija_stapa = new float[] { 850f, 1200f, -1550f };
        private float[] rotacija_stapa = new float[] { 0f, 0f, 0f };
        private float[] pozicija_kugle1 = new float[] { -850, 1150f, 400f };
        private float[] pozicija_kugle2 = new float[] { -1100f, 1150f, 200f };

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public bool Animation
        {
            get { return m_animation; }
            set { m_animation = value; }
        }

        public float[] AmbSpot
        {
            get { return _amb; }
            set
            {
                _amb = value;
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, _amb);
            }
        }

        public OpenGL GL
        {
            get;
            set;
        }

        public float Skaliranje_stapa { get => skaliranje_stapa; set => skaliranje_stapa = value; }
        public float Rotacija_kugli { get => rotacija_kugli; set => rotacija_kugli = value; }
        public float Boja_svetla { get => boja_svetla; set => boja_svetla = value; }
        public bool Animacija { get => animacija; set => animacija = value; }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, int width, int height, OpenGL gl)
        {
            this.gl = gl;
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_textures = new uint[m_textureCount];
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);
            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE_MODE);

            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.Enable(OpenGL.GL_NORMALIZE);

            podesiOsvetljenje(gl);
            podesiTeksturu(gl);

            gl.Enable(OpenGL.GL_LIGHTING);

            // ovo gasim jer kad ga upalim iako mi je po zadatku tako dato teksture ne lice ni na sta ali pokazati da imam
            //gl.Enable(OpenGL.GL_TEXTURE_2D);
            //gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);
            //gl.GenTextures(m_textureCount, m_textures);



            m_scene.LoadScene();
            m_scene.Initialize();
        }

        private void UdaracStapa(object sender, EventArgs e)
        {

           

            if (prvi_put_udara)
            {
                Console.WriteLine("prvi upad");
                pozicija_stapa = new float[] { 850f, 1200f, -1550f };
                //rotacija_stapa = new float[] { 165f, 0f, 0f };
                prvi_put_udara = false;
            }
            else
            {
                if (drugi)
                {
                    Console.WriteLine("drugi upad");
                    pozicija_stapa[1] -= 50f;
                    pozicija_stapa[2] -= 70f;
                    drugi = false;
                }
                else if(treci)
                {
                    Console.WriteLine("treci upad");
                    pozicija_stapa[1] += 50f;
                    pozicija_stapa[2] += 70f;
                    pozicija_kugle1[0] = -1100f;
                    pozicija_kugle1[2] = 200f;
                    //pozicija_kugle2[0] = -150f;
                    //pozicija_kugle2[2] = 900f;
                    treci = false;
                }
                else if (cetvrti)
                {
                    pozicija_kugle2[0] -= 100;
                    pozicija_kugle2[2] -= 100;
                    cetvrti = false;
                }
                else if (peti)
                {
                    pozicija_kugle2[0] -= 100;
                    pozicija_kugle2[2] -= 100;
                    peti = false;
                }
                else if (sesti)
                {
                    pozicija_kugle2[0] -= 100;
                    pozicija_kugle2[2] -= 100;
                    sesti = false;
                }
                else if (sedmi)
                {
                    pozicija_kugle2[0] -= 100;
                    pozicija_kugle2[2] -= 100;
                    sedmi = false;
                }
                else if (osmi)
                {
                    pozicija_kugle2[0] -= 100;
                    pozicija_kugle2[2] -= 100;
                    osmi = false;
                }
                else if (deveti)
                {
                    pozicija_kugle2[0] -= 100;
                    pozicija_kugle2[2] -= 100;
                    deveti = false;
                }
                else if (deseti)
                {
                    pozicija_kugle2[0] -= 100;
                    pozicija_kugle2[2] -= 100;
                    deseti = false;
                }
                else if (jedanaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] -= 100;
                    jedanaesti = false;
                }
                else if (dvanaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] -= 100;
                    dvanaesti = false;
                }
                else if (trinaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 100;
                    trinaesti = false;
                }
                else if (cetrnaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 100;
                    cetrnaesti = false;
                }
                else if (petnaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 100;
                    petnaesti = false;
                }
                else if (sesnaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 100;
                    sesnaesti = false;
                }
                else if (sedamnaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 100;
                    sedamnaesti = false;
                }
                else if (osamnaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 100;
                    osamnaesti = false;
                }
                else if (devetnaesti)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 100;
                    devetnaesti = false;
                }
                else if (dvadeseti)
                {
                    pozicija_kugle2[0] += 200;
                    pozicija_kugle2[2] += 200;
                    dvadeseti = false;
                }
                else if (dvajedan)
                {
                    pozicija_kugle2[0] += 200;
                    pozicija_kugle2[2] += 200;
                    dvajedan = false;
                }
                else if (dvadva)
                {
                    pozicija_kugle2[0] += 200;
                    pozicija_kugle2[2] += 200;
                    dvadva = false;
                }
                else if (dvatri)
                {
                    pozicija_kugle2[0] += 200;
                    pozicija_kugle2[2] += 200;
                    dvatri = false;
                }
                else if (dvacetiri)
                {
                    pozicija_kugle2[0] += 100;
                    pozicija_kugle2[2] += 150;
                    dvacetiri = false;
                }
                else if (dvapet)
                {
                    pozicija_kugle2[1] -= 100;
                    dvapet = false;
                }
                else
                {
                    timer1.Stop();
                }



            }

        }

        private void podesiTeksturu(OpenGL gl)
        {
            //podesavanje teksture
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            //gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);// ovako je ruzno

            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);
                Bitmap image = new Bitmap(m_textureFiles[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData imageData = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0); //ova metoda pravi mipmape, ne smao jednu teksturu
                //gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGB8, imageData.Width, imageData.Height, 0, OpenGL.GL_BGR, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0);

                image.UnlockBits(imageData);
                image.Dispose();
            }

        }

        private void podesiOsvetljenje(OpenGL gl)
        {



            float[] light0pos = new float[] { 0f, 1500f, 0f, -m_sceneDistance, 1.0f };
            float[] light0ambient = new float[] { 0.1f, 0.1f, 0.1f, 1.0f };
            float[] light0diffuse = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };


            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);

            float[] light1pos = new float[] { 0f, 1400f, 0f, -m_sceneDistance, 1f };
            float[] light1diffuse = new float[] { 0.5f, 0.5f, 0.5f, 1.0f };
            float[] light1direction = new float[] { 0.0f, -1.0f, 0.0f };


            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambijentalna_komponenta);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, light1diffuse);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 25.0f);  // crvena 25
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, light1direction);
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT1);

            gl.Enable(OpenGL.GL_NORMALIZE);




            gl.ShadeModel(OpenGL.GL_SMOOTH);

        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Viewport(0, 0, m_width, m_height);
            
            float[] light0pos = new float[] { 0f, 1500f, 0f, 1f };
            float[] light1direction = new float[] { 0f, -1f, 0f, 0f };
            float[] light1pos = new float[] { 0f, 1500f, 0f, 1f };

            gl.LoadIdentity();
            // ovo gasim jer posle radi neke cudne stvari kad se rotira ali znam da pozicioniram kameru nisam debil.
            gl.LookAt(3, 9000,3, 0, 0, -m_sceneDistance, 0, 1, 0);
            
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            
            
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, light1direction);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
            
           

            gl.Enable(OpenGL.GL_AUTO_NORMAL);
            gl.PushMatrix();
            gl.Translate(0.0f, 0.0f, -m_sceneDistance);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            IscrtajPod(gl);
            IscrtajZid(gl, 0f, 2000f, -4000f, 5000, 2000, 50);
            IscrtajZid(gl, 0f, 2000f, 4000f, 5000, 2000, 50);
            IscrtajZid(gl, 5000f, 2000f, 0f, 50, 2000, 4000);
            IscrtajZid(gl, -5000f, 2000f, 0f, 50, 2000, 4000);
            
            IscrtajLoptu1(gl, -850, 400);
            IscrtajLoptu2(gl, -1100, 200);
            
            IscrtajStap(gl);

            if(animacija==true)
            {
                //Console.WriteLine("animacija");

                timer1 = new DispatcherTimer();
                timer1.Interval = TimeSpan.FromMilliseconds(100);
                timer1.Start();
                timer1.Tick += new EventHandler(UdaracStapa);
            }



            m_scene.Draw();

            //tekst
            gl.PushMatrix();
            {
                
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                gl.LoadIdentity();
                gl.Ortho2D(-20, 5, -4, 8);

                gl.MatrixMode(OpenGL.GL_MODELVIEW);
                gl.LoadIdentity();
                gl.Color(255f, 255f, 255f);
                gl.Scale(0.5f, 0.5f, 0.5f);
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
                {
                    gl.PushMatrix();

                    gl.Translate(-2.5f, -3f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "Predmet: Racunarska grafika");
                    gl.PopMatrix();

                    gl.PushMatrix();
                    gl.Translate(-2.5f, -3.1f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "_______________________");
                    gl.PopMatrix();


                    gl.PushMatrix();
                    gl.Translate(-2.5f, -4f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "Sk.god: 2019/20.");
                    gl.PopMatrix();

                    gl.PushMatrix();
                    gl.Translate(-2.5f, -4.1f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "_______________________");
                    gl.PopMatrix();


                    gl.PushMatrix();
                    gl.Translate(-2.5f, -5f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "Ime: Borislav");
                    gl.PopMatrix();

                    gl.PushMatrix();
                    gl.Translate(-2.5f, -5.1f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "_______________________");
                    gl.PopMatrix();

                    gl.PushMatrix();
                    gl.Translate(-2.5f, -6f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "Prezime: Gajic");
                    gl.PopMatrix();

                    gl.PushMatrix();
                    gl.Translate(-2.5f, -6.1f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "_______________________");
                    gl.PopMatrix();

                    gl.PushMatrix();
                    gl.Translate(-2.5f, -7f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "Sifra zad: 18.2");
                    gl.PopMatrix();

                    gl.PushMatrix();
                    gl.Translate(-2.5f, -7.1f, 0f);
                    gl.DrawText3D("Helvetica", 12f, 1f, 0.1f, "_______________________");
                    gl.PopMatrix();

                }
            }
            gl.PopMatrix();

            gl.PopMatrix();

            gl.Viewport(0, 0, m_width, m_height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(50f, (double)m_width / m_height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.FrontFace(OpenGL.GL_CCW);
            gl.LookAt(600, 2000, 200, 0, 0, -6 * m_sceneDistance, 0, 1, 0);
            // Oznaci kraj iscrtavanja
            gl.Flush();
        }


        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0, 0, m_width, m_height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(50f, (double)width / height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
            }
        }


        private void IscrtajPod(OpenGL gl)
        {
            gl.PushMatrix();
            gl.Color(1f, 1f, 1f);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Tepih]);
            gl.MatrixMode(OpenGL.GL_TEXTURE);
            gl.LoadIdentity();
            gl.Scale(10f, 10f, 1.0f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.Begin(OpenGL.GL_QUADS);
            gl.TexCoord(1.0f, 0.0f);
            gl.Vertex(-5000.0f, 0f, -4000.0f);
            gl.TexCoord(1.0f, 1.0f);
            gl.Vertex(-5000.0f, 0f, 4000.0f);
            gl.TexCoord(0.0f, 1.0f);
            gl.Vertex(5000.0f, 0f, 4000.0f);
            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(5000.0f, 0f, -4000.0f);
            gl.End();
            gl.PopMatrix();
        }

        public void IscrtajZid(OpenGL gl, float x, float y, float z, float xx, float yy, float zz)
        {
            gl.PushMatrix();
            gl.Color(1f, 0.5f, 0f);
            gl.Translate(x, y, z);
            gl.Scale(xx, yy, zz);

            Cube wall = new Cube();
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
            wall.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
        }

        public void IscrtajLoptu1(OpenGL gl, float x, float z)
        {
            gl.PushMatrix();
            gl.Rotate(0, rotacija_kugli/10, 0);
            gl.Translate(pozicija_kugle1[0], pozicija_kugle1[1], pozicija_kugle1[2]);
            gl.Scale(40, 60, 60);

            Sphere ball = new Sphere();
            ball.CreateInContext(gl);
            gl.Color(0f, 0f, 0f);

            ball.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
        }

        public void IscrtajLoptu2(OpenGL gl, float x, float z)
        {
            gl.PushMatrix();
            gl.Rotate(0, rotacija_kugli/10, 0);
            gl.Translate(pozicija_kugle2[0], pozicija_kugle2[1], pozicija_kugle2[2]);
            gl.Scale(40, 60, 60);

            Sphere ball = new Sphere();
            ball.CreateInContext(gl);
            gl.Color(1f, 1f, 1f);

            ball.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
        }

        public void IscrtajStap(OpenGL gl)
        {
            gl.PushMatrix();
            gl.Rotate(0, 230, 0);
            gl.Translate(pozicija_stapa[0], pozicija_stapa[1], pozicija_stapa[2]);
            gl.Scale(20, 20, 1800);


            Cylinder cue = new Cylinder();
            cue.BaseRadius = 2;
            cue.TopRadius = 0.8;
            cue.CreateInContext(gl);
            //gl.Color(0.55f, 0.36f, -0.11f);
            cue.BaseRadius = 0.45f + skaliranje_stapa / 40;
            cue.TopRadius = 0.13f + skaliranje_stapa / 80;

            gl.Scale(3*skaliranje_stapa,1, 1);

            cue.TextureCoords = true;
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Stap]);
            

            cue.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
        }

        public void RestartAnimacije()
        {
            prvi_put_udara = true;
            drugi = true;
            treci = true;
            cetvrti = true;
            peti = true;
            sesti = true;
            sedmi = true;
            osmi = true;
            deveti = true;
            deseti = true;
            jedanaesti = true;
            dvanaesti = true;
            trinaesti = true;
            cetrnaesti = true;
            petnaesti = true;
            sesnaesti = true;
            sedamnaesti = true;
            osamnaesti = true;
            devetnaesti = true;
            dvadeseti = true;
            dvajedan = true;
            dvadva = true;
            dvatri = true;
            dvacetiri = true;
            dvapet = true;
            pozicija_kugle1[0] = -850f;
            pozicija_kugle1[1] = 1150f;
            pozicija_kugle1[2] = 400f;
            pozicija_kugle2[0] = -1100f;
            pozicija_kugle2[1] = 1150f;
            pozicija_kugle2[2] = 200f;
    }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
