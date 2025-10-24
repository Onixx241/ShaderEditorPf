using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShaderEditor
{


    public partial class MainWindow
    {
        //future flag for saving/loading
        public string? CurrentShader { get; set; }

        public ShaderCompiler current { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 6
            };

            OpenTkControl.Start(settings);

            this.current = new ShaderCompiler();
            this.current.ShaderPath = "Shaders\\shader.frag";
        }

        private void OpenTkControl_OnRender(TimeSpan delta) 
        {
            GL.ClearColor(0.4f, 0.5f, 0.2f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            this.current.SetupScreen();

        }

        private void textChangedEventHandler(object sender, TextChangedEventArgs args) 
        {
            Debug.WriteLine("Text Changed!");

            string? text = sender.ToString();

            string? cleanedText = text.Substring(text.IndexOf("TextBox:") + 8);

            File.WriteAllText("Shaders\\shader.frag", cleanedText);

            Debug.WriteLine(cleanedText.TrimStart());

        }

    }
}