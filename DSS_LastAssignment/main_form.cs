using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using java.io;
using weka.classifiers;
using weka.classifiers.bayes;
using weka.classifiers.functions;
using weka.classifiers.lazy;
using weka.classifiers.trees;
using weka.core;
using weka.filters;
using weka.filters.unsupervised.attribute;
using weka.filters.unsupervised.instance;
using File = System.IO.File;
using Normalize = weka.filters.unsupervised.instance.Normalize;

namespace DSS_LastAssignment
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            _count = 0;
            _max = -9999999;
            InitializeComponent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        OpenFileDialog _file = new OpenFileDialog();
        public String _fileDirectory = "";
        double _max;
        int _count;

        private void button1_Click(object sender, EventArgs e)
        {
            _file.Filter = "ARFF|*.arff";
            if (_file.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = _file.SafeFileName;
                _fileDirectory = _file.FileName;
            }
        }

        private static Classifier _clNaive;
        private static Classifier _clKnn;
        private static Classifier _clTree;
        private static Classifier _clNn;
        private static Classifier _clSvm;

        public MainForm(double max)
        {
            _count = 0;
            _max = max;
        }

        private const int PercentSplit = 66;

        private void result_Click(object sender, EventArgs e)
        {
            string classifierName = "";
            if (classifierName == null) throw new ArgumentNullException(nameof(classifierName));
            List<string> algorithms = new List<string>
            {
                "Naive Bayes",
                "K Nearest Neighbor",
                "Decision Tree",
                "Neural Network",
                "Support Vector Machine"
            };

            List<double> successPercent = new List<double>();

            //Naive Bayes
            Instances insts = new Instances(new FileReader(_fileDirectory));

            // Create a new DataGridView with 2 columns
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowCount = insts.numAttributes();
            String[,] matrixOfInstances = new String[insts.numInstances(), insts.numAttributes()];


            // Fill the DataGridView with the attribute names and their values
            for (int y = 0; y < insts.numAttributes() - 1; y++)
            {
                // Fill the first column with the attribute names
                dataGridView1.Rows[y].Cells[0].Value = insts.attribute(y).name();
                
                if (insts.attribute(y).isNominal())
                {
                    
                    string phrase = insts.attribute(y).toString();
                    string[] first = phrase.Split('{');

                    string[] second = first[1].Split('}');

                    string[] attributeValues = second[0].Split(',');

                    DataGridViewComboBoxCell comboColumn = new DataGridViewComboBoxCell();

                    foreach (var a in attributeValues)
                    {
                        comboColumn.Items.Add(a);
                    }
                    dataGridView1.Rows[y].Cells[1] = comboColumn;
                }
            }
            // Fill the last row with the class attribute
            insts.setClassIndex(insts.numAttributes() - 1);
            // Create a new instance of the NaiveBayes classifier
            
            //randomize the order of the instances in the dataset.
            Filter randomize = new Randomize();
            
            //Nominal to Binary
            Filter nominalToBinary = new NominalToBinary();
            //Normalization
            Filter normalize = new Normalize();
            
            _clNaive = new NaiveBayes();
            
            HandleNaiveBayes(insts, successPercent ,randomize);
            //kNN 

            Instances insts2 = new Instances(new FileReader(_fileDirectory));

            insts2.setClassIndex(insts2.numAttributes() - 1);

            _clKnn = new IBk();

            HandleKnn(insts2, successPercent, randomize, nominalToBinary, normalize);

            //Decision tree
            Instances insts3 = new Instances(new FileReader(_fileDirectory));

            insts3.setClassIndex(insts3.numAttributes() - 1);

            _clTree = new J48();



            HandleTree(insts3, successPercent, randomize, normalize);

            //Neural Network
            Instances insts4 = new Instances(new FileReader(_fileDirectory));

            insts4.setClassIndex(insts4.numAttributes() - 1);

            _clNn = new MultilayerPerceptron();

            HandleNn(insts4, successPercent, randomize , nominalToBinary , normalize);

            //SVM
            Instances insts5 = new Instances(new FileReader(_fileDirectory));

            insts5.setClassIndex(insts5.numAttributes() - 1);

            _clSvm = new SMO();

            HandleSvm(insts5, successPercent, randomize , nominalToBinary , normalize);


            for (int i = 0; i < successPercent.Count; i++)
            {

                if (successPercent[i] > _max)
                {
                    _max = successPercent[i];
                    _count = i + 1;
                }

            }
            for (int i = 0; i < _count; i++)
            {
                classifierName = algorithms[i];
            }

            label1.Text = $@"{classifierName} is the best algorithm with {_max:F2}% success rate.";


        }

        private static void HandleSvm(Instances insts5, List<double> successPercent, Filter random , Filter nominalToBinary , Filter normalize)
        {
            
            nominalToBinary.setInputFormat(insts5);
            insts5 = Filter.useFilter(insts5, nominalToBinary);

           
            normalize.setInputFormat(insts5);
            insts5 = Filter.useFilter(insts5, normalize);

            
            random.setInputFormat(insts5);
            insts5 = Filter.useFilter(insts5, random);

            int trainSize5 = insts5.numInstances() * PercentSplit / 100;
            int testSize5 = insts5.numInstances() - trainSize5;
            Instances train5 = new Instances(insts5, 0, trainSize5);

            _clSvm.buildClassifier(train5);

            _clSvm.toString();
            // Test the classifier
            var numCorrect5 = 0;
            numCorrect5 = TestAlgoritm(trainSize5, insts5, numCorrect5, _clSvm);
            var supportVectorMachine = numCorrect5 / (double)testSize5 * 100.0;
            successPercent.Add(supportVectorMachine);
        }

        private static void HandleNn(Instances insts4, List<double> successPercent , Filter random , Filter nominalToBinary , Filter normalize)
        {
            //Nominal to Binary
            nominalToBinary.setInputFormat(insts4);
            insts4 = Filter.useFilter(insts4, nominalToBinary);

            //Normalization
            normalize.setInputFormat(insts4);
            insts4 = Filter.useFilter(insts4, normalize);

           
            random.setInputFormat(insts4);
            insts4 = Filter.useFilter(insts4, random);

            int trainSize4 = insts4.numInstances() * PercentSplit / 100;
            int testSize4 = insts4.numInstances() - trainSize4;
            Instances train4 = new Instances(insts4, 0, trainSize4);

            _clNn.buildClassifier(train4);


            var numCorrect4 = 0;
            numCorrect4 = TestAlgoritm(trainSize4, insts4, numCorrect4, _clNn);

            var neuralNetwork = numCorrect4 / (double)testSize4 * 100.0;
            successPercent.Add(neuralNetwork);
        }

        private static void HandleTree(Instances insts3, List<double> successPercent , Filter random,Filter normalize)
        {
           
            normalize.setInputFormat(insts3);
            insts3 = Filter.useFilter(insts3, normalize);


           
            random.setInputFormat(insts3);
            insts3 = Filter.useFilter(insts3, random);

            int trainSize3 = insts3.numInstances() * PercentSplit / 100;
            int testSize3 = insts3.numInstances() - trainSize3;
            Instances train3 = new Instances(insts3, 0, trainSize3);

            _clTree.buildClassifier(train3);

            _clTree.toString();

            var numCorrect3 = 0;
            numCorrect3 = TestAlgoritm(trainSize3, insts3, numCorrect3, _clTree);
            var decisionTree = numCorrect3 / (double)testSize3 * 100.0;
            successPercent.Add(decisionTree);
        }

        private static void HandleKnn(Instances insts2, List<double> successPercent , Filter random, Filter nominalToBinary , Filter normalize)
        {
            //Nominal to Binary
           
            nominalToBinary.setInputFormat(insts2);
            insts2 = Filter.useFilter(insts2, nominalToBinary);

            //Normalization
        
            normalize.setInputFormat(insts2);
            insts2 = Filter.useFilter(insts2, normalize);
            
            random.setInputFormat(insts2);
            insts2 = Filter.useFilter(insts2, random);

            int trainSize2 = insts2.numInstances() * PercentSplit / 100;
            int testSize2 = insts2.numInstances() - trainSize2;
            Instances train2 = new Instances(insts2, 0, trainSize2);

            _clKnn.buildClassifier(train2);

            _clKnn.toString();

            var numCorrect2 = 0;
            numCorrect2 = TestAlgoritm(trainSize2, insts2, numCorrect2, _clKnn);
            var kNearestNeighbor = numCorrect2 / (double)testSize2 * 100.0;
            successPercent.Add(kNearestNeighbor);
        }

        private static void HandleNaiveBayes(Instances insts, List<double> successPercent ,Filter random)
        {
            Filter nominalData = new Discretize();
            nominalData.setInputFormat(insts);
            insts = Filter.useFilter(insts, nominalData);


            
            random.setInputFormat(insts);
            insts = Filter.useFilter(insts, random);

            int trainSize = insts.numInstances() * PercentSplit / 100;
            int testSize = insts.numInstances() - trainSize;
            Instances train = new Instances(insts, 0, trainSize);

            _clNaive.buildClassifier(train);

            _clNaive.toString();

            var numCorrect = 0;
            numCorrect = TestAlgoritm(trainSize, insts, numCorrect, _clNaive);
            var naiveBayes = numCorrect / (double)testSize * 100.0;
            successPercent.Add(naiveBayes);
        }

        private static int TestAlgoritm (int trainSize, Instances insts, int numCorrect , Classifier classifier)
        {
            for (int i = trainSize; i < insts.numInstances(); i++)
            {
                Instance currentInst = insts.instance(i);
                double predictedClass = classifier.classifyInstance(currentInst);
                if (predictedClass == insts.instance(i).classValue())
                    numCorrect++;
            }

            return numCorrect;
        }
        


        private void button_Discover_Click(object sender, EventArgs e)
        {
            String newDirectory = "test.arff"; // for algortihms
            // Clear the contents of the "test.arff" file
            File.WriteAllText(newDirectory, string.Empty);

            // Open the file in write mode to append new data
            using (StreamWriter sw = new StreamWriter(newDirectory, true))
            {
                // Read data from the specified file until encountering "@data" or "@DATA"
                using (StreamReader sr = new StreamReader(_fileDirectory))
                {
                    string line;
                    string comp = "@data";
                    string comp2 = "@DATA";

                    while ((line = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                        if (line == comp || line == comp2)
                            break;
                    }
                }

                // Construct a new instance based on user input from the DataGridView
                String s_newInstance = "";
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    s_newInstance += (String)dataGridView1.Rows[i].Cells[1].Value + ",";
                }
                s_newInstance += "?";
                sw.WriteLine(s_newInstance);
            }


            if (_count == 1)
            {
                Instances insts = new Instances(new FileReader(newDirectory));
                insts.setClassIndex(insts.numAttributes() - 1);

                Filter nominalData = new Discretize();
                nominalData.setInputFormat(insts);
                insts = Filter.useFilter(insts, nominalData);

                //randomize the order of the instances in the dataset.
                Filter myRandom = new Randomize();
                myRandom.setInputFormat(insts);
                insts = Filter.useFilter(insts, myRandom);

                double predictedClass = _clNaive.classifyInstance(insts.instance(0));
                label2.Text = insts.classAttribute().value(Convert.ToInt32(predictedClass));
            }
            else if (_count == 2)
            {
                Instances insts2 = new Instances(new FileReader(_fileDirectory));

                insts2.setClassIndex(insts2.numAttributes() - 1);

                //Nominal to Binary
                Filter myBinaryData = new NominalToBinary();
                myBinaryData.setInputFormat(insts2);
                insts2 = Filter.useFilter(insts2, myBinaryData);

                //Normalization
                Filter myNormalized = new Normalize();
                myNormalized.setInputFormat(insts2);
                insts2 = Filter.useFilter(insts2, myNormalized);

                //randomize the order of the instances in the dataset.
                Filter myRandom2 = new Randomize();
                myRandom2.setInputFormat(insts2);
                insts2 = Filter.useFilter(insts2, myRandom2);

                double predictedClass2 = _clKnn.classifyInstance(insts2.instance(0));
                label2.Text = insts2.classAttribute().value(Convert.ToInt32(predictedClass2));
            }
            else if (_count == 3)
            {
                Instances insts3 = new Instances(new FileReader(newDirectory));

                insts3.setClassIndex(insts3.numAttributes() - 1);
                Filter myNormalized2 = new Normalize();
                myNormalized2.setInputFormat(insts3);
                insts3 = Filter.useFilter(insts3, myNormalized2);


                //randomize the order of the instances in the dataset.
                Filter myRandom3 = new Randomize();
                myRandom3.setInputFormat(insts3);
                insts3 = Filter.useFilter(insts3, myRandom3);

                double predictedClass3 = _clTree.classifyInstance(insts3.instance(0));
                label2.Text = insts3.classAttribute().value(Convert.ToInt32(predictedClass3));
            }
            else if (_count == 4)
            {
                Instances insts4 = new Instances(new FileReader(newDirectory));
                insts4.setClassIndex(insts4.numAttributes() - 1);

                //cl = new weka.classifiers.functions.MultilayerPerceptron();
                //Nominal to Binary
                Filter myBinaryData2 = new NominalToBinary();
                myBinaryData2.setInputFormat(insts4);
                insts4 = Filter.useFilter(insts4, myBinaryData2);

                //Normalization
                Filter myNormalized3 = new Normalize();
                myNormalized3.setInputFormat(insts4);
                insts4 = Filter.useFilter(insts4, myNormalized3);

                //randomize the order of the instances in the dataset.
                Filter myRandom4 = new Randomize();
                myRandom4.setInputFormat(insts4);
                insts4 = Filter.useFilter(insts4, myRandom4);

                double predictedClass4 = _clNn.classifyInstance(insts4.instance(0));
                label2.Text = insts4.classAttribute().value(Convert.ToInt32(predictedClass4));
            }
            else if (_count == 5)
            {
                Instances insts5 = new Instances(new FileReader(newDirectory));

                insts5.setClassIndex(insts5.numAttributes() - 1);


                //Nominal to Binary
                Filter myBinaryData3 = new NominalToBinary();
                myBinaryData3.setInputFormat(insts5);
                insts5 = Filter.useFilter(insts5, myBinaryData3);

                //Normalization
                Filter myNormalized4 = new Normalize();
                myNormalized4.setInputFormat(insts5);
                insts5 = Filter.useFilter(insts5, myNormalized4);

                //randomize the order of the instances in the dataset.
                Filter myRandom5 = new Randomize();
                myRandom5.setInputFormat(insts5);
                insts5 = Filter.useFilter(insts5, myRandom5);

                double predictedClass5 = _clSvm.classifyInstance(insts5.instance(0));
                label2.Text = insts5.classAttribute().value(Convert.ToInt32(predictedClass5));
            }
            else
            {
                label2.Text = "Error!";
            }
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

