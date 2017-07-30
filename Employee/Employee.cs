using System;

namespace Employee
{
    public enum Post
    {
        Director, DepartmentHead, DivisionHead, Manager, Clerk
    };

    public enum Person
    {
        Man, Woman
    };

    public class Employee
        : IComparable, ICloneable
    {
        public Employee()
        {
        }

        public Employee(string surname, string name, string patronymic,
            int yearOfBirth, int monthOfBirth, int dayOfBirth, string post,
            int yearOfHire, int monthOfHire, int dayOfHire, int salary, int id, string sex, string picture)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            DateOfBirth = new DateTime(yearOfBirth, monthOfBirth, dayOfBirth);
            DateOfHire = new DateTime(yearOfHire, monthOfHire, dayOfHire);
            Id = id;
            Salary = salary;
            Picture = picture;

            switch (post)
            {
                case "director":
                    OccupiedPost = Post.Director;
                    break;
                case "department_head":
                    OccupiedPost = Post.DepartmentHead;
                    break;
                case "division_head":
                    OccupiedPost = Post.DivisionHead;
                    break;
                case "manager":
                    OccupiedPost = Post.Manager;
                    break;
                case "clerk":
                    OccupiedPost = Post.Clerk;
                    break;
            }

            switch (sex)
            {
                case "man":
                    Sex = Person.Man;
                    break;
                case "woman":
                    Sex = Person.Woman;
                    break;
            }
        }

        public string Surname { set; get; }

        public string Name { set; get; }

        public string Patronymic { set; get; }

        public int DayOfBirth
        {
            set
            {
                DateTime dt = DateOfBirth.AddDays(value - 1);
                DateOfBirth = dt;
            }
            get
            {
                return DateOfBirth.Day;
            }
        }

        public int MonthOfBirth
        {
            set
            {
                DateTime dt = DateOfBirth.AddMonths(value - 1);
                DateOfBirth = dt;
            }
            get
            {
                return DateOfBirth.Month;
            }
        }

        public int YearOfBirth
        {
            set
            {
                DateOfBirth = new DateTime();
                DateTime dt = DateOfBirth.AddYears(value - 1);
                DateOfBirth = dt;
            }
            get
            {
                return DateOfBirth.Year;
            }
        }

        public DateTime DateOfBirth { get; private set; }

        public Post OccupiedPost { set; get; }

        public int DayOfHire
        {
            set
            {
                DateTime dt = DateOfHire.AddDays(value - 1);
                DateOfHire = dt;
            }
            get
            {
                return DateOfHire.Day;
            }
        }

        public int MonthOfHire
        {
            set
            {
                DateTime dt = DateOfHire.AddMonths(value - 1);
                DateOfHire = dt;
            }
            get
            {
                return DateOfHire.Month;
            }
        }

        public int YearOfHire
        {
            set
            {
                DateOfHire = new DateTime();
                DateTime dt = DateOfHire.AddYears(value - 1);
                DateOfHire = dt;
            }
            get
            {
                return DateOfHire.Year;
            }
        }

        public DateTime DateOfHire { get; private set; }

        public int Salary { set; get; }

        public Employee Head { set; get; }

        public int Id { set; get; }

        public Person Sex { set; get; }

        public string Picture { set; get; }

        public int Age
        {
            get
            {
                var dateNow = DateTime.Now;
                var currentAge = dateNow.Year - DateOfBirth.Year;
                if (dateNow.Month < DateOfBirth.Month ||
                    (dateNow.Month == DateOfBirth.Month && dateNow.Day < DateOfBirth.Day))
                {
                    currentAge--;
                }

                return currentAge;
            }
        }

        public int Experience
        {
            get
            {
                var dateNow = DateTime.Now;
                var exp = dateNow.Year - DateOfHire.Year;
                if (dateNow.Month < DateOfHire.Month ||
                    (dateNow.Month == DateOfHire.Month && dateNow.Day < DateOfHire.Day))
                {
                    exp--;
                }

                return exp;
            }
        }

        public string FullName()
        {
            return Surname + " " + Name + " " + Patronymic;
        }

        public int CompareTo(object obj)
        {
            if (Experience.CompareTo(((Employee)obj).Experience) == 1)
            {
                return 1;
            }
            if (Experience.CompareTo(((Employee)obj).Experience) == -1)
            {
                return -1;
            }
            if ((Salary).CompareTo(((Employee)obj).Salary) == 1)
            {
                return 1;
            }
            if ((Salary).CompareTo(((Employee)obj).Salary) == -1)
            {
                return -1;
            }
            if (OccupiedPost.CompareTo((int)((Employee)obj).OccupiedPost) == 1)
            {
                return 1;
            }
            if (OccupiedPost.CompareTo((int)((Employee)obj).OccupiedPost) == -1)
            {
                return -1;
            }

            return 0;
        }

