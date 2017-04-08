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

namespace Fitts_Law
{
    public partial class Form1 : Form
    {
        //initializing variables
        double dragdistance = 0;
        private static Point initialPosition;
        private Point currentPoint = initialPosition;//setting current position to initial position
        bool isClicked = false;
        static System.Diagnostics.Stopwatch sWatch = new System.Diagnostics.Stopwatch();//stop watch
        Random rand = new Random();
        Rectangle rectangle;
        Graphics graphics;
        int Rollcount = 0;
        int trial = 120;

        //initializing fix distance
        private static Point distance1 = new Point(807, 115);
        private static Point distance2 = new Point(650, 115);
        private static Point distance3 = new Point(164, 115);
        private static Point distance4 = new Point(0, 115);

        //initialize fix sizes
        private static Size size1 = new Size(16, 16);
        private static Size size2 = new Size(32, 32);
        private static Size size3 = new Size(64, 64);

        List<Point> distance = new List<Point>(new Point[] { distance1, distance2, distance3, distance4 });
        List<Size> size = new List<Size>(new Size[] { size1, size2, size3 });
        List<string> timeTaken = new List<string>();
        List<double> distanceOfObject = new List<double>();
        List<double> distanceOfObject1 = new List<double>();
        List<double> lstCircleSizes = new List<double>();
        List<string> lstCircleDirection = new List<string>();
        int noOfErrors = 0;
        List<int> lstNoOfErrors = new List<int>();
        //creating circles
        static CircleObj circle1 = new CircleObj(size1, distance1);
        static CircleObj circle2 = new CircleObj(size1, distance2);
        static CircleObj circle3 = new CircleObj(size1, distance3);
        static CircleObj circle4 = new CircleObj(size1, distance4);
        static CircleObj circle5 = new CircleObj(size2, distance1);
        static CircleObj circle6 = new CircleObj(size2, distance2);
        static CircleObj circle7 = new CircleObj(size2, distance3);
        static CircleObj circle8 = new CircleObj(size2, distance4);
        static CircleObj circle9 = new CircleObj(size3, distance1);
        static CircleObj circle10 = new CircleObj(size3, distance2);
        static CircleObj circle11 = new CircleObj(size3, distance3);
        static CircleObj circle12 = new CircleObj(size3, distance4);

        //creating list of circle

        List<CircleObj> circle = new List<CircleObj>(new CircleObj[] { circle1, circle12, circle3, circle10, circle5, circle9, circle7, circle8, circle6, circle4, circle11, circle2 });
        //array of currently made circle
        CircleObj[] currentCircle = new CircleObj[1];

        Stack<CircleObj> myCollection = new Stack<CircleObj>();
        List<CircleObj> shuffledCircle = new List<CircleObj>();


        public Form1()
        {
            InitializeComponent();

            //getting the windows middle position
            initialPosition = new Point(this.Width / 2, this.Height / 2);
            label9.Text = initialPosition.ToString();
            label9.Text = "";

            //adding list element to stack 
            for (int i = 0; i < 10; i++)
            {
                addToStack();
            }
        }


