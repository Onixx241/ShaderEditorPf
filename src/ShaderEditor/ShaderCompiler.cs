using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class ShaderCompiler
{
    //this is moreso a pointer for future saving/loading feature. 
    public string? CurrentShader { get; set; }
    public string? ShaderPath { get; set; }

    int ShaderProg { get; set; }

    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;

    float[] vertices =
    {
        1.0f,1.0f,0.0f,
        1.0f,-1.0f,0.0f,
        -1.0f,-1.0f,0.0f,
        -1.0f,1.0f,0.0f
    };

    uint[] indices =
    {
        0,1,2,
        0,2,3
    };

    public ShaderCompiler() 
    { }


    //maybe change from init it might need to start over and over every keypress.
    public void ReCompileShader() 
    {
        
        string? shaderSource = this.CurrentShader;
        string? vertSource = File.ReadAllText("Shaders\\Do not touch\\shader.vert");

        int Shader = GL.CreateShader(ShaderType.FragmentShader);
        int vertShader = GL.CreateShader(ShaderType.VertexShader);

        GL.ShaderSource(Shader, File.ReadAllText(this.ShaderPath));
        GL.ShaderSource(vertShader, vertSource);

        //Maybe do -> if error is returned then make screen red or something. default to all red shader.
        GL.CompileShader(Shader);
        GL.GetShader(Shader, ShaderParameter.CompileStatus, out int success2);
        if (success2 == 0)
        {
            string log = GL.GetShaderInfoLog(Shader);
            Debug.WriteLine($"Fragment shader compile error: {log}");
        }

        GL.CompileShader(vertShader);
        GL.GetShader(vertShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string log = GL.GetShaderInfoLog(vertShader);
            Debug.WriteLine($"Vertex shader compile error: {log}");
        }
        this.ShaderProg = GL.CreateProgram();

        

        GL.AttachShader(ShaderProg, Shader);
        GL.AttachShader(ShaderProg, vertShader);

        GL.LinkProgram(ShaderProg);
        GL.GetProgram(ShaderProg, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            string log = GL.GetProgramInfoLog(ShaderProg);
            Debug.WriteLine($"Program link error: {log}");
        }

        GL.UseProgram(this.ShaderProg);

    }

    public void SetupScreen() 
    {
        this.VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(this.VertexArrayObject);

        this.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);

        this.ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);

        //this still works because once you use bufferdata the data is in your gpu
        this.VertexBufferObject = 0;

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);

        this.ReCompileShader();

        GL.DrawElements(PrimitiveType.Triangles, this.indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}