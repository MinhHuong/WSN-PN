using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PAT.GUI.SVModule
{
    public partial class RandomForm : Form
    {
        public event EventHandler ButtonGetNumSenLinkClicked;
        public RandomForm()
        {
            InitializeComponent();
        }

        //public void getsenlink(ref int sensor, ref int link)
        //{
        //    sensor = int.Parse(textBox1.Text);
        //    link = int.Parse(textBox2.Text);
        //    Console.WriteLine(sensor);
        //    Console.WriteLine(link);
        //}

        public void button1_Click(object sender, EventArgs e)
        {
            if (ButtonGetNumSenLinkClicked != null)
                if ((textBox1.Text != "") && (textBox2.Text != ""))
                {
                    ButtonGetNumSenLinkClicked(sender, e);
                    this.Close();
                }
                else
                {
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("Number sensor is not null", "Error number sensor");
                        textBox1.Focus();
                    }
                    if (textBox2.Text == "")
                    {
                        MessageBox.Show("Number link is not null", "Error number link");
                        textBox2.Focus();
                    }
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ButtonGetNumSenLinkClicked != null)

                if (e.KeyChar == 13)
                {
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("Please input value sensor", "Number sensor invalid");
                        textBox1.Focus();
                    }
                    if (textBox2.Text == "")
                    {
                        MessageBox.Show("Please input value channel", "Number channel invalid");
                        textBox2.Focus();
                    }
                    if ((textBox1.Text != "") && (textBox2.Text != ""))
                    {
                        //this.Close();
                        ButtonGetNumSenLinkClicked(sender, e);
                        this.Close();
                    }
                }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ButtonGetNumSenLinkClicked != null)

                if (e.KeyChar == 13)
                {
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("Please input value sensor", "Number sensor invalid");
                        textBox1.Focus();
                    }
                    if (textBox2.Text == "")
                    {
                        MessageBox.Show("Please input value channel", "Number channel invalid");
                        textBox2.Focus();
                    }
                    if ((textBox1.Text != "") && (textBox2.Text != ""))
                    {
                        //this.Close();
                        ButtonGetNumSenLinkClicked(sender, e);
                        this.Close();
                    }
                }
        }
    }
}
