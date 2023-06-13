/*
 
    Praktikum 2.4
    Author: JOhannes Harnisch
    Erstellt: 12.06.2023
    control Module of the wpf 
    starts the instance
    contains file interaction methods
    


*/


using Praktikum_2._3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Praktikum2._4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //initialize of the system
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                read();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler");
            }
        }


        //event handler
        /// <summary>
        /// writs slider value in label and starts calculation new when new value selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slFrequency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            lbFreq.Content = (slFrequency.Value)/10;

            BerechneParallelZweiPol();

        }

        //also file interaction
        /// <summary>
        /// saves actual dataset to file at click of button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string bau = "Parallel";

            bool saved = false;
            string path = @"..\..\..\txt.txt";
            FileStream fs;
            using (new FileStream(path, FileMode.OpenOrCreate)) ;

            fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("{0},{1},{2},{3}", tbWid.Text, tbCap.Text, slFrequency.Value, bau);
                sw.Flush();
                cbPreset.Items.Add(string.Format("{0},{1},{2},{3}", tbWid.Text, tbCap.Text, slFrequency.Value, bau));
            }
            return;
        }

        /// <summary>
        /// starts calculation new when textbox content is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbWid_TextChanged(object sender, TextChangedEventArgs e)
        {
            BerechneParallelZweiPol();

        }
        /// <summary>
        /// starts calculation new when textbox content is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCap_TextChanged(object sender, TextChangedEventArgs e)
        {
            BerechneParallelZweiPol();
        }


        //funktion Code
        /// <summary>
        /// calculates Betrag and dthe komplex value
        /// </summary>
        private void BerechneParallelZweiPol()
        {
            double r;
            double f;
            double c;
            string tmp;
            bool tmp1;
            RCZweipolParallel zppObjekt;
            string bauform = "placeholder";


            double betrag;
            double im, re, kom;


            if (tbKomplex == null)//only activates if window is not initialized
            {
                return;
            }


            try
            {
                tmp = tbWid.Text;
                tmp1 = double.TryParse(tmp, out r);
                if (r < 0)
                {
                    tbBetrag.Text = "";
                    tbKomplex.Text = "";
                    throw new ArgumentOutOfRangeException("Fehler! Der Widerstand muss eine positive Zahl sein!");

                }
                else if (!tmp1)
                {
                    tbBetrag.Text = "";
                    tbKomplex.Text = "";
                    throw new ArgumentException("Fehler! Der Widerstand muss eine Zahl sein!");

                }

                tmp = tbCap.Text;
                tmp1 = double.TryParse(tmp, out c);
                if (c < 0)
                {
                    tbBetrag.Text = "";
                    tbKomplex.Text = "";
                    throw new ArgumentOutOfRangeException("Fehler! Die Frequenz muss eine positive Zahl sein!");

                }
                else if (!tmp1)
                {
                    tbBetrag.Text = "";
                    tbKomplex.Text = "";
                    throw new ArgumentException("Fehler! Die Frequenz muss eine Zahl sein!");

                }

                
                f = (slFrequency.Value)/10;




                c = c * 1E-6;


                if (f == 0 || c == 0 || r == 0)
                {
                    //in diesem fall hat nur r einfluss c = 0
                    tbBetrag.Text = r.ToString();
                    tbKomplex.Text = r.ToString();
                    return;
                }
            }
            
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"Fehler");
                return;
            }

            zppObjekt = new RCZweipolParallel(r, c, bauform, f);

            betrag = zppObjekt.GetZBetrag();
            re = zppObjekt.GetZReal();
            im = Math.Abs(zppObjekt.GetZImag());

            
            tbBetrag.Text = string.Format("{0:F3}", betrag);
            tbKomplex.Text = string.Format("{0:F3} -j{1:F3}", re, im);


            return;


        }
        private void BerechneReihenZweiPol()
        {
            //like Paralllel Zweipol but different
        }

        /// <summary>
        /// decides which berechnen method to use currently unused
        /// </summary>
        private void BerechneZweipol()
        {
            switch (cbType.Text)
            {
                case "RC-Reihen-Zweipol":

                    BerechneReihenZweiPol();
                    break;
                case "RC-Parallel-Zweipol":
                    BerechneParallelZweiPol();
                    break;
            }

        }

        //file interaction methods
        /// <summary>
        /// reads saved entrys from defined files 
        /// </summary>
        private void read()
        {
            string zeile;
            string path = @"..\..\..\txt.txt";
            FileStream fs;
            fs = new FileStream(path, FileMode.OpenOrCreate);


            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    zeile = sr.ReadLine();
                    cbPreset.Items.Add(zeile);

                }

            }
        }

        
    }
}
