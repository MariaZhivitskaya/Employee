using System;
using System.Collections;

namespace Employee
{
    public class Employees
        : ICloneable, IEnumerable, IEnumerator
    {
        private readonly ArrayList employeeList = new ArrayList();

        public ArrayList EmployeeList
        {
            get { return employeeList; }
        }

        public Employees()
        {
        }

        public void Add(Employee employee)
        {
            employeeList.Add(employee);
        }

        public void Clear()
        {
            employeeList.Clear();
        }

        public int Size()
        {
            return employeeList.Count;
        }

        public Employee this[int curIndex]
        {
            get
            {
                if (employeeList[curIndex] is Employee)
                {
                    return employeeList[curIndex] as Employee;
                }

                throw new EmployeeException(1);
            }
            set
            {
                employeeList[curIndex] = value;
            }
        }

        public int GetIndex(Employee employee)
        {
            return employeeList.IndexOf(employee);
        }

        public virtual void Sort(IComparer comparer)
        {
            employeeList.Sort(comparer);
        }

        public void Sort()
        {
            employeeList.Sort();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        private int index = -1;

        public object Current
        {
            get
            {
                return employeeList[index];
            }
        }

        public bool MoveNext()
        {
            if (index == employeeList.Count - 1)
            {
                Reset();
                return false;
            }

            index++;

            return true;
        }

        public void Reset()
        {
            index = -1;
        }

        public int MaxId { set; get; }

        public Employee GetDirector()
        {
            var list = new ArrayList(employeeList);

            list.Sort(new ComparerPost());
            if (list[0] is Employee)
            {
                return list[0] as Employee;
            }

            throw new EmployeeException(1);
        }

        public ArrayList GetDepartmentHeads()
        {
            var headList = new ArrayList();
            var list = new ArrayList(employeeList);

            list.Sort(new ComparerPost());
            for (int i = 0; i < employeeList.Count; i++)
            {
                if (list[i] is Employee)
                {
                    if ((int)(((Employee) list[i]).OccupiedPost) == (int)Post.DepartmentHead)
                    {
                        headList.Add(list[i]);
                    }
                    else if ((int)(((Employee) list[i]).OccupiedPost) > (int)Post.DepartmentHead)
                    {
                        break;
                    }
                }
                else
                {
                    throw new EmployeeException(1);
                }
            }

            return headList;
        }

        public ArrayList GetDivisionHeads()
        {
            var headList = new ArrayList();
            var list = new ArrayList(employeeList);

            list.Sort(new ComparerPost());
            for (int i = 0; i < employeeList.Count; i++)
            {
                if (list[i] is Employee)
                {
                    if ((int)(((Employee) list[i]).OccupiedPost) == (int)Post.DivisionHead)
                    {
                        headList.Add(list[i]);
                    }
                    else if ((int)(((Employee) list[i]).OccupiedPost) > (int)Post.DivisionHead)
                    {
                        break;
                    }
                }
                else
                {
                    throw new EmployeeException(1);
                }
            }

            return headList;
        }

        public ArrayList GetManagers()
        {
            var managers = new ArrayList();
            var list = new ArrayList(employeeList);

            list.Sort(new ComparerPost());
            for (int i = 0; i < employeeList.Count; i++)
            {
                if (list[i] is Employee)
                {
                    if ((int)(((Employee) list[i]).OccupiedPost) == (int)Post.Manager)
                    {
                        managers.Add(list[i]);
                    }
                    else if ((int)(((Employee) list[i]).OccupiedPost) > (int)Post.Manager)
                    {
                        break;
                    }
                }
                else
                {
                    throw new EmployeeException(1);
                }
            }

            return managers;
        }

        public ArrayList GetClerks()
        {
            var clerks = new ArrayList();
            var list = new ArrayList(employeeList);

            list.Sort(new ComparerPost());
            for (int i = 0; i < employeeList.Count; i++)
            {
                if (list[i] is Employee)
                {
                    if ((int)(((Employee) list[i]).OccupiedPost) == (int)Post.Clerk)
                    {
                        clerks.Add(list[i]);
                    }
                }
                else
                {
                    throw new EmployeeException(1);
                }
            }

            return clerks;
        }

        public ArrayList GetImmediateSubordinates(Employee employee)
        {
            var immediateSubordinates = new ArrayList();
            var list = new ArrayList(employeeList);

            list.Sort(new ComparerPost());
            int indexOfEmployee = list.IndexOf(employee);
            int idHead = employee.Id;
            int p = (int)employee.OccupiedPost;

            for (int i = indexOfEmployee + 1; i < employeeList.Count; i++)
            {
                if ((int)(list[i] as Employee).OccupiedPost >= p + 1)
                {
                    if ((list[i] as Employee).Head.Id == idHead)
                    {
                        immediateSubordinates.Add(list[i]);
                    }
                }
            }

            return immediateSubordinates;
        }

        public ArrayList immediateSubordinatesList = new ArrayList();
        private ArrayList subordinates;

        public ArrayList GetSubordinates(Employee e)
        {
            subordinates = GetImmediateSubordinates(e);
            
            if (subordinates.Count != 0)
            {
                immediateSubordinatesList.AddRange(subordinates);
                foreach (Employee elem in subordinates)
                {
                    GetSubordinates(elem);
                }
            }

            return immediateSubordinatesList;
        }

        public void Dismiss(Employee e)
        {
            employeeList.Remove(e);
        }
    }
}