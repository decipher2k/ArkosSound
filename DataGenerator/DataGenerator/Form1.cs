using MidiSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int convertToPitch(String note)
        {
            String sym = "";
            int oct = 0;
            String[][] notes ={new string[] {"C"}, new string[] { "Db", "C#"}, new string[] { "D"}, new string[] { "Eb", "D#"}, new string[] { "E"}, new string[] { "F"}, new string[] { "Gb", "F#"}, new string[] { "G"}, new string[] { "Ab", "G#"}, new string[] { "A"}, new string[] { "Bb", "A#"}, new string[] { "B"} };

            char[] splitNote = note.ToCharArray();

            // If the length is two, then grab the symbol and number.
            // Otherwise, it must be a two-char note.
            if (splitNote.Count() == 2)
            {
                sym += splitNote[0];
                oct = splitNote[1]+1;
            }
            else if (splitNote.Count() == 3)
            {
                sym += char.ToString(splitNote[0]);
                sym += char.ToString(splitNote[1]);
                oct = splitNote[2]+1;
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
            String outX = "";
            string outY = "";

            foreach (String fileName in Directory.EnumerateFiles("d:\\Downloads\\", "*.mid"))
            {
                FileStream file = File.OpenRead(fileName);
                MidiSequence midi=MidiSequence.Open(file);
                try
                {
                    MidiTrack track0 = midi.ElementAt(1);
                    int trackLength = track0.Count();
                    if (trackLength > 16)
                    {
                        for (int i = 0; i < 7; i++)
                        {

                            int start = new Random((int)DateTime.Now.Millisecond).Next(trackLength - 16);
                            int[] sequence = new int[8];
                            int noteCount = 0;
                            int count = 0;
                            while (noteCount < 8)
                            {
                                String token = track0.ElementAt(start + count).ToString();

                                String note = token.Split('\t')[3];
                                int iNote = convertToPitch(note);
                                if (iNote > -1)
                                {
                                    sequence[noteCount] = iNote;
                                    noteCount++;
                                }
                                count++;
                            }
                            for (int z = 0; z < 7; z++)
                            {
                                outX += sequence[z].ToString() + "\r\n";
                            }
                            if (sequence[7] > sequence[6])
                                outX += "10000\r\n";
                            else
                                outX += "-10000\r\n";
                            outY += sequence[7].ToString() + "\r\n";
                            System.Threading.Thread.Sleep(new Random((int)DateTime.Now.Millisecond).Next(new Random((int)DateTime.Now.Millisecond / 10).Next(100)));
                        }
                    }
                }catch (Exception ex) { }
            }
            File.WriteAllText("d:\\Downloads\\outX.txt", outX);
            File.WriteAllText("d:\\Downloads\\outY.txt", outY);
        }
    }
}
