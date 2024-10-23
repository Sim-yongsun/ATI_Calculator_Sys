#include "pch.h"

#include "Operator.h"

#include <math.h>

namespace Operator
{
	double MyOperator::Plus(double num1, double num2)
	{
		return num1 + num2;
	}
	double MyOperator::Minus(double num1, double num2)
	{
		return num1 - num2;
	}

	double MyOperator::Multiply(double num1, double num2)
	{
		return num1 * num2;
	}

	double MyOperator::Divide(double num1, double num2)
	{
		return num1 / num2;
	}

	double MyOperator::Sqrt(double num)
	{
		return sqrt(num);
	}
}
