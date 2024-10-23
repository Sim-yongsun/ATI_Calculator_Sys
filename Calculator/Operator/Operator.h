#pragma once

using namespace System;

namespace Operator {
	public ref class MyOperator
	{
	private:
		double value;
	public:
		double Plus(double num1, double num2);
		double Minus(double num1, double num2);
		double Multiply(double num1, double num2);
		double Divide(double num1, double num2);
		double Sqrt(double num);
	};
}