        private void drawCircle()
        {
            //this method draw the circle taken from the stack of circles randomly
            graphics = this.CreateGraphics();
            currentCircle[0] = myCollection.Pop();

            rectangle = new Rectangle(currentCircle[0].location, currentCircle[0].Diameter);
            graphics.FillEllipse(Brushes.DarkSlateBlue, rectangle);
            label6.Text = currentCircle[0].Location.ToString();
            lstCircleSizes.Add(currentCircle[0].diameter.Height);

            if (currentCircle[0].location.X == 0 || currentCircle[0].location.X == 807)
            {
                distanceOfObject1.Add(512);
                if (currentCircle[0].location.X == 0)
                    lstCircleDirection.Add("Left");
                else
                { lstCircleDirection.Add("Right"); }

            }
            else
            {
                distanceOfObject1.Add(128);
                if (currentCircle[0].location.X == 164)
                    lstCircleDirection.Add("Left");
                else
                { lstCircleDirection.Add("Right"); }
            }



        }
        private void distances()
        {

        }
        private void clearGraphics()
        {
            //clear the circle from the screen
            graphics.Clear(Color.LightBlue);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            resultLabel.Text = "";
            button1.Visible = false;
            Rollcount++;
            label1.Text = "Total Trials: 120";
            label2.Text = "Trial: " + (Rollcount);
            label3.Text = "Remaining Trial: " + (trial - Rollcount);
            dragdistance = 0;

            if (Rollcount == trial + 1)//if the trial are all done
            {
                addToFile();
                label2.Text = "Trial: Finished";
                label3.Text = "Remaining Trial: 0";
                label8.Text = "You have successfully completed the trails";
                label5.Text = "";
                //cbk


            }
            else
            {
                if (rectangle.IsEmpty)//if circle is not in the screen
                {
                    isClicked = false;                    
                    sWatch.Start();
                    drawCircle();
                    calculateTime();
                }
                else//if circle is in the screen will clear the circle
                {
                    isClicked = false;
                    clearGraphics();                    
                    sWatch.Start();
                    drawCircle();
                    calculateTime();
                }
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            //this methods check if the circle is being clicked
            foreach (CircleObj obj in currentCircle)
            {
                if (obj.IsHit(e.X, e.Y))
                {
                    isClicked = true;
                    calculateTime();
                    sWatch.Reset();
                    resultLabel.Text = "Successfully Clicked!\n Move to next trial by clicking again on below circle";
                    dragdistance += Math.Abs(Math.Sqrt((Math.Pow((currentPoint.X - e.X), 2) + Math.Pow((currentPoint.Y - e.Y), 2))));
                    distanceOfObject.Add(dragdistance);
                    dragdistance = 0;
                    lstNoOfErrors.Add(noOfErrors);
                    noOfErrors = 0;
                    button1.Visible = true;
                    clearGraphics();
                }
                else
                {
                    noOfErrors++;
                }
            }
        }

        public void addToStack()
        {
            //this method add the circles from the list to the stack           
            Random rnd = new Random();
            for (int i = 0; i < 12; i++)
            {
                int positionCircle = rnd.Next(0, circle.Count - 1);
                myCollection.Push(circle[positionCircle]);
                circle.Remove(circle[positionCircle]);
            }
            circle = new List<CircleObj>(new CircleObj[] { circle1, circle12, circle3, circle10, circle5, circle9, circle7, circle8, circle6, circle4, circle11, circle2 });

        }

        public string calculateTime()
        {
            //this method calculates the time for the user to click on the circle 
            sWatch.Start();
            if (isClicked == true)
            {
                sWatch.Stop();
                double seconds = sWatch.Elapsed.TotalSeconds;
                label7.Text = seconds.ToString();
                timeTaken.Add(label7.Text);
            }
            return label7.Text;
        }

        public void addToFile()
        {
            //this method add the data to the file
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "user_records.csv");//saves the file to the user desktop
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine("Circle Radus(px),Circle Distance(px),Circle Position,Total Time(sec),Distance Traveled(Apporx),ID,NoOfErrors");
                for (int i = 0; i < timeTaken.Count() && i<120; i++)
                {
                    //  writer.WriteLine(timeTaken[i] + "," + distanceOfObject[i] + "," + lstCircleSizes[i] + "," + distanceOfObject1[i] + "," + lstCircleDirection[i] + "," + Math.Log(distanceOfObject[i] / lstCircleSizes[i] + 1) + "," + lstNoOfErrors[i]);
                    writer.WriteLine(lstCircleSizes[i] + "," + distanceOfObject1[i] + "," + lstCircleDirection[i] +","+ timeTaken[i] + ","+ distanceOfObject[i] + ","+ Math.Log(distanceOfObject[i] / lstCircleSizes[i] + 1,2.0) + ","+ lstNoOfErrors[i]);
                }
            }
        }
        
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //records the mouse movements position
            if (button1.Visible == false)
            {
                if (e.Location != currentPoint)//calculates the distance
                {
                    dragdistance += Math.Abs(Math.Sqrt((Math.Pow((currentPoint.X - e.X), 2) + Math.Pow((currentPoint.Y - e.Y), 2))));

                }
                currentPoint = e.Location;
            }
        }

    

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