        public object Clone()
        {
            string postClone = "";

            switch (OccupiedPost)
            {
                case Post.Director:
                    postClone = "director";
                    break;
                case Post.DepartmentHead:
                    postClone = "department head";
                    break;
                case Post.DivisionHead:
                    postClone = "division head";
                    break;
                case Post.Manager:
                    postClone = "manager";
                    break;
                case Post.Clerk:
                    postClone = "clerk";
                    break;
            }

            string sexClone = "";

            switch (Sex)
            {
                case Person.Man:
                    sexClone = "man";
                    break;
                case Person.Woman:
                    sexClone = "woman";
                    break;
            }

            var emp = new Employee(Surname, Name, Patronymic,
                YearOfBirth, MonthOfBirth, DayOfBirth, postClone,
                YearOfHire, MonthOfHire, DayOfHire, Salary, Id, sexClone, Picture)
            {
                Head = Head
            };

            return emp;
        }

        public override string ToString()
        {
            string occupiedPost = "";
            string sex = "";
            string head;

            switch (OccupiedPost)
            {
                case Post.Director:
                    occupiedPost = "director";
                    break;
                case Post.DepartmentHead:
                    occupiedPost = "department head";
                    break;
                case Post.DivisionHead:
                    occupiedPost = "division head";
                    break;
                case Post.Manager:
                    occupiedPost = "manager";
                    break;
                case Post.Clerk:
                    occupiedPost = "clerk";
                    break;
            }

            switch (Sex)
            {
                case Person.Man:
                    sex = "man";
                    break;
                case Person.Woman:
                    sex = "woman";
                    break;
            }

            if (Head != null)
            {
                head = "Head: " + Head.Surname + "\n";
            }
            else
            {
                head = "Head: -\n";
            }

            return "Surname: " + Surname + "\n" + "Name: " + Name + "\n" + "Patronymic: " + Patronymic + "\n" +
                "Sex: " + sex + "\n" + "Date of bitrth: " + DateOfBirth.ToString("dd/MM/yyyy") + "\n" +
                "Post: " + occupiedPost + "\n" + "Date of hire: " + DateOfHire.ToString("dd/MM/yyyy") + "\n" +
                "Salary: " + Salary + "\n" + "Age: " + Age + "\n" + "Experience: " + Experience + "\n" + head;
        }

        public bool CheckDataEmployee(string[] s)
        {
            return CheckYearOfBirth(s[3]) && CheckMonth(s[4]) && CheckDay(s[5]) && CheckPost(s[6]) &&
                CheckYearOfHire(s[7]) && CheckMonth(s[8]) && CheckDay(s[9]) && CheckSalary(s[10]) &&
                CheckHead(s[11]) && CheckId(s[12]) && CheckSex(s[13]);
        }

        private static bool CheckYearOfBirth(string yearOfBirth)
        {
            int year;

            return int.TryParse(yearOfBirth, out year) && (year > 1915 && year < 1998);
        }

        private static bool CheckYearOfHire(string yearOfHire)
        {
            int year;

            return int.TryParse(yearOfHire, out year) && (year > 1930 && year < 2017);
        }

        private static bool CheckMonth(string checkMonth)
        {
            int month;

            return int.TryParse(checkMonth, out month) && (month > 0 && month < 13);
        }

        private static bool CheckDay(string checkDay)
        {
            int day;

            return int.TryParse(checkDay, out day) && (day > 0 && day < 32);
        }

        private static bool CheckPost(string post)
        {
            return post.Equals("director") || post.Equals("department_head") ||
                post.Equals("division_head") || post.Equals("manager") || post.Equals("clerk");
        }

        private static bool CheckSalary(string checkSalary)
        {
            int salary;

            return int.TryParse(checkSalary, out salary) && salary > 0;
        }

        private static bool CheckHead(string checkHead)
        {
            int head;

            return checkHead.Equals("-") || ((int.TryParse(checkHead, out head) && head > 0));
        }

        private static bool CheckId(string checkId)
        {
            int id;

            return int.TryParse(checkId, out id) && id > 0;
        }

        private static bool CheckSex(string sex)
        {
            return sex.Equals("man") || sex.Equals("woman");
        }
    }
}
