using System;

namespace Employee
{
    class EmployeeException 
        : Exception
    {
        private readonly int type;

        public EmployeeException(int typeOfException)
        {
            type = typeOfException;
        }

        public override string ToString()
        {
            string str;

            switch (type)
            {
                case 1: 
                    str = "Wrong element in the collection 'Employees'!"; 
                    break;
                case 2:
                    str = "File is empty!";
                    break;
                case 3: 
                    str = "Wrong number of paremeters in the file!"; 
                    break;
                case 4:
                    str = "Wrong information in the file!"; 
                    break;
                case 5:
                    str = "Wrong request!"; 
                    break;
                default: 
                    str = "Unknown type of exception!"; 
                    break;
            }

            return str;
        }
    }
}
