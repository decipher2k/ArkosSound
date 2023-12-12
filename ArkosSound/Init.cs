using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArkosSound
{
    public partial class Init : Form
    {
        public List<int> ret=new List<int>();
        public Init()
        {
            InitializeComponent();
        }

        private void Init_Load(object sender, EventArgs e)
        {

        }

        public int convertToPitch(String note)
        {
            String sym = "";
            int oct = 0;
            String[][] notes = { new string[] { "C" }, new string[] { "Db", "C#" }, new string[] { "D" }, new string[] { "Eb", "D#" }, new string[] { "E" }, new string[] { "F" }, new string[] { "Gb", "F#" }, new string[] { "G" }, new string[] { "Ab", "G#" }, new string[] { "A" }, new string[] { "Bb", "A#" }, new string[] { "B" } };

            char[] splitNote = note.ToCharArray();

            // If the length is two, then grab the symbol and number.
            // Otherwise, it must be a two-char note.
            if (splitNote.Count() == 2)
            {
                sym += splitNote[0];
                oct = splitNote[1] + 1;
            }
            else if (splitNote.Count() == 3)
            {
                sym += char.ToString(splitNote[0]);
                sym += char.ToString(splitNote[1]);
                oct = splitNote[2] + 1;
            }

            // Find the corresponding note in the array.
            for (int i = 0; i < notes.Count(); i++)
                for (int j = 0; j < notes[i].Count(); j++)
                {
                    if (notes[i][j].Equals(sym))
                    {
                        return (int)(char.GetNumericValue((char)oct) * 12 + i);
                    }
                }

            // If nothing was found, we return -1.
            return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length != 0)
            {
                int note=convertToPitch(textBox1.Text);
                if (note != -1)
                {
                    ret.Add(note);                    
                }
                else
                {
                    MessageBox.Show("Invalid note: 1");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid note: 1");
                return;
            }
            if (textBox2.Text.Length != 0)
            {
                int note = convertToPitch(textBox1.Text);
                if (note != -1)
                {
                    ret.Add(note);
                }
                else
                {
                    MessageBox.Show("Invalid note: 2");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid note: 1");
                return;
            }
            if (textBox3.Text.Length != 0)
            {
                int note = convertToPitch(textBox1.Text);
                if (note != -1)
                {
                    ret.Add(note);
                }
                else
                {
                    MessageBox.Show("Invalid note: 3");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid note: 1");
                return;
            }
            if (textBox4.Text.Length != 0)
            {
                int note = convertToPitch(textBox1.Text);
                if (note != -1)
                {
                    ret.Add(note);
                }
                else
                {
                    MessageBox.Show("Invalid note: 4");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid note: 1");
                return;
            }
            if (textBox5.Text.Length != 0)
            {
                int note = convertToPitch(textBox1.Text);
                if (note != -1)
                {
                    ret.Add(note);
                }
                else
                {
                    MessageBox.Show("Invalid note: 5");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid note: 1");
                return;
            }
            if (textBox6.Text.Length != 0)
            {
                int note = convertToPitch(textBox1.Text);
                if (note != -1)
                {
                    ret.Add(note);
                }
                else
                {
                    MessageBox.Show("Invalid note: 6");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid note: 1");
                return;
            }
            if (textBox7.Text.Length != 0)
            {
                int note = convertToPitch(textBox1.Text);
                if (note != -1)
                {
                    ret.Add(note);
                }
                else
                {
                    MessageBox.Show("Invalid note: 7");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid note: 1");
                return;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
