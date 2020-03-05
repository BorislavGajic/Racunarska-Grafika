using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;
using System.Text.RegularExpressions;


namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();

            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Bilijar"), "BilliardTable.3DS", (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.Width, (int)openGLControl.Height);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F10: this.Close(); break;
                case Key.W:
                    if (m_world.RotationX - 5.0f < 20.0f)
                        break;
                    else
                    {
                        m_world.RotationX -= 5.0f; break;
                    }
                case Key.S:
                    if (m_world.RotationX + 5.0f > 90.0f)
                        break;
                    else
                    {
                        m_world.RotationX += 5.0f; break;
                    }
                case Key.T:
                    if (m_world.RotationX - 5.0f <20.0f)
                        break;
                    else
                    {
                        m_world.RotationX -= 5.0f; break;
                    }
                case Key.G:
                    if (m_world.RotationX + 5.0f > 90.0f)
                        break;
                    else
                    {
                        m_world.RotationX += 5.0f; break;
                    }
                case Key.A: m_world.RotationY -= 5.0f; break;
                case Key.D: m_world.RotationY += 5.0f; break;
                case Key.F: m_world.RotationY -= 5.0f; break;
                case Key.H: m_world.RotationY += 5.0f; break;
                case Key.Add: m_world.SceneDistance -= 700.0f; break;
                case Key.Subtract: m_world.SceneDistance += 700.0f; break;
                case Key.X: m_world.Animacija = true; break;
                case Key.R: m_world.RestartAnimacije(); break;
                case Key.F2: this.Close(); break;
                    /*OpenFileDialog opfModel = new OpenFileDialog();
                    bool result = (bool) opfModel.ShowDialog();
                    if (result)
                    {

                        try
                        {
                            World newWorld = new World(Directory.GetParent(opfModel.FileName).ToString(), Path.GetFileName(opfModel.FileName), (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
                            m_world.Dispose();
                            m_world = newWorld;
                            m_world.Initialize(openGLControl.OpenGL);
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta:\n" + exp.Message, "GRESKA", MessageBoxButton.OK );
                        }
                    }*/

            }
        }

        private void skaliranjeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool ok = new Regex("[0-9]").IsMatch(e.Text);
            float broj;

            if (ok)
            {
                broj = float.Parse(this.skaliranjeTB.Text + e.Text);
                m_world.Skaliranje_stapa = broj / 100;
            }

            e.Handled = !ok;
        }

        private void brzinaTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool ok = new Regex("[0-9]").IsMatch(e.Text);
            float broj;

            if (ok)
            {
                m_world.Rotacija_kugli = int.Parse(this.brzinaTB.Text + e.Text);
            }

            e.Handled = !ok;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_world.Animation)
            {
                return;
            }

            switch (this.svjetloComboBox.SelectedIndex)
            {
                case 0:
                    m_world.AmbSpot = new float[] { 1f, 0f, 0f, 1.0f }; break;
                case 1:
                    m_world.AmbSpot = new float[] { 0f, 0f, 1f, 1.0f }; break;
                case 2:
                    m_world.AmbSpot = new float[] { 0f, 1f, 0f, 1.0f }; break;

            }
            Console.WriteLine(m_world.AmbSpot[0] + " " + m_world.AmbSpot[1] + " " + m_world.AmbSpot[2]);
        }
    }
}
