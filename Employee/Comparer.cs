using System.Collections;

namespace Employee
{
    class ComparerFullName 
        : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((Employee)x).Surname.CompareTo(((Employee)y).Surname) == 1)
            {
                return 1;
            }
            if (((Employee)x).Surname.CompareTo(((Employee)y).Surname) == -1)
            {
                return -1;
            }
            if (((Employee)x).Name.CompareTo(((Employee)y).Name) == 1)
            {
                return 1;
            }
            if (((Employee)x).Name.CompareTo(((Employee)y).Name) == -1)
            {
                return -1;
            }
            if (((Employee)x).Patronymic.CompareTo(((Employee)y).Patronymic) == 1)
            {
                return 1;
            }
            if (((Employee)x).Patronymic.CompareTo(((Employee)y).Patronymic) == -1)
            {
                return -1;
            }

            return 0;
        }
    }

    class ComparerDateOfBirth 
        : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((Employee)x).DateOfBirth.CompareTo(((Employee)y).DateOfBirth) == 1)
            {
                return 1;
            }
            if (((Employee)x).DateOfBirth.CompareTo(((Employee)y).DateOfBirth) == -1)
            {
                return -1;
            }

            return 0;
        }
    }

    class ComparerPost 
        : IComparer
    {
        public int Compare(object x, object y)
        {
            if ((int)((Employee)x).OccupiedPost > (int)((Employee)y).OccupiedPost)
            {
                return 1;
            }
            if ((int)((Employee)x).OccupiedPost < (int)((Employee)y).OccupiedPost)
            {
                return -1;
            }

            return 0;
        }
    }

    class ComparerSalary 
        : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((Employee)x).Salary > ((Employee)y).Salary)
            {
                return 1;
            }
            if (((Employee)x).Salary < ((Employee)y).Salary)
            {
                return -1;
            }

            return 0;
        }
    }

    class ComparerDateOfHire 
        : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((Employee)x).DateOfHire.CompareTo(((Employee)y).DateOfHire) == 1)
            {
                return 1;
            }
            if (((Employee)x).DateOfHire.CompareTo(((Employee)y).DateOfHire) == -1)
            {
                return -1;
            }

            return 0;
        }
    }

    class ComparerHead 
        : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((Employee)x).Head != null && ((Employee)y).Head != null)
            {
                if (((Employee)x).Head.Surname.CompareTo(((Employee)y).Head.Surname) == 1)
                {
                    return 1;
                }
                if (((Employee)x).Head.Surname.CompareTo(((Employee)y).Head.Surname) == -1)
                {
                    return -1;
                }
                if (((Employee)x).Head.Name.CompareTo(((Employee)y).Head.Name) == 1)
                {
                    return 1;
                }
                if (((Employee)x).Head.Name.CompareTo(((Employee)y).Head.Name) == -1)
                {
                    return -1;
                }
                if (((Employee)x).Head.Patronymic.CompareTo(((Employee)y).Head.Patronymic) == 1)
                {
                    return 1;
                }
                if (((Employee)x).Head.Patronymic.CompareTo(((Employee)y).Head.Patronymic) == -1)
                {
                    return -1;
                }

                return 0;
            }

            if (((Employee)x).Head != null)
            {
                return 1;
            }
            if (((Employee)y).Head != null)
            {
                return -1;
            }

            return 0;
        }
    }

    class ComparerId 
        : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((Employee)x).Id.CompareTo(((Employee)y).Id) == 1)
            {
                return 1;
            }
            if (((Employee)x).Id.CompareTo(((Employee)y).Id) == -1)
            {
                return -1;
            }

            return 0;
        }
    }
}
