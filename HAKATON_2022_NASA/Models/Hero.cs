using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HAKATON_2022_NASA.Models
{
    internal class Hero
    {
        private string generationDescription;
        private int generationNumber = 0;
        private int stepsCount;
        private int mutationQualifer;
        private int makeStepCount;
        private int yPosition;
        private bool isAlive;
        private bool makeAllSteps = false;
        private Brush color;

        Random random = new Random();

        public string GenerationDescription
        {
            get { return generationDescription; }
            //set { generationDescription = value; }
        }
        public int StepsCount
        {
            get { return stepsCount; }
            set { stepsCount = value; }
        }
        public int MutationQualifer
        {
            get { return mutationQualifer; }
            set { mutationQualifer = value; }
        }
        public int MakeStepCount
        {
            get { return makeStepCount; }
            set { makeStepCount = value; }
        }
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
        public bool MakeAllSteps 
        { 
            get { return makeAllSteps; } 
            //set { makeAllSteps = value; }
        }
        public int GenerationNumber
        { 
            get { return generationNumber; } 
            //set { makeAllSteps = value; }
        }

        public Hero(int yPosition, int heroNumber, int mutationQualifer, Brush brushes)
        {
            this.yPosition = yPosition;
            makeStepCount = 10;
            stepsCount = 0;
            generationDescription = $"Hero {heroNumber}";
            isAlive = true;
            this.mutationQualifer = mutationQualifer;
            color = brushes;
        }

        public void makeMutation(int mutationType)
        {
            int mutation = random.Next(1, mutationType*3);
            if (mutation%5 == 0)
                MutationQualifer++;
            else if (mutation % 2 != 0)
                MakeStepCount = makeStepCount;
            else
                makeStepCount += random.Next(mutation / 2, mutation);
        }

        public Line makeStep()
        {
            Line line = new Line() { Stroke = color, X1 = 0, X2 = makeStepCount > 755 ? 755 : makeStepCount, Y1 = yPosition, Y2 = yPosition, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, StrokeThickness = 2 };
            stepsCount += MakeStepCount;
            if(makeStepCount >= 755)
                isAlive = false;
            generationNumber++;
            return line;
        }
    }
}
