using System;

namespace ConsoleApp2.thirdTaskClass
{
    class MatrixException : Exception
    {
        public MatrixException(string message) : base(message) {}
    }

    class MatrixSizeException : MatrixException
    {
        public MatrixSizeException(string message) : base(message) {}
    }

    class MatrixNullException : MatrixException
    {
        public MatrixNullException(string message) : base(message) {}
    }

    class MatrixDimensionException : MatrixException
    {
        public MatrixDimensionException(string message) : base(message) {}
    }

    class MatrixOperationException : MatrixException
    {
        public MatrixOperationException(string message) : base(message) {}
    }

    class SingularMatrixException : MatrixException
    {
        public SingularMatrixException(string message) : base(message) {}
    }
}
