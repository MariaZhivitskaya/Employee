using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Employee
{
    public partial class Form1 : Form
    {
        readonly Employees collection = new Employees();
        private bool isSortableById;
        private bool isSortableByFullName;
        private bool isSortableByDateOfBirth;
        private bool isSortableByPost;
        private bool isSortableByDateOfHire;
        private bool isSortableBySalary;
        private bool isSortableByHead;
        private readonly ImageList myImageList = new ImageList();
        private readonly ImageList photos = new ImageList();

        public Form1()
        {
            InitializeComponent();
            listView1.Columns.Add("#");
            listView1.Columns.Add("Full name");
            listView1.Columns.Add("Date of birth");
            listView1.Columns.Add("Occupied post");
            listView1.Columns.Add("Date of hire");
            listView1.Columns.Add("Salary");
            listView1.Columns.Add("Head");
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            toolStripStatusLabel1.Text = "";
            statusStrip1.Items.Add(toolStripStatusLabel1);

            myImageList.Images.Add(Image.FromFile(Environment.CurrentDirectory + "/red.png"));
            myImageList.Images.Add(Image.FromFile(Environment.CurrentDirectory + "/green.png"));
            listView1.SmallImageList = myImageList;
            photos.ImageSize = new Size(200, 120);
            photos.ColorDepth = ColorDepth.Depth32Bit;
            listView2.LargeImageList = photos;

            try
            {
                var file = new StreamReader(Environment.CurrentDirectory + "/Empl.txt");
                string line;
                var heads = new ArrayList();

                if ((line = file.ReadLine()) == null)
                {
                    throw new EmployeeException(2);
                }

                string[] data;
                string strPhoto;
                var emp = new Employee();

                data = line.Split(' ');

                if (data.Length == 15)
                {
                    if (emp.CheckDataEmployee(data))
                    {
                        strPhoto = Environment.CurrentDirectory + "/" + data[14];
                        emp = new Employee(data[0], data[1], data[2], int.Parse(data[3]),
                            int.Parse(data[4]), int.Parse(data[5]), data[6], int.Parse(data[7]),
                            int.Parse(data[8]), int.Parse(data[9]), int.Parse(data[10]), int.Parse(data[12]), data[13], strPhoto);
                        if (data[11].Equals("-"))
                        {
                            heads.Add(null);
                        }
                        else
                        {
                            heads.Add(int.Parse(data[11]));
                        }

                        collection.Add(emp);
                    }
                }
                else
                {
                    throw new EmployeeException(4);
                }

                while ((line = file.ReadLine()) != null)
                {
                    data = line.Split(' ');

                    if (data.Length == 15)
                    {
                        if (emp.CheckDataEmployee(data))
                        {
                            strPhoto = Environment.CurrentDirectory + "/" + data[14];
                            emp = new Employee(data[0], data[1], data[2], int.Parse(data[3]),
                                int.Parse(data[4]), int.Parse(data[5]), data[6], int.Parse(data[7]),
                                int.Parse(data[8]), int.Parse(data[9]), int.Parse(data[10]), int.Parse(data[12]), data[13], strPhoto);
                            if (data[11].Equals("-"))
                            {
                                heads.Add(null);
                            }
                            else
                            {
                                heads.Add(int.Parse(data[11]));
                            }

                            collection.Add(emp);
                        }
                        else
                        {
                            throw new EmployeeException(4);
                        }
                    }
                    else
                    {
                        throw new EmployeeException(3);
                    }
                }

                file.Close();

                for (int i = 0; i < collection.Size(); i++)
                {
                    if (heads[i] != null)
                    {
                        for (int j = 0; j < collection.Size(); j++)
                        {
                            if (heads[i].Equals(collection[j].Id))
                            {
                                if (collection[j].OccupiedPost >= collection[i].OccupiedPost)
                                {
                                    collection.Clear();
                                    throw new EmployeeException(4);
                                }

                                collection[i].Head = collection[j];
                            }
                        }
                    }

                    photos.Images.Add(collection[i].Id.ToString(), Image.FromFile(collection[i].Picture));
                    AddElement(collection[i]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                collection.MaxId = collection[collection.Size() - 1].Id;
            }
            catch (EmployeeException e)
            {
                collection.Clear();
                MessageBox.Show(e.ToString(), "Error!");
                Environment.Exit(1);
            }
        }

        private void AddElement(Employee emp)
        {
            string[] data = new string[7];
            int index = 0;
            string post = "";

            data[0] = emp.Id.ToString();
            data[1] = emp.FullName();
            data[2] = emp.DateOfBirth.ToString("dd/MM/yyyy");

            switch (emp.OccupiedPost)
            {
                case Post.Director:
                    post = "director";
                    break;
                case Post.DepartmentHead:
                    post = "department head";
                    break;
                case Post.DivisionHead:
                    post = "division head";
                    break;
                case Post.Manager:
                    post = "manager";
                    break;
                case Post.Clerk:
                    post = "clerk";
                    break;
            }

            data[3] = post;
            data[4] = emp.DateOfHire.ToString("dd/MM/yyyy");
            data[5] = emp.Salary.ToString();

            if (emp.Head == null)
            {
                data[6] = "-";
            }
            else
            {
                data[6] = emp.Head.FullName();
            }

            switch (emp.Sex)
            {
                case Person.Man:
                    index = 0;
                    break;
                case Person.Woman:
                    index = 1;
                    break;
            }

            var item = new ListViewItem(data)
            {
                ImageIndex = index
            };

            listView1.Items.Add(item);

            var itemPhoto = new ListViewItem(emp.FullName())
            {
                ImageIndex = listView2.LargeImageList.Images.Count - 1
            };

            listView2.Items.Add(itemPhoto);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0 && !isSortableById && collection.Size() != 0)
            {
                isSortableById = true;
                isSortableByFullName = false;
                isSortableByDateOfBirth = false;
                isSortableByPost = false;
                isSortableByDateOfHire = false;
                isSortableBySalary = false;
                isSortableByHead = false;

                listView1.Items.Clear();
                listView2.Clear();

                var comparerId = new ComparerId();

                collection.Sort(comparerId);

                for (int j = 0; j < collection.Size(); j++)
                {
                    photos.Images.Add(collection[j].Id.ToString(), Image.FromFile(collection[j].Picture));
                    AddElement(collection[j]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else if (e.Column == 1 && !isSortableByFullName && collection.Size() != 0)
            {
                isSortableById = false;
                isSortableByFullName = true;
                isSortableByDateOfBirth = false;
                isSortableByPost = false;
                isSortableByDateOfHire = false;
                isSortableBySalary = false;
                isSortableByHead = false;

                listView1.Items.Clear();
                listView2.Clear();

                var comparerFullName = new ComparerFullName();

                collection.Sort(comparerFullName);

                for (int j = 0; j < collection.Size(); j++)
                {
                    photos.Images.Add(collection[j].Id.ToString(), Image.FromFile(collection[j].Picture));
                    AddElement(collection[j]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else if (e.Column == 2 && !isSortableByDateOfBirth && collection.Size() != 0)
            {
                isSortableById = false;
                isSortableByFullName = false;
                isSortableByDateOfBirth = true;
                isSortableByPost = false;
                isSortableByDateOfHire = false;
                isSortableBySalary = false;
                isSortableByHead = false;

                listView1.Items.Clear();
                listView2.Clear();

                var comparerDateOfBirth = new ComparerDateOfBirth();

                collection.Sort(comparerDateOfBirth);

                for (int j = 0; j < collection.Size(); j++)
                {
                    photos.Images.Add(collection[j].Id.ToString(), Image.FromFile(collection[j].Picture));
                    AddElement(collection[j]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else if (e.Column == 3 && !isSortableByPost && collection.Size() != 0)
            {
                isSortableById = false;
                isSortableByFullName = false;
                isSortableByDateOfBirth = false;
                isSortableByPost = true;
                isSortableByDateOfHire = false;
                isSortableBySalary = false;
                isSortableByHead = false;

                listView1.Items.Clear();
                listView2.Clear();

                var comparerPost = new ComparerPost();

                collection.Sort(comparerPost);

                for (int j = 0; j < collection.Size(); j++)
                {
                    photos.Images.Add(collection[j].Id.ToString(), Image.FromFile(collection[j].Picture));
                    AddElement(collection[j]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else if (e.Column == 4 && !isSortableByDateOfHire && collection.Size() != 0)
            {
                isSortableById = false;
                isSortableByFullName = false;
                isSortableByDateOfBirth = false;
                isSortableByPost = false;
                isSortableByDateOfHire = true;
                isSortableBySalary = false;
                isSortableByHead = false;

                listView1.Items.Clear();
                listView2.Clear();

                var comparerDateOfHire = new ComparerDateOfHire();

                collection.Sort(comparerDateOfHire);

                for (int j = 0; j < collection.Size(); j++)
                {
                    photos.Images.Add(collection[j].Id.ToString(), Image.FromFile(collection[j].Picture));
                    AddElement(collection[j]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else if (e.Column == 5 && !isSortableBySalary && collection.Size() != 0)
            {
                isSortableById = false;
                isSortableByFullName = false;
                isSortableByDateOfBirth = false;
                isSortableByPost = false;
                isSortableByDateOfHire = false;
                isSortableBySalary = true;
                isSortableByHead = false;

                listView1.Items.Clear();
                listView2.Clear();

                var comparerSalary = new ComparerSalary();

                collection.Sort(comparerSalary);

                for (int j = 0; j < collection.Size(); j++)
                {
                    photos.Images.Add(collection[j].Id.ToString(), Image.FromFile(collection[j].Picture));
                    AddElement(collection[j]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else if (e.Column == 6 && !isSortableByHead && collection.Size() != 0)
            {
                isSortableById = false;
                isSortableByFullName = false;
                isSortableByDateOfBirth = false;
                isSortableByPost = false;
                isSortableByDateOfHire = false;
                isSortableBySalary = false;
                isSortableByHead = true;

                listView1.Items.Clear();
                listView2.Clear();

                var comparerHead = new ComparerHead();

                collection.Sort(comparerHead);

                for (int j = 0; j < collection.Size(); j++)
                {
                    photos.Images.Add(collection[j].Id.ToString(), Image.FromFile(collection[j].Picture));
                    AddElement(collection[j]);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            Employee emp = collection[e.ItemIndex];

            toolStripStatusLabel1.Text = emp.Surname + " " + emp.Name + " " + emp.Patronymic;
            statusStrip1.Items.Add(toolStripStatusLabel1);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int selected = listView1.SelectedIndices[0];
                Employee emp = collection[selected];
                var dlg = new Dlg(collection, true, null);

                dlg.DlgEdit(emp, selected);
                dlg.ShowDialog();

                if (dlg.IsCheckable)
                {
                    toolStripStatusLabel1.Text = "";
                    statusStrip1.Items.Add(toolStripStatusLabel1);

                    if (dlg.IsChangableByPost)
                    {
                        SetHead(emp);
                    }
                    else
                    {
                        listView1.Items.Clear();
                        listView2.Items.Clear();

                        foreach (Employee empl in collection.GetImmediateSubordinates(collection[selected]))
                        {
                            empl.Head = collection[selected];
                        }

                        for (int i = 0; i < collection.Size(); i++)
                        {
                            photos.Images.Add(collection[i].Id.ToString(),
                                                Image.FromFile(collection[i].Picture));
                            AddElement(collection[i]);
                        }
                    }

                    isSortableByFullName = false;
                    isSortableByDateOfBirth = false;
                    isSortableByPost = false;
                    isSortableByDateOfHire = false;
                    isSortableBySalary = false;
                    isSortableByHead = false;
                }
            }
            else
            {
                MessageBox.Show(collection.Size() == 0
                    ? "Invalid operation! List is empty!"
                    : "Please, select an element!");
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (collection.GetDepartmentHeads().Count == 0)
            {
                MessageBox.Show("No department heads in list! Add a new one.");
                var dlg = new Dlg(collection, false, "department head");

                dlg.ShowDialog();
                if (dlg.IsCheckable)
                {
                    photos.Images.Add(collection[collection.Size() - 1].Id.ToString(),
                        Image.FromFile(collection[collection.Size() - 1].Picture));
                    AddElement(collection[collection.Size() - 1]);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                    isSortableByFullName = false;
                    isSortableByDateOfBirth = false;
                    isSortableByPost = false;
                    isSortableByDateOfHire = false;
                    isSortableBySalary = false;
                    isSortableByHead = false;
                }
            }
            else if (collection.GetDivisionHeads().Count == 0)
            {
                MessageBox.Show("No division heads in list! Add a new one.");
                var dlg = new Dlg(collection, false, "division head");

                dlg.ShowDialog();
                if (dlg.IsCheckable)
                {
                    photos.Images.Add(collection[collection.Size() - 1].Id.ToString(),
                        Image.FromFile(collection[collection.Size() - 1].Picture));
                    AddElement(collection[collection.Size() - 1]);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                    isSortableByFullName = false;
                    isSortableByDateOfBirth = false;
                    isSortableByPost = false;
                    isSortableByDateOfHire = false;
                    isSortableBySalary = false;
                    isSortableByHead = false;
                }
            }
            else
            {
                var dlg = new Dlg(collection, false, null);
                dlg.ShowDialog();

                if (dlg.IsCheckable)
                {
                    photos.Images.Add(collection[collection.Size() - 1].Id.ToString(),
                        Image.FromFile(collection[collection.Size() - 1].Picture));
                    AddElement(collection[collection.Size() - 1]);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                    isSortableByFullName = false;
                    isSortableByDateOfBirth = false;
                    isSortableByPost = false;
                    isSortableByDateOfHire = false;
                    isSortableBySalary = false;
                    isSortableByHead = false;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string question = "Do you want to save this collection in file?";
            DialogResult result = MessageBox.Show(question, "Warning", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                var saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.FileName = "Document";
                saveFileDialog1.DefaultExt = ".text";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var streamWriter = new StreamWriter(saveFileDialog1.FileName);

                    foreach (Employee emp in collection)
                    {
                        streamWriter.WriteLine(emp.ToString());
                    }

                    streamWriter.Close();
                    MessageBox.Show("File saved here: " + saveFileDialog1.FileName);
                }
            }

            collection.Clear();
            myImageList.Images.Clear();
            photos.Images.Clear();
            Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selected = listView1.SelectedIndices[0];
                Employee emp = collection[selected];
                string question = "Do you want to delete " + emp.FullName() + "?";
                DialogResult result = MessageBox.Show(question, "Warning", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    collection.Dismiss(emp);
                    toolStripStatusLabel1.Text = "";
                    statusStrip1.Items.Add(toolStripStatusLabel1);
                    photos.Images.RemoveByKey(emp.Id.ToString());
                    SetHead(emp);
                }
            }
            else
            {
                MessageBox.Show("Please, select an employee!");
            }
        }

        private void SetHead(Employee emp)
        {
            ArrayList al = new ArrayList();
            var heads = new ArrayList();

            collection.immediateSubordinatesList.Clear();
            al = collection.GetImmediateSubordinates(emp);

            if (emp.Head == null)
            {
                MessageBox.Show("Director is dismissed! Add a new one.");

                var dlg = new Dlg(collection, false, "director");

                dlg.ShowDialog();

                if (dlg.IsCheckable)
                {
                    AddElement(collection[collection.Size() - 1]);
                    photos.Images.Add(collection[collection.Size() - 1].Id.ToString(),
                        Image.FromFile(collection[collection.Size() - 1].Picture));
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                    isSortableByFullName = false;
                    isSortableByDateOfBirth = false;
                    isSortableByPost = false;
                    isSortableByDateOfHire = false;
                    isSortableBySalary = false;
                    isSortableByHead = false;
                }
            }
            if (al.Count > 0)
            {
                foreach (Employee em in al)
                {
                    heads.Clear();

                    switch (em.OccupiedPost)
                    {
                        case Post.DepartmentHead:
                            heads.Add(collection.GetDirector());
                            break;
                        case Post.DivisionHead:
                            heads = collection.GetDepartmentHeads();
                            break;
                        case Post.Manager:
                            heads.AddRange(collection.GetDepartmentHeads());
                            heads.AddRange(collection.GetDivisionHeads());
                            break;
                        case Post.Clerk:
                            heads = collection.GetDivisionHeads();
                            break;
                    }

                    var head = new Head();

                    if (heads.Count == 1)
                    {
                        if (heads[0] is Employee)
                        {
                            em.Head = heads[0] as Employee;
                        }
                        else
                        {
                            throw new EmployeeException(1);
                        }
                    }

                    else if (heads.Count == 0)
                    {
                        switch (em.OccupiedPost)
                        {
                            case Post.DivisionHead:
                                MessageBox.Show("No department heads in list! Add a new one.");

                                var dlg1 = new Dlg(collection, false, "department head");

                                dlg1.ShowDialog();

                                if (dlg1.IsCheckable)
                                {
                                    photos.Images.Add(collection[collection.Size() - 1].Id.ToString(),
                                        Image.FromFile(collection[collection.Size() - 1].Picture));
                                    AddElement(collection[collection.Size() - 1]);
                                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                                    isSortableByFullName = false;
                                    isSortableByDateOfBirth = false;
                                    isSortableByPost = false;
                                    isSortableByDateOfHire = false;
                                    isSortableBySalary = false;
                                    isSortableByHead = false;
                                }
                                break;
                            case Post.Clerk:
                                MessageBox.Show("No division heads in list! Add a new one.");

                                var dlg2 = new Dlg(collection, false, "division head");

                                dlg2.ShowDialog();

                                if (dlg2.IsCheckable)
                                {
                                    AddElement(collection[collection.Size() - 1]);
                                    photos.Images.Add(collection[collection.Size() - 1].Id.ToString(),
                                        Image.FromFile(collection[collection.Size() - 1].Picture));
                                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                                    isSortableByFullName = false;
                                    isSortableByDateOfBirth = false;
                                    isSortableByPost = false;
                                    isSortableByDateOfHire = false;
                                    isSortableBySalary = false;
                                    isSortableByHead = false;
                                }
                                break;
                        }

                        heads.Add(collection[collection.Size() - 1]);
                        em.Head = heads[0] as Employee;
                    }
                    else
                    {
                        head = new Head(collection, em);
                        head.ShowDialog();
                    }
                }

                listView1.Items.Clear();
                listView2.Items.Clear();

                for (int i = 0; i < collection.Size(); i++)
                {
                    photos.Images.Add(collection[i].Id.ToString(),
                                        Image.FromFile(collection[i].Picture));
                    AddElement(collection[i]);
                }
            }
            else
            {
                listView1.Items.Clear();
                listView2.Items.Clear();

                for (int i = 0; i < collection.Size(); i++)
                {
                    photos.Images.Add(collection[i].Id.ToString(),
                                        Image.FromFile(collection[i].Picture));
                    AddElement(collection[i]);
                }
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int selected = listView1.SelectedIndices[0];
                Employee emp = collection[selected];
                var dlg = new Dlg(collection, true, null);

                dlg.DlgEdit(emp, selected);
                dlg.ShowDialog();

                if (dlg.IsCheckable)
                {
                    toolStripStatusLabel1.Text = "";
                    statusStrip1.Items.Add(toolStripStatusLabel1);

                    if (dlg.IsChangableByPost)
                    {
                        SetHead(emp);
                    }
                    else
                    {
                        listView1.Items.Clear();
                        listView2.Items.Clear();

                        foreach (Employee empl in collection.GetImmediateSubordinates(collection[selected]))
                        {
                            empl.Head = collection[selected];
                        }

                        for (int i = 0; i < collection.Size(); i++)
                        {
                            photos.Images.Add(collection[i].Id.ToString(),
                                                Image.FromFile(collection[i].Picture));
                            AddElement(collection[i]);
                        }
                    }

                    isSortableByFullName = false;
                    isSortableByDateOfBirth = false;
                    isSortableByPost = false;
                    isSortableByDateOfHire = false;
                    isSortableBySalary = false;
                    isSortableByHead = false;
                }
            }
        }

        private void dismissToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selected = listView1.SelectedIndices[0];
                Employee emp = collection[selected];
                string question = "Do you want to delete " + emp.FullName() + "?";
                DialogResult result = MessageBox.Show(question, "Warning", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    collection.Dismiss(emp);
                    toolStripStatusLabel1.Text = "";
                    statusStrip1.Items.Add(toolStripStatusLabel1);
                    photos.Images.RemoveByKey(emp.Id.ToString());
                    SetHead(emp);
                }
            }
            else
            {
                MessageBox.Show("Please, select an employee!");
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                FileName = "Document",
                DefaultExt = ".text",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var streamWriter = new StreamWriter(saveFileDialog1.FileName);

                foreach (Employee emp in collection)
                {
                    streamWriter.WriteLine(emp.ToString());
                }

                streamWriter.Close();
                MessageBox.Show("File saved here: " + saveFileDialog1.FileName);
            }
        }
    }
}


