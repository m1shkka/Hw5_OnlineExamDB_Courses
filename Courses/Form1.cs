using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity;


namespace Courses
{
    public partial class Form1 : Form
    {

        public class Converter
        {
            public List<Courses> FromJson(string adress)
            {
                var Json = File.ReadAllText(adress);
                var List = JsonConvert.DeserializeObject<List<Courses>>(Json);
                return List;
            }

            public void FillingInTheDatabase(List<Courses> values)
            {
                using (var ctx = new OnlineExamBD())
                {
                    foreach (var courses in values)
                    ctx.Courses.Add(courses);

                    ctx.SaveChanges();
                }
            }
        }

        List<Courses> CoursesList = new List<Courses>();
        Converter converter = new Converter();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (var ctx = new OnlineExamBD())
            {
                var sqlCommand = File.ReadAllText(@"C:\Users\misha\Desktop\DB_DUMP.sql");
                ctx.Database.ExecuteSqlCommandAsync(sqlCommand);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Courses> values = converter.FromJson(@"C:\Users\misha\source\repos\Courses\Courses\bin\Debug\CoursesJSON.txt");
            CoursesList = values;
            converter.FillingInTheDatabase(values);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var ctx = new OnlineExamBD())
            {
                foreach (var code in CoursesList)
                {
                    ctx.Entry(code).State = EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var ctx = new OnlineExamBD())
            {
                string command = "RESTORE DATABASE AdventureWorks2012 FROM DISK = 'C:/Program Files/Microsoft SQL Server/MSSQL14.SQLEXPRESS/MSSQL/Backup/TestBD.bak'; ";
                ctx.Database.ExecuteSqlCommandAsync(command);
            }
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            using (var ctx = new OnlineExamBD())
            {
                ctx.Database.Delete();
            }
        }
    }
}
