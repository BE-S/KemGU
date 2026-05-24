using System;
using System.Text;

namespace ConsoleApp2.thirdTaskClass
{
    interface IPrototype<T>
    {
        T Clone();
    }

    class SquareMatrix : IComparable<SquareMatrix>, IPrototype<SquareMatrix>

    {
        private double[,] Matrix { get; }
        private int Size { get; }

        private static readonly Random _random = new Random();

        public SquareMatrix(int size)
        {
            ValidateSize(size);

            Size = size;

            Matrix = new double[Size, Size];

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    Matrix[i, j] = _random.Next(0, 6);
                }
            }
        }

        public SquareMatrix(double[,] matrix) : this(GetMatrixSize(matrix), matrix) {}

        protected SquareMatrix(int size, double[,] matrix)
        {
            ValidatePositiveSize(size);
            ValidateMatrix(size, matrix);

            Size = size;

            Matrix = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Matrix[i, j] = matrix[i, j];
                }
            }
        }

        public SquareMatrix Clone()
        {
            return new SquareMatrix(Size, Matrix);
        }

        public static SquareMatrix operator +(SquareMatrix first, SquareMatrix second)
        {
            ValidateNotNull(first, nameof(first));
            ValidateNotNull(second, nameof(second));

            if (first.Size != second.Size)
            {
                throw new MatrixDimensionException("Сложение невозможно. Матрицы имеют разный размер.");
            }

            double[,] matrix = new double[first.Size, first.Size];

            for (int i = 0; i < first.Size; ++i)
            {
                for (int j = 0; j < first.Size; ++j)
                {
                    matrix[i, j] = first.Matrix[i, j] + second.Matrix[i, j];
                }
            }

            return new SquareMatrix(first.Size, matrix);
        }

        public static SquareMatrix operator *(SquareMatrix first, SquareMatrix second)
        {
            ValidateNotNull(first, nameof(first));
            ValidateNotNull(second, nameof(second));

            if (first.Size != second.Size)
            {
                throw new MatrixDimensionException("Умножение невозможно. Матрицы имеют разный размер.");
            }

            double[,] matrix = new double[first.Size, first.Size];

            for (int i = 0; i < first.Size; ++i)
            {
                for (int temp = 0; temp < second.Size; ++temp)
                {
                    for (int j = 0; j < second.Size; ++j)
                    {
                        matrix[i, temp] += first.Matrix[i, j] * second.Matrix[j, temp];
                    }
                }
            }

            return new SquareMatrix(first.Size, matrix);
        }

        public static double CalculateNorm(SquareMatrix matrix)
        {
            ValidateNotNull(matrix, nameof(matrix));

            double normaOfMatrix = 0;

            for (int i = 0; i < matrix.Size; ++i)
            {
                for (int j = 0; j < matrix.Size; ++j)
                {
                    normaOfMatrix += matrix.Matrix[i, j] * matrix.Matrix[i, j];
                }
            }

            return Math.Sqrt(normaOfMatrix);
        }
        
        public double Determinant()
        {
            SquareMatrix resultMatrix = new SquareMatrix(Size, Matrix);
            double[] tempDiagonal = new double[Size];

            for (int diagonal = 0; diagonal < resultMatrix.Size - 1; ++diagonal)
            {
                int currentLine = diagonal + 1;

                bool diagonalHasNull = false;

                for (int j = diagonal, shiftDiagonal = 1; j < resultMatrix.Size; ++j)
                {
                    tempDiagonal[j] = resultMatrix.Matrix[diagonal, j];

                    if ((j == diagonal && resultMatrix.Matrix[diagonal, j] == 0) || diagonalHasNull)
                    {
                        while (j == 0 && (diagonal + shiftDiagonal) < resultMatrix.Size && resultMatrix.Matrix[diagonal + shiftDiagonal, j] == 0)
                        {
                            ++shiftDiagonal;
                        }

                        tempDiagonal[j] = resultMatrix.Matrix[diagonal, j] = resultMatrix.Matrix[diagonal + shiftDiagonal, j] + tempDiagonal[j];

                        resultMatrix.Matrix[diagonal + shiftDiagonal, j] = resultMatrix.Matrix[diagonal + shiftDiagonal, j] - resultMatrix.Matrix[diagonal, j];

                        diagonalHasNull = true;
                    }
                }

                if (diagonalHasNull)
                {
                    ++currentLine;
                }

                for (int i = currentLine; i < Size; ++i)
                {
                    double factor = 0;

                    if (tempDiagonal[diagonal] != 0)
                    {
                        factor = resultMatrix.Matrix[i, diagonal] / tempDiagonal[diagonal];
                    }

                    for (int j = diagonal; j < resultMatrix.Size; ++j)
                    {
                        resultMatrix.Matrix[i, j] = resultMatrix.Matrix[i, j] - (tempDiagonal[j] * factor);
                    }
                }
            }

            double result = resultMatrix.Matrix[0, 0];

            for (int i = 1; i < Size; ++i)
            {
                for (int j = i; j == i; ++j)
                {
                    result *= resultMatrix.Matrix[i, j];
                }
            }

            return result;
        }

        public SquareMatrix InverseMatrix()
        {
            double det = Determinant();

            if (det == 0)
            {
                throw new SingularMatrixException("Матрица является вырожденной. Обратную матрицу найти невозможно.");
            }

            double[,] inverse = new double[Size, Size];

            for (int diagonal = 0; diagonal < Size; ++diagonal)
            {
                for (int temp = 0; temp < Size; ++temp)
                {
                    double[,] matrixForAlgebraicComplement = new double[Size - 1, Size - 1];


                    for (int i = 0, i2 = 0; i < Size; ++i)
                    {
                        if (i == diagonal)
                        {
                            continue;
                        }

                        for (int j = 0, j2 = 0; j < Size; ++j)
                        {
                            if (j == temp)
                            {
                                continue;
                            }

                            matrixForAlgebraicComplement[i2, j2] = Matrix[i, j];

                            ++j2;
                        }

                        ++i2;
                    }

                    inverse[temp, diagonal] = Math.Pow(-1, (temp + diagonal)) * new SquareMatrix(Size - 1, matrixForAlgebraicComplement).Determinant();
                }
            }

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    inverse[i, j] = inverse[i, j] / det;
                }
            }

            return new SquareMatrix(Size, inverse);
        }

        //
        // Выясняет большее значение, через нахождение нормы матриц
        //
        public static bool operator >(SquareMatrix first, SquareMatrix second)
        {
            return CalculateNorm(first) > CalculateNorm(second);
        }

        //
        // Выясняет большее или равное значение, через нахождение нормы матриц
        //
        public static bool operator >=(SquareMatrix first, SquareMatrix second)
        {
            return CalculateNorm(first) >= CalculateNorm(second);
        }

        //
        // Выясняет меньшее значение, через нахождение нормы матриц
        //
        public static bool operator <(SquareMatrix first, SquareMatrix second)
        {
            return CalculateNorm(first) < CalculateNorm(second);
        }

        //
        // Выясняет меньшее или равное значение, через нахождение нормы матриц
        //
        public static bool operator <=(SquareMatrix first, SquareMatrix second)
        {
            return CalculateNorm(first) <= CalculateNorm(second);
        }

        //
        // Выясняет равное значение, через нахождение нормы матриц
        //
        public static bool operator ==(SquareMatrix first, SquareMatrix second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        //
        // Выясняет не равное значение, через нахождение нормы матриц
        //
        public static bool operator !=(SquareMatrix first, SquareMatrix second)
        {
            return !(first == second);
        }

        public static explicit operator sbyte(SquareMatrix matrix)
        {
            try
            {
                return checked((sbyte)CalculateNorm(matrix));
            }
            catch (OverflowException)
            {
                Console.Write("Попытка конвертировать {0} в {1} привела к переполнению памяти\n\n", "SquareMatrix", "sbyte");

                return 0;
            }
        }

        public static explicit operator byte(SquareMatrix matrix)
        {
            try
            {
                return checked((byte)CalculateNorm(matrix));
            }
            catch (OverflowException)
            {
                Console.Write("Попытка конвертировать {0} в {1} привела к переполнению памяти\n\n", "SquareMatrix", "byte");

                return 0;
            }
        }

        public static explicit operator short(SquareMatrix matrix)
        {
            try
            {
                return checked((short) CalculateNorm(matrix));
            }
            catch (OverflowException)
            {
                Console.Write("Попытка конвертировать {0} в {1} привела к переполнению памяти\n\n", "SquareMatrix", "short");

                return 0;
            }
        }

        public static explicit operator ushort(SquareMatrix matrix)
        {
            try
            {
                return checked((ushort)CalculateNorm(matrix));
            }
            catch (OverflowException)
            {
                Console.Write("Попытка конвертировать {0} в {1} привела к переполнению памяти\n\n", "SquareMatrix", "ushort");

                return 0;
            }
        }

        public static explicit operator int(SquareMatrix matrix)
        {
            return (int) CalculateNorm(matrix);
        }

        public static explicit operator uint(SquareMatrix matrix)
        {
            return (uint)CalculateNorm(matrix);
        }

        public static explicit operator double(SquareMatrix matrix)
        {
            return CalculateNorm(matrix);
        }

        public static explicit operator float(SquareMatrix matrix)
        {
            return (float)CalculateNorm(matrix);
        }

        public static explicit operator long(SquareMatrix matrix)
        {
            return (long)CalculateNorm(matrix);
        }

        public static explicit operator ulong(SquareMatrix matrix)
        {
            return (ulong)CalculateNorm(matrix);
        }

        public static explicit operator string(SquareMatrix matrix)
        {
            return CalculateNorm(matrix).ToString();
        }

        public static explicit operator decimal(SquareMatrix matrix)
        {
            return (decimal)CalculateNorm(matrix);
        }

        public static explicit operator char(SquareMatrix matrix)
        {
            throw new MatrixOperationException("Приведение матрицы в тип char невозможно.");
        }

        public static explicit operator bool(SquareMatrix matrix)
        {
            return CalculateNorm(matrix) != 0;
        }

        public int CompareTo(SquareMatrix matrix)
        {
            if (ReferenceEquals(matrix, null))
            {
                return 1;
            }

            return CalculateNorm(this).CompareTo(CalculateNorm(matrix));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Size: ");
            builder.Append(Size);
            builder.AppendLine();
            builder.AppendLine();

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    builder.Append(Matrix[i, j]);

                    if (j < Size - 1)
                    {
                        builder.Append(" ");
                    }
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public override bool Equals(object matrix)
        {
            if (matrix is SquareMatrix squareMatrix)
            {
                if (Size != squareMatrix.Size)
                {
                    return false;
                }

                for (int i = 0; i < Size; ++i)
                {
                    for (int j = 0; j < Size; ++j)
                    {
                        if (Matrix[i, j] != squareMatrix.Matrix[i, j])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Size;

                for (int i = 0; i < Size; ++i)
                {
                    for (int j = 0; j < Size; ++j)
                    {
                        hash = (hash * 397) ^ Matrix[i, j].GetHashCode();
                    }
                }

                return hash;
            }
        }

        private static void ValidateSize(int size)
        {
            if (size <= 1)
            {
                throw new MatrixSizeException("Размер матрицы должен быть больше 1.");
            }
        }

        private static void ValidatePositiveSize(int size)
        {
            if (size <= 0)
            {
                throw new MatrixSizeException("Размер матрицы должен быть положительным.");
            }
        }

        private static void ValidateMatrix(int size, double[,] matrix)
        {
            ValidateNotNull(matrix, nameof(matrix));

            if (matrix.GetLength(0) != size || matrix.GetLength(1) != size)
            {
                throw new MatrixDimensionException("Размеры массива должны совпадать с размером матрицы.");
            }
        }

        private static void ValidateNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new MatrixNullException("Значение " + parameterName + " не должно быть null.");
            }
        }

        private static int GetMatrixSize(double[,] matrix)
        {
            ValidateNotNull(matrix, nameof(matrix));

            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new MatrixDimensionException("Матрица должна быть квадратной.");
            }

            int size = matrix.GetLength(0);
            ValidateSize(size);

            return size;
        }
    }
}
