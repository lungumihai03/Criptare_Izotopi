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

namespace Criptare_Izotopi
{
    public partial class Form1 : Form
    {
        string mesaj;
        int nivel;

        Dictionary<char, List<int>> dictionar_cript = new Dictionary<char, List<int>>();
        Dictionary<char, List<int>> dictionar_decript = new Dictionary<char, List<int>>();

        public Form1()
        {
            InitializeComponent();
            //nivel = (int)numericUpDown1.Value; // Initializeaza nivel cu valoarea default
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            nivel = (int)numericUpDown1.Value;
        }

        private void gen_alfabet(object sender, EventArgs e)
        {
            mesaj = richTextBox1.Text;
            
            Random rand = new Random();
            HashSet<int> numereGlobale = new HashSet<int>();

            foreach (char c in mesaj)
            {
                if (!dictionar_cript.ContainsKey(c))
                {
                    List<int> numere = new List<int>();
                    while (numere.Count < nivel)
                    {
                        int numar = rand.Next(99, 999);
                        if (!numere.Contains(numar) && !numereGlobale.Contains(numar))
                        {
                            numere.Add(numar);
                            numereGlobale.Add(numar);
                        }
                    }
                    dictionar_cript[c] = numere;
                }
            }
        }

        private void save_dictionar(object sender, EventArgs e)
        {
            if (dictionar_cript.Count == 0)
            {
                gen_alfabet(sender, e);
            }
            //mai intai sterge fisierul daca exista
            if (File.Exists("dictionar.txt"))
            {
                File.Delete("dictionar.txt");
            }
            StringBuilder sb = new StringBuilder();
            foreach (var pereche in dictionar_cript)
            {
                sb.Append(pereche.Key + ": ");
                sb.Append(string.Join(", ", pereche.Value));
                sb.AppendLine();
            }
            System.IO.File.WriteAllText("dictionar.txt", sb.ToString());
            //MessageBox.Show("Dictionarul a fost salvat in fisierul 'dictionar.txt'");
            
        }

        private void save_dictionar_formatat(object sender, EventArgs e)
        {
            
                gen_alfabet(sender, e);
            
            StringBuilder sb = new StringBuilder();
            var caractere = dictionar_cript.Keys.OrderBy(c => c).ToList();
            int nivelMax = dictionar_cript.Values.Max(list => list.Count);

            sb.AppendLine(string.Join("\t", caractere));

            for (int i = 0; i < nivelMax; i++)
            {
                List<string> linie = new List<string>();
                foreach (var c in caractere)
                {
                    var lista = dictionar_cript[c];
                    if (i < lista.Count)
                        linie.Add(lista[i].ToString());
                    else
                        linie.Add("");
                }
                sb.AppendLine(string.Join("\t", linie));
            }

            System.IO.File.WriteAllText("dictionar_formatat.txt", sb.ToString());
            MessageBox.Show("Dictionarul formatat a fost salvat in fisierul 'dictionar_formatat.txt'");
            System.Diagnostics.Process.Start("notepad.exe", "dictionar_formatat.txt");
        }

        private void load_dictionar_decript(object sender, EventArgs e)
        {
            dictionar_decript.Clear();
            string[] linii = System.IO.File.ReadAllLines("dictionar.txt");
            foreach (string linie in linii)
            {
                string[] parti = linie.Split(new string[] { ": " }, StringSplitOptions.None);
                if (parti.Length == 2)
                {
                    char caracter = parti[0][0];
                    List<int> numere = parti[1].Split(new string[] { ", " }, StringSplitOptions.None).Select(int.Parse).ToList();
                    dictionar_decript[caracter] = numere;
                }
            }
            MessageBox.Show("Dictionarul decriptat a fost incarcat din fisierul 'dictionar.txt'");
        }

        private int[,] AplicaPermutarePeLinii(int[,] matrice, int[] permutare)
        {
            int n = matrice.GetLength(0);
            int[,] rezultat = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                int linieSursa = permutare[i] - 1; 
                for (int j = 0; j < n; j++)
                {
                    rezultat[i, j] = matrice[linieSursa, j];
                }
            }
            return rezultat;
        }

