namespace AlumniProject.ExceptionHandler;
using System;
public class NotFoundException : Exception
{

	public NotFoundException(String message):base(message)
	{

	}
}
