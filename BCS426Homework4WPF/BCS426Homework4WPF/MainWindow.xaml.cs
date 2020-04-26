//****************************************************
// File: MainWindow.xaml.cs
//
// Purpose: Utilize the dynamic link library and the WPF 
//          to interact with users
//
// Written By: Samantha Smith
//
// Compiler: Visual Studio 2019
// 
// Update Information
// ------------------
//
// 
//****************************************************
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BCS426Homework4WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //****************************************************
        // Method: void ButtonFindSubmission_Click(object sender, RoutedEventArgs e)
        //
        // Purpose: finds the submission of the user inputed assignment requested
        //
        //****************************************************
        private void ButtonFindSubmission_Click(object sender, RoutedEventArgs e)
        {   
            //creates a CourseWork instance and reads it
            BCS426Homework4DLL.CourseWork cw = new BCS426Homework4DLL.CourseWork();
            cw = ReadFromFile("courseworkansi.json");

            //calls FindSubmission and returns a submission or null depending on the result
            BCS426Homework4DLL.Submission s = cw.FindSubmission(textBoxTarget.Text);
            //if submission is not null it will put the data into the text boxes
            if (s != null)
            {
                textBoxAssignment.Text = s.AssignmentName;
                textBoxCategory.Text = s.CategoryName;
                textBoxGrade.Text = s.Grade.ToString(); //converts the type to a string
            }
            //if it is null it clears the textboxes
            else
            {
                textBoxAssignment.Clear();
                textBoxCategory.Clear();
                textBoxGrade.Clear();
            }
        }

        //****************************************************
        // Method: void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        //
        // Purpose: opens the windows explorer to the current working directory 
        //          for it to populate the listviews
        //
        //****************************************************
        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            //course work instance
            BCS426Homework4DLL.CourseWork cw = new BCS426Homework4DLL.CourseWork();

            //opens the file dialog box
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //filters the files to only show json files
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            //openFileDialog.InitialDirectory = @"C:\Users\Samantha\Documents\Fall 2019\BCS426\BCS426Homework4WPF\BCS426Homework4WPF\bin\Debug";
            //sets the intital directory to the current working directory
            openFileDialog.InitialDirectory = @System.IO.Directory.GetCurrentDirectory();
            //changes the title of the dialog box
            openFileDialog.Title = "Open Course Work From JSON";

            if (openFileDialog.ShowDialog() == true) {
                //gets the filename wanted
                string filename = openFileDialog.FileName;
                textBoxCourseWorkFilename.Text = filename;
                //reads from the file selected into the coursework instance
                cw = ReadFromFile(filename);
                //calls populate which populates the listviews
                Populate(cw);
                //clears the find submission text boxes
                textBoxAssignment.Clear();
                textBoxCategory.Clear();
                textBoxGrade.Clear();

            }
        }

        //****************************************************
        // Method: BCS426Homework4DLL.CourseWork ReadFromFile(string filename)
        //
        // Purpose: read from the file using serialization
        //
        //****************************************************
        private BCS426Homework4DLL.CourseWork ReadFromFile(string filename)
        {
            //creates a course work instance
            BCS426Homework4DLL.CourseWork cw = new BCS426Homework4DLL.CourseWork();

            FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read);   //opens the FileStream to read the JSON from 

            DataContractJsonSerializer serializer;          //creates an instance of DataContractJsonSerializer          
            serializer = new DataContractJsonSerializer(typeof(BCS426Homework4DLL.CourseWork));

            cw = (BCS426Homework4DLL.CourseWork)serializer.ReadObject(reader);    //reads from the file
            reader.Close();                              //closes the file
            return cw;                                  //returns the coursework instance
            

        }

        //****************************************************
        // Method: void Populate(BCS426Homework4DLL.CourseWork courseWork)
        //
        // Purpose: populates the listviews
        //
        //****************************************************
        private void Populate(BCS426Homework4DLL.CourseWork courseWork)
        {
            //puts the course name into the textbox
            textBoxCourseName.Text = courseWork.CourseName;
            //puts the grade into the textbox
            textBoxOverallGrade.Text = courseWork.CalculateGrade().ToString();  //converts to string

            //loops through all the categories and adds to the category listview
            foreach (BCS426Homework4DLL.Category cat in courseWork.Categories)
            {
                listViewCategories.Items.Add(cat);
            }
            //loops through all the assignments and adds to the assignment listview
            foreach (BCS426Homework4DLL.Assignment assign in courseWork.Assignments)
            {
                listViewAssignments.Items.Add(assign);
            }
            //loops through all the submissions and adds to the submission listview
            foreach (BCS426Homework4DLL.Submission sub in courseWork.Submissions)
            {
                listViewSubmissions.Items.Add(sub);
            }
        }
    }
        
}


