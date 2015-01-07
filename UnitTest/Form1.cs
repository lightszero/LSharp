using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class Form1 : Form, CLRSharp.ICLRSharp_Logger
    {
        public static Form1 g_this;
        public Form1()
        {
            InitializeComponent();
            g_this = this;
        }
        CLRSharp.CLRSharp_Environment env;
        private void Form1_Load(object sender, EventArgs e)
        {
            var bytes = System.IO.File.ReadAllBytes("UnitTestDll.dll");
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);

            env = new CLRSharp.CLRSharp_Environment();
            env.LoadModule(ms);
            var types = env.GetAllTypes();
            foreach (var t in types)
            {
                if (env.GetType(t) != null && env.GetType(t).type_CLRSharp != null && env.GetType(t).type_CLRSharp.HasMethods)
                {
                    TreeNode node = new TreeNode(t);
                    treeView1.Nodes.Add(node);
                    foreach (var m in env.GetType(t).type_CLRSharp.Methods)
                    {
                        TreeNode method = new TreeNode(m.Name);
                        method.Tag = m;
                        node.Nodes.Add(method);
                        if (m.HasParameters == false && m.Name.Contains("UnitTest") && m.IsStatic)
                        {
                            method.BackColor = Color.Green;
                        }
                    }

                }
            }
            treeView1.ExpandAll();
        }

        public void Log(string str)
        {
            this.listBox1.Items.Add(str);
        }

        public void Log_Warning(string str)
        {
            Log("<W>" + str);
        }

        public void Log_Error(string str)
        {
            Log("<Err>" + str);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Mono.Cecil.MethodDefinition)
            {

                FillTree_Method(e.Node.Tag as Mono.Cecil.MethodDefinition);
            }
            else
            {
                treeView2.Nodes.Clear();
                treeView2.Tag = null;
            }
        }
        void FillTree_Method(Mono.Cecil.MethodDefinition method)
        {
            treeView2.Tag = method;
            treeView2.Nodes.Clear();
            TreeNode methodinfo = new TreeNode(method.ToString());
            treeView2.Nodes.Add(methodinfo);

            if (method.HasParameters)
            {
                TreeNode paraminfo = new TreeNode("Params");
                treeView2.Nodes.Add(paraminfo);
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    TreeNode p = new TreeNode(method.Parameters[i].ParameterType + " " + method.Parameters[i].Name);
                    paraminfo.Nodes.Add(p);
                }
            }
            TreeNode body = new TreeNode("Body");
            treeView2.Nodes.Add(body);
            if (method.HasBody)
            {
                TreeNode variables = new TreeNode("Variables");
                body.Nodes.Add(variables);
                if (method.Body.HasVariables)
                {
                    foreach (var v in method.Body.Variables)
                    {
                        TreeNode var = new TreeNode(v.VariableType + " " + v.Name);
                        variables.Nodes.Add(var);
                    }
                }
                TreeNode eh = new TreeNode("ExceptionHandlers");
                body.Nodes.Add(eh);
                if (method.Body.HasExceptionHandlers)
                {
                    foreach (var v in method.Body.ExceptionHandlers)
                    {
                        TreeNode var = new TreeNode(v.ToString());
                        variables.Nodes.Add(var);
                    }
                }
                TreeNode code = new TreeNode("Code");
                body.Nodes.Add(code);
                foreach (var i in method.Body.Instructions)
                {
                    TreeNode op = new TreeNode(i.ToString());
                    code.Nodes.Add(op);
                }
            }
            treeView2.ExpandAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mono.Cecil.MethodDefinition d = this.treeView2.Tag as Mono.Cecil.MethodDefinition;
            if (d == null) return;
            var type = env.GetType(d.DeclaringType.FullName);
            var method = type.GetMethod(d.Name, null);
            CLRSharp.Context context = new CLRSharp.Context(env);
            try
            {
                method.Invoke(context, null, null);
                Log("----RunOK----");
            }
            catch (Exception err)
            {
                Log("----RunErr----");
                Log_Error(err.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