        private int[,] AplicaPermutarePeColoane(int[,] matrice, int[] permutare)
        {
            int n = matrice.GetLength(0);
            int[,] rezultat = new int[n, n];

            for (int j = 0; j < n; j++)
            {
                int coloanaSursa = permutare[j] - 1; 
                for (int i = 0; i < n; i++)
                {
                    rezultat[i, j] = matrice[i, coloanaSursa];
                }
            }
            return rezultat;
        }

        private int[] CalculeazaPermutareaInversa(int[] permutare)
        {
            int n = permutare.Length;
            int[] inversa = new int[n];

            for (int i = 0; i < n; i++)
            {
                inversa[permutare[i] - 1] = i + 1;
            }
            return inversa;
        }

        private void SalveazaMatriceInFisier(int[,] matrice, string numeFisier, string descriere)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(descriere);
            sb.AppendLine(new string('-', 50));

            int n = matrice.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sb.Append(matrice[i, j].ToString("D3") + " ");
                }
                sb.AppendLine();
            }
            sb.AppendLine();

            File.AppendAllText(numeFisier, sb.ToString());
        }

        private void criptare_mesaj(object sender, EventArgs e)
        {
            
                gen_alfabet(sender, e);
                save_dictionar(sender, e);
            

            List<int> codCifrat = new List<int>();
            Random rand = new Random();
            foreach (char c in mesaj)
            {
                if (dictionar_cript.ContainsKey(c))
                {
                    List<int> optiuni = dictionar_cript[c];
                    int index = rand.Next(optiuni.Count);
                    codCifrat.Add(optiuni[index]);
                }
            }

            int n = (int)Math.Ceiling(Math.Sqrt(codCifrat.Count));
            string input = textBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Introdu numere separate prin spațiu în textBox2!");
                return;
            }

            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != n)
            {
                MessageBox.Show($"Trebuie să introduci exact {n} numere pentru permutare!");
                return;
            }

            int[] permutare = parts.Select(int.Parse).ToArray();

            int[,] alfa = new int[2, n];
            for (int i = 0; i < n; i++)
            {
                alfa[0, i] = i + 1;
                alfa[1, i] = permutare[i];
            }

            string pathAlfa = Path.Combine(Application.StartupPath, "alfa.txt");
            using (StreamWriter sw = new StreamWriter(pathAlfa))
            {
                sw.WriteLine("Matricea ALFA:");
                for (int i = 0; i < n; i++)
                    sw.Write(alfa[0, i] + "\t");
                sw.WriteLine();
                for (int i = 0; i < n; i++)
                    sw.Write(alfa[1, i] + "\t");
                sw.WriteLine();
            }

            int[,] matrice = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int index = i * n + j;
                    if (index < codCifrat.Count)
                    {
                        matrice[i, j] = codCifrat[index];
                    }
                    else
                    {
                        matrice[i, j] = 0;
                    }
                }
            }

            string pathCriptare = Path.Combine(Application.StartupPath, "criptare_detaliat.txt");
            File.WriteAllText(pathCriptare, "PROCESUL DE CRIPTARE\n");

            SalveazaMatriceInFisier(matrice, pathCriptare, "Matricea Initiala (dupa codificare):");

            int[,] matriceDupaLinii = AplicaPermutarePeLinii(matrice, permutare);
            SalveazaMatriceInFisier(matriceDupaLinii, pathCriptare,
                "Matricea dupa permutarea pe LINII conform ALFA:");

            int[,] matriceFinala = AplicaPermutarePeColoane(matriceDupaLinii, permutare);
            SalveazaMatriceInFisier(matriceFinala, pathCriptare,
                "Matricea FINALA dupa permutarea pe COLOANE:");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sb.Append(matriceFinala[i, j].ToString("D3") + " ");
                }
                sb.AppendLine();
            }
            richTextBox2.Text = sb.ToString();

            File.AppendAllText(pathCriptare, "\nPermutarea ALFA folosita:\n");
            File.AppendAllText(pathCriptare, "Linia 1: " + string.Join(" ", Enumerable.Range(1, n)) + "\n");
            File.AppendAllText(pathCriptare, "Linia 2: " + string.Join(" ", permutare) + "\n");

            MessageBox.Show("Criptarea a fost realizata! Detalii salvate in 'criptare_detaliat.txt'");
            System.Diagnostics.Process.Start("notepad.exe", "dictionar.txt");
            System.Diagnostics.Process.Start("notepad.exe", pathCriptare);
        }

        private void decriptare_mesaj(object sender, EventArgs e)
        {
            if (dictionar_decript.Count == 0)
            {
                load_dictionar_decript(sender, e);
            }

            string[] linii = richTextBox2.Text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            int n = linii.Length;
            int[,] matriceCriptata = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                string[] numereStr = linii[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < n && j < numereStr.Length; j++)
                {
                    if (int.TryParse(numereStr[j], out int numar))
                    {
                        matriceCriptata[i, j] = numar;
                    }
                }
            }

            string inputAlfa = textBox3.Text.Trim();
            if (string.IsNullOrWhiteSpace(inputAlfa))
            {
                MessageBox.Show("Introdu permutarea ALFA în TextBox3!");
                return;
            }

            string[] partsAlfa = inputAlfa.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (partsAlfa.Length != n)
            {
                MessageBox.Show($"Permutarea ALFA trebuie să aibă exact {n} numere!");
                return;
            }

            int[] permutareAlfa = partsAlfa.Select(int.Parse).ToArray();

            int[] permutareInversa = CalculeazaPermutareaInversa(permutareAlfa);

            string pathDecriptare = Path.Combine(Application.StartupPath, "decriptare_detaliat.txt");
            File.WriteAllText(pathDecriptare, "PROCESUL DE DECRIPTARE\n");

            File.AppendAllText(pathDecriptare, "Permutarea ALFA originala:\n");
            File.AppendAllText(pathDecriptare, "Linia 1: " + string.Join(" ", Enumerable.Range(1, n)) + "\n");
            File.AppendAllText(pathDecriptare, "Linia 2: " + string.Join(" ", permutareAlfa) + "\n\n");

            File.AppendAllText(pathDecriptare, "Permutarea ALFA^(-1) (inversa):\n");
            File.AppendAllText(pathDecriptare, "Linia 1: " + string.Join(" ", Enumerable.Range(1, n)) + "\n");
            File.AppendAllText(pathDecriptare, "Linia 2: " + string.Join(" ", permutareInversa) + "\n\n");

            SalveazaMatriceInFisier(matriceCriptata, pathDecriptare, "Matricea CRIPTATA primita:");

            int[,] matriceDupaColoane = AplicaPermutarePeColoane(matriceCriptata, permutareInversa);
            SalveazaMatriceInFisier(matriceDupaColoane, pathDecriptare,
                "Matricea dupa aplicarea ALFA^(-1) pe COLOANE:");

            int[,] matriceDecriptata = AplicaPermutarePeLinii(matriceDupaColoane, permutareInversa);
            SalveazaMatriceInFisier(matriceDecriptata, pathDecriptare,
                "Matricea FINALA DECRIPTATA dupa aplicarea ALFA^(-1) pe LINII:");

            List<int> coduriDecriptate = new List<int>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matriceDecriptata[i, j] != 0)
                    {
                        coduriDecriptate.Add(matriceDecriptata[i, j]);
                    }
                }
            }

            StringBuilder mesajDecriptat = new StringBuilder();
            foreach (int numar in coduriDecriptate)
            {
                foreach (var pereche in dictionar_decript)
                {
                    if (pereche.Value.Contains(numar))
                    {
                        mesajDecriptat.Append(pereche.Key);
                        break;
                    }
                }
            }

            File.AppendAllText(pathDecriptare, "MESAJUL DECRIPTAT:\n");
            File.AppendAllText(pathDecriptare, mesajDecriptat.ToString() + "\n");

            MessageBox.Show("Decriptarea a fost realizata! Detalii salvate in 'decriptare_detaliat.txt'");
            System.Diagnostics.Process.Start("notepad.exe", pathDecriptare);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear all dictionaries and text boxes
            dictionar_cript.Clear();
            dictionar_decript.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
            textBox2.Clear();
            textBox3.Clear();

        }
    }
}