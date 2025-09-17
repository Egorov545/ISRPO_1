using System;
using System.Data;
using System.Windows;

namespace ISRPO_1
{
    public partial class MainWindow : Window
    {
        private double[,] _matrix;

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

            // Создаём матрицу со случайными числами, округлёнными до десятых
            _matrix = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    // 20% шанс поставить 0.0
                    if (rnd.NextDouble() < 0.2)
                    {
                        _matrix[i, j] = 0.0;
                    }
                    else
                    {
                        // Генерируем число от -5.0 до 5.0 и округляем до десятых
                        _matrix[i, j] = Math.Round(rnd.NextDouble() * 10 - 5, 1);
                    }
                }
            }

            // Привязываем к DataGrid через DataTable
            DgMatrix.ItemsSource = _matrix.ToDataTableTwo().DefaultView;
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (_matrix == null)
            {
                MessageBox.Show("Сначала создайте матрицу.");
                return;
            }

            int n = _matrix.GetLength(0);
            int m = _matrix.GetLength(1);

            // Обновляем матрицу из DataGrid (на случай, если пользователь что-то изменил)
            var view = (DataView)DgMatrix.ItemsSource;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    var cellValue = view[i][j];
                    if (cellValue != DBNull.Value && double.TryParse(cellValue.ToString(), out double val))
                    {
                        _matrix[i, j] = val;
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка в ячейке [{i + 1}, {j + 1}]. Введите корректное число.");
                        return;
                    }
                }
            }

            // Вычисляем сумму столбцов, содержащих хотя бы один ноль
            double totalSum = 0;

            for (int j = 0; j < m; j++)
            {
                bool hasZero = false;
                double columnSum = 0;

                for (int i = 0; i < n; i++)
                {
                    columnSum += _matrix[i, j];
                    if (_matrix[i, j] == 0)
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