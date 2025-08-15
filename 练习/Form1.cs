using System.ComponentModel;

namespace 练习;

public partial class Form1 : Form
{
    public Form1() => InitializeComponent();
    
    //写一个交互逻
    public Form1(IContainer components)
    {
        this.components = components;
        
    }
}