using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;
using UnityEngine.TestTools;

public class MatrixTests
{
    [Test]
    public void MatrixMultiply1()
    {
        double[,] aValues = new double[2, 2] { { 4, -1 }, { 0, 7 } };
        Matrix a = new Matrix(aValues);
        double[,] bValues = new double[2, 2] { { -1, 1 }, { 5, 2 } };
        Matrix b = new Matrix(bValues);
        double[,] cValues = new double[2, 2] { { -9, 2 }, { 35, 14 } };
        Matrix c = new Matrix(cValues);
        Assert.IsTrue(Matrix.Equal(Matrix.Multiply(a, b), c));
    }

    [Test]
    public void MatrixMultiply2()
    {
        double[,] aValues = new double[2, 2] { { 2.1, 3.5 }, { -0.5, 6 } };
        Matrix a = new Matrix(aValues);
        double[,] bValues = new double[2, 2] { { 0.5, 3 }, { -6, 6 } };
        Matrix b = new Matrix(bValues);
        double[,] cValues = new double[2, 2] { { -19.95, 27.3 }, { -36.25, 34.5 } };
        Matrix c = new Matrix(cValues);
        Assert.IsTrue(Matrix.Equal(Matrix.Multiply(a, b), c));
    }

    [Test]
    public void MatrixMultiply3()
    {
        double[,] aValues = new double[2, 3] { { 2, 4, -1 }, { 2, -6, 5 } };
        Matrix a = new Matrix(aValues);
        double[,] bValues = new double[3, 2] { { 3, 0 }, { 7, -1 }, { -9, 11} };
        Matrix b = new Matrix(bValues);
        double[,] cValues = new double[2, 2] { { 43, -15 }, { -81, 61 } };
        Matrix c = new Matrix(cValues);
        Assert.IsTrue(Matrix.Equal(Matrix.Multiply(a, b), c));
    }

    [Test]
    public void MatrixMultiply4()
    {
        double[,] aValues = new double[3, 2] { { 2, 3}, { -2, 4 }, { 1, 8 } };
        Matrix a = new Matrix(aValues);
        double[,] bValues = new double[3, 3] { { -4, 2, -1 }, { -5, -1, 0 }, { 2, -6, 3} };
        Matrix b = new Matrix(bValues);
        Assert.That(() => Matrix.Multiply(a, b), Throws.TypeOf<InvalidSize>());
    }

    [Test]
    public void MatrixAdd1()
    {
        double[,] aValues = new double[2, 2] { { 4, -1 }, { 0, 7 } };
        Matrix a = new Matrix(aValues);
        double[,] bValues = new double[2, 2] { { -1, 1 }, { 5, 2 } };
        Matrix b = new Matrix(bValues);
        double[,] cValues = new double[2, 2] { { 3, 0 }, { 5, 9 } };
        Matrix c = new Matrix(cValues);
        Assert.IsTrue(Matrix.Equal(Matrix.Add(a, b), c));
    }

    [Test]
    public void MatrixAdd2()
    {
        double[,] aValues = new double[2, 2] { { 2.1, 3.5 }, { -0.5, 6 } };
        Matrix a = new Matrix(aValues);
        double[,] bValues = new double[2, 2] { { 0.5, 3 }, { -6, 6 } };
        Matrix b = new Matrix(bValues);
        double[,] cValues = new double[2, 2] { { 2.6, 6.5 }, { -6.5, 12 } };
        Matrix c = new Matrix(cValues);
        Assert.IsTrue(Matrix.Equal(Matrix.Add(a, b), c));
    }

    [Test]
    public void MatrixAdd3()
    {
        double[,] aValues = new double[2, 3] { { 2, 4, -1 }, { 2, -6, 5 } };
        Matrix a = new Matrix(aValues);
        double[,] bValues = new double[3, 2] { { 3, 0 }, { 7, -1 }, { -9, 11 } };
        Matrix b = new Matrix(bValues);
        Assert.That(() => Matrix.Add(a, b), Throws.TypeOf<InvalidSize>());
    }

    [Test]
    public void EmptyMatrix()
    {
        Matrix a = new Matrix(3, 2);
        double[,] bValues = new double[3, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 } };
        Matrix b = new Matrix(bValues);
        Assert.IsTrue(Matrix.Equal(a, b));
    }

    [Test]
    public void GetColumn1()
    {
        double[,] aValues = new double[2, 3] { { 2, 4, -1 }, { 2, -6, 5 } };
        Matrix a = new Matrix(aValues);
        double[] bValues = { 4, -6 };
        Assert.IsTrue(a.GetColumn(1)[0] == bValues[0] && a.GetColumn(1)[1] == bValues[1]);
    }

    [Test]
    public void GetColumn2()
    {
        double[,] aValues = new double[2, 3] { { 2, 4, -1 }, { 2, -6, 5 } };
        Matrix a = new Matrix(aValues);
        double[] bValues = { 2, 2 };
        Assert.IsTrue(a.GetColumn(0)[0] == bValues[0] && a.GetColumn(0)[1] == bValues[1]);
    }

    [Test]
    public void GetColumn3()
    {
        double[,] aValues = new double[2, 3] { { 2, 4, -1 }, { 2, -6, 5 } };
        Matrix a = new Matrix(aValues);
        double[] bValues = { -1, 5 };
        Assert.IsTrue(a.GetColumn(2)[0] == bValues[0] && a.GetColumn(2)[1] == bValues[1]);
    }

    [Test]
    public void GetColumn4()
    {
        double[,] aValues = new double[2, 3] { { 2, 4, -1 }, { 2, -6, 5 } };
        Matrix a = new Matrix(aValues);
        Assert.That(() => a.GetColumn(3), Throws.TypeOf<OutOfBounds>());
    }
}