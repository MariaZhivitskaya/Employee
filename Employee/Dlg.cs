using System;
using System.Windows.Forms;

namespace Employee
{
    public partial class Dlg 
        : Form
    {
        private readonly Employees collection;
        private bool isCheckable;
        private readonly bool isEditable;
        private int index;
        private Employee empEdit;
        private bool isChangableByPicture;
        private readonly string[] data;

        public bool IsCheckable
        {
            get
            {
                return isCheckable;
            }
        }

        public bool IsChangableByPost { get; private set; }

        public Dlg(Employees employees, bool isEditable, string newPost)
        {
            collection = employees;
            this.isEditable = isEditable;
            InitializeComponent(); 
            comboBoxSex.Items.Add("man");
            comboBoxSex.Items.Add("woman");

            if (newPost == null)
            {               
                comboBoxPost.Items.Add("department head");
                comboBoxPost.Items.Add("division head");
                comboBoxPost.Items.Add("manager");
                comboBoxPost.Items.Add("clerk");
            }
            else
            {
                comboBoxPost.Items.Add(newPost);
                comboBoxPost.SelectedItem = newPost;
                comboBoxPost.SelectedIndex = 0;
                comboBoxPost.Enabled = false;
                button3.Visible = false;
                button3.Enabled = false;
            }
            
            data = new string[15];
        }

        public void DlgEdit(Employee emp, int ind)
        {
            empEdit = emp;
            textSurname.Text = emp.Surname;
            textName.Text = emp.Name;
            textPatronymic.Text = emp.Patronymic;
            textBoxSalary.Text = emp.Salary.ToString();
            textBox1.Text = System.IO.Path.GetFileName(emp.Picture);

            switch (emp.Sex)
            {
                case Person.Man:
                    comboBoxSex.Text = "man";
                    break;
                case Person.Woman:
                    comboBoxSex.Text = "woman";
                    break;

            }

            switch (emp.OccupiedPost)
            {
                case Post.Director:
                    comboBoxPost.Items.Add("director");
                    comboBoxPost.Text = "director";                    
                    comboBoxHead.Items.Add("-");
                    break;
                case Post.DepartmentHead:
                    comboBoxPost.Text = "department head";
                    break;
                case Post.DivisionHead:
                    comboBoxPost.Text = "division head";
                    break;
                case Post.Manager:
                    comboBoxPost.Text = "manager";
                    break;
                case Post.Clerk:
                    comboBoxPost.Text = "clerk";
                    break;
            }

            if (emp.Head != null)
            {
                comboBoxHead.Text = emp.Head.Surname + " " + emp.Head.Name + " " + emp.Head.Patronymic;
            }
            else
            {
                comboBoxHead.Text = "-";
            }

            dateTimePicker1.Value = new DateTime(emp.YearOfBirth, emp.MonthOfBirth, emp.DayOfBirth);
            dateTimePicker2.Value = new DateTime(emp.YearOfHire, emp.MonthOfHire, emp.DayOfHire);
            index = ind;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textSurname.Text != "" && textName.Text != "" && textPatronymic.Text != "" && textBoxSalary.Text != ""
                && comboBoxSex.Text != "" && comboBoxPost.Text != "" && comboBoxHead.Text != "")
            {               
                data[0] = textSurname.Text;
                data[1] = textName.Text;
                data[2] = textPatronymic.Text;
                data[3] = dateTimePicker1.Value.Year.ToString();
                data[4] = dateTimePicker1.Value.Month.ToString();
                data[5] = dateTimePicker1.Value.Day.ToString();

                if (comboBoxPost.Text.Equals("department head"))
                {
                    data[6] = "department_head";
                }
                else if (comboBoxPost.Text.Equals("division head"))
                {
                    data[6] = "division_head";
                }
                else
                {
                    data[6] = comboBoxPost.Text;
                }

                data[7] = dateTimePicker2.Value.Year.ToString();
                data[8] = dateTimePicker2.Value.Month.ToString();
                data[9] = dateTimePicker2.Value.Day.ToString();
                data[10] = textBoxSalary.Text;
                data[13] = comboBoxSex.Text;

                var emp = new Employee();

                if (isEditable)
                {
                    data[12] = empEdit.Id.ToString();

                    if (!isChangableByPicture)
                    {
                        data[14] = empEdit.Picture;
                    }
                }
                else
                {
                    data[12] = (collection.MaxId + 1).ToString();
                }                   

                int ind = 0;

                if (comboBoxHead.Text.CompareTo("-") == 0)
                {
                    data[11] = "-";                        
                }
                else
                {
                    for (int i = 0; i < collection.Size(); i++)
                    {
                        if (comboBoxHead.Text.CompareTo(collection[i].FullName()) == 0)
                        {
                            ind = i;
                            data[11] = collection[i].Id.ToString();
                            break;
                        }
                    }                        
                }                    
                if (emp.CheckDataEmployee(data))
                {
                    if (data[14] == null)
                    {
                        data[14] = Environment.CurrentDirectory + "/no.jpg";
                    }

                    emp = new Employee(data[0], data[1], data[2], int.Parse(data[3]),
                        int.Parse(data[4]), int.Parse(data[5]), data[6], int.Parse(data[7]),
                        int.Parse(data[8]), int.Parse(data[9]), int.Parse(data[10]), int.Parse(data[12]), data[13],
                        data[14])
                    {
                        Head = data[11].Equals("-") ? null : collection[ind]
                    };


                    if (!isEditable)
                    {                            
                        collection.Add(emp);
                    }
                    else
                    {
                        string post = collection[index].OccupiedPost.ToString();

                        if (!post.Equals(emp.OccupiedPost.ToString()))
                        {
                            IsChangableByPost = true;
                        }

                        collection[index] = emp;
                    }
                    if (!isEditable)
                    {
                        collection.MaxId++; 
                    }
                      
                    isCheckable = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong information! Please, check all fields!");
                }          
            }
            else
            {
                MessageBox.Show("Please, fill all fields!");
            }
        }

        private void comboBoxPost_SelectedValueChanged(object sender, EventArgs e)
        {
            var employees = new Employees();

            employees = collection;

            if (comboBoxPost.SelectedItem.ToString().Equals("department head"))
            {
                comboBoxHead.Items.Clear();

                for (int i = 0; i < employees.Size(); i++)
                {
                    if ((int)employees[i].OccupiedPost == 0)
                    {                      
                        comboBoxHead.Items.Add(employees[i].FullName());
                        break;
                    }
                }               
            }
            else if (comboBoxPost.SelectedItem.ToString().Equals("division head"))
            {
                comboBoxHead.Items.Clear();

                foreach (Employee emp in employees.GetDepartmentHeads())
                {
                    if (isEditable)
                    {
                        if (emp != empEdit)
                        {
                            comboBoxHead.Items.Add(emp.FullName());
                        }
                    }
                    else
                    {
                        comboBoxHead.Items.Add(emp.FullName());
                    }           
                }
            }
            else if (comboBoxPost.SelectedItem.ToString().Equals("manager"))
            {
                comboBoxHead.Items.Clear();

                foreach (Employee emp in employees.GetDepartmentHeads())
                {
                    if (isEditable)
                    {
                        if (emp != empEdit)
                        {
                            comboBoxHead.Items.Add(emp.FullName());
                        }
                    }
                    else
                    {
                        comboBoxHead.Items.Add(emp.FullName());
                    }      
                }

                foreach (Employee emp in employees.GetDivisionHeads())
                {
                    if (isEditable)
                    {
                        if (emp != empEdit)
                        {
                            comboBoxHead.Items.Add(emp.FullName());
                        }
                    }
                    else
                    {
                        comboBoxHead.Items.Add(emp.FullName());
                    }      
                }
            }
            else if (comboBoxPost.SelectedItem.ToString().Equals("clerk"))
            {
                comboBoxHead.Items.Clear();

                foreach (Employee emp in employees.GetDivisionHeads())
                {
                    if (isEditable)
                    {
                        if (emp != empEdit)
                        {
                            comboBoxHead.Items.Add(emp.FullName());
                        }
                    }
                    else
                    {
                        comboBoxHead.Items.Add(emp.FullName());
                    }      
                }
            }
            if (comboBoxPost.SelectedItem.ToString().Equals("director"))
            {
                comboBoxHead.Items.Clear();
                comboBoxHead.Items.Add("-");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog1.OpenFile() != null)
                    {
                        data[14] = openFileDialog1.FileName;
                        textBox1.Text = System.IO.Path.GetFileName(openFileDialog1.FileName);
                        isChangableByPicture = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Error! Could not open this file!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isCheckable = false;
            Close();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(button2, "Some of them you can find here: Employee/Employee/bin/Debug");
        }       
    }
}
