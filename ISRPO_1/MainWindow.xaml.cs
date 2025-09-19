using System;
using System.Data;
using System.Windows;

namespace ISRPO_1
{
    public partial class MainWindow : Window
    {
        private double[,] matrix;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnCreateMatrix_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            if (!int.TryParse(TbRows.Text, out int n) || !int.TryParse(TbCols.Text, out int m) || n <= 0 || m <= 0)
            {
                MessageBox.Show("Введите корректные положительные значения для n и m.");
                return;
            }

            matrix = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (rnd.NextDouble() < 0.2)
                    {
                        matrix[i, j] = 0.0;
                    }
                    else
                    {
                        matrix[i, j] = Math.Round(rnd.NextDouble() * 10, 1);
                    }
                }
            }
            DgMatrix.ItemsSource = matrix.ToDataTableTwo().DefaultView;
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Сначала создайте матрицу.");
                return;
            }

            double totalSum = 0;

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                bool hasZero = false;
                double columnSum = 0;

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    columnSum += matrix[i, j];
                    if (matrix[i, j] == 0)
                        hasZero = true;
                }

                if (hasZero)
                {
                    totalSum += columnSum;
                }
            }

            TbResult.Text = $"Общая сумма элементов столбцов, содержащих хотя бы один ноль: {totalSum}";
        }
    }
}