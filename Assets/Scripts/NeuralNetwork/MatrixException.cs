using System;

public class MatrixException : Exception { }
public class InvalidVector : MatrixException { }
public class InvalidSize : MatrixException { }
public class OutOfBounds : MatrixException { }