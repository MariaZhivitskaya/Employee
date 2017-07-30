using System;
using System.Windows.Forms;

namespace Employee
{
    public partial class Head 
        : Form
    {
        private readonly Employee employee;
        private readonly Employees collection;

        public Head()
        {
        }

        public Head(Employees collection, Employee e)
        {
            employee = e;
            this.collection = collection;
            InitializeComponent();
            textBox1.Text = e.FullName();

            switch (e.OccupiedPost)
            {
                case Post.Director:
                    textBox2.Text = "director";
                    break;
                case Post.DepartmentHead:
                    textBox2.Text = "department_head";
                    break;
                case Post.DivisionHead:
                    textBox2.Text = "division_head";
                    break;
                case Post.Manager:
                    textBox2.Text = "manager";
                    break;
                case Post.Clerk:
                    textBox2.Text = "clerk";
                    break;
            }

            if (textBox2.Text.Equals("department_head"))
            {                
                for (int i = 0; i < collection.Size(); i++)
                {
                    if ((int)collection[i].OccupiedPost == 0)
                    {                      
                        comboBox1.Items.Add(collection[i].FullName());
                        break;
                    }
                }               
            }
            else if (textBox2.Text.Equals("division_head"))
            {
                foreach (Employee emp in collection.GetDepartmentHeads())
                {
                    if (emp != employee)
                    {
                        comboBox1.Items.Add(emp.FullName());
                    }                            
                }
            }
            else if (textBox2.Text.Equals("manager"))
            {
                foreach (Employee emp in collection.GetDepartmentHeads())
                {
                    comboBox1.Items.Add(emp.FullName());
                }
                foreach (Employee emp in collection.GetDivisionHeads())
                {
                    comboBox1.Items.Add(emp.FullName());
                }
            }
            else if (textBox2.Text.Equals("clerk"))
            {
                foreach (Employee emp in collection.GetDivisionHeads())
                {
                    comboBox1.Items.Add(emp.FullName());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                for (int i = 0; i < collection.Size(); i++)
                {
                    if (comboBox1.Text.CompareTo(collection[i].FullName()) == 0)
                    {
                        employee.Head = collection[i];
                        break;
                    }
                }

                Close();
            }
            else
            {
                MessageBox.Show("Please, fill all fields!");
            }
        }
    }
}
