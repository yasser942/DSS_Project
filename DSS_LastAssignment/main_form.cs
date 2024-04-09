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
        private const int PercentSplit = 80;
        private static Classifier _clNaive;
        private static Classifier _clKnn;
        private static Classifier _clTree;
        private static Classifier _clNn;
        private static Classifier _clSvm;
        private readonly OpenFileDialog _file = new OpenFileDialog();

        //Nominal to Binary
        private readonly Filter _nominalToBinary = new NominalToBinary();

        //Normalization
        private readonly Filter _normalize = new Normalize();

        //randomize the order of the instances in the dataset.
        private readonly Filter _randomize = new Randomize();
        private string _classifierName = "";
        private string _fileDirectory = "";
        private double _rate;

        public MainForm()
        {
            _rate = 0.0;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            _file.Filter = "ARFF|*.arff";
            if (_file.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = _file.SafeFileName;
                _fileDirectory = _file.FileName;
            }
        }


        private void result_Click(object sender, EventArgs e)
        {
            if (_classifierName == null) throw new ArgumentNullException(nameof(_classifierName));
            var algorithms = new List<string>
            {
                "Naive Bayes",
                "K Nearest Neighbor",
                "Decision Tree",
                "Neural Network",
                "Support Vector Machine"
            };

            var successPercent = new List<double>();

            //Naive Bayes
            var insts = new Instances(new FileReader(_fileDirectory));
            //Render the view
            RenderView(insts);

            _clNaive = new NaiveBayes();

            HandleNaiveBayes(insts, successPercent, _randomize);

            //kNN 

            var insts2 = new Instances(new FileReader(_fileDirectory));

            insts2.setClassIndex(insts2.numAttributes() - 1);

            _clKnn = new IBk();

            HandleKnn(insts2, successPercent, _randomize, _nominalToBinary, _normalize);

            //Decision tree
            var insts3 = new Instances(new FileReader(_fileDirectory));

            insts3.setClassIndex(insts3.numAttributes() - 1);

            _clTree = new J48();


            HandleTree(insts3, successPercent, _randomize, _normalize);

            //Neural Network
            var insts4 = new Instances(new FileReader(_fileDirectory));

            insts4.setClassIndex(insts4.numAttributes() - 1);

            _clNn = new MultilayerPerceptron();

            HandleNn(insts4, successPercent, _randomize, _nominalToBinary, _normalize);

            //SVM
            var insts5 = new Instances(new FileReader(_fileDirectory));

            insts5.setClassIndex(insts5.numAttributes() - 1);

            _clSvm = new SMO();

            HandleSvm(insts5, successPercent, _randomize, _nominalToBinary, _normalize);


            for (var i = 0; i < successPercent.Count; i++)
                if (successPercent[i] > _rate)
                {
                    _rate = successPercent[i];
                    _classifierName = algorithms[i];
                }

            label1.Text = $@"{_classifierName} is the best algorithm with {_rate:F2}% success rate.";
        }

        private void RenderView(Instances insts)
        {
            // Create a new DataGridView with 2 columns
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowCount = insts.numAttributes();
            var matrixOfInstances = new string[insts.numInstances(), insts.numAttributes()];


            // Fill the DataGridView with the attribute names and their values
            for (var y = 0; y < insts.numAttributes() - 1; y++)
            {
                // Fill the first column with the attribute names
                dataGridView1.Rows[y].Cells[0].Value = insts.attribute(y).name();

                if (insts.attribute(y).isNominal())
                {
                    var phrase = insts.attribute(y).toString();
                    var first = phrase.Split('{');

                    var second = first[1].Split('}');

                    var attributeValues = second[0].Split(',');

                    var comboColumn = new DataGridViewComboBoxCell();

                    foreach (var a in attributeValues) comboColumn.Items.Add(a);

                    dataGridView1.Rows[y].Cells[1] = comboColumn;
                }
            }

            // Fill the last row with the class attribute
            insts.setClassIndex(insts.numAttributes() - 1);
        }

        private static void HandleSvm(Instances insts5, List<double> successPercent, Filter random,
            Filter nominalToBinary, Filter normalize)
        {
            //Nominal to Binary
            insts5 = NominalToBinary(nominalToBinary, insts5);

            //Normalization
            insts5 = Normalize(normalize, insts5);

            //randomize the order of the instances in the dataset.
            insts5 = Randomize(random, insts5);

            var trainSize5 = insts5.numInstances() * PercentSplit / 100;
            var testSize5 = insts5.numInstances() - trainSize5;
            var train5 = new Instances(insts5, 0, trainSize5);

            _clSvm.buildClassifier(train5);

            _clSvm.toString();
            // Test the classifier
            var numCorrect5 = 0;
            numCorrect5 = TestAlgorithm(trainSize5, insts5, numCorrect5, _clSvm);
            var supportVectorMachine = numCorrect5 / (double)testSize5 * 100.0;
            successPercent.Add(supportVectorMachine);
        }

        private static void HandleNn(Instances insts4, List<double> successPercent, Filter random,
            Filter nominalToBinary, Filter normalize)
        {
            //Nominal to Binary
            insts4 = NominalToBinary(nominalToBinary, insts4);

            //Normalization
            insts4 = Normalize(normalize, insts4);

            //randomize the order of the instances in the dataset.   
            insts4 = Randomize(random, insts4);

            var trainSize4 = insts4.numInstances() * PercentSplit / 100;
            var testSize4 = insts4.numInstances() - trainSize4;
            var train4 = new Instances(insts4, 0, trainSize4);

            _clNn.buildClassifier(train4);


            var numCorrect4 = 0;
            numCorrect4 = TestAlgorithm(trainSize4, insts4, numCorrect4, _clNn);

            var neuralNetwork = numCorrect4 / (double)testSize4 * 100.0;
            successPercent.Add(neuralNetwork);
        }

        private static void HandleTree(Instances insts3, List<double> successPercent, Filter random, Filter normalize)
        {
            //Normalization
            insts3 = Normalize(normalize, insts3);
            //randomize the order of the instances in the dataset.
            insts3 = Randomize(random, insts3);

            var trainSize3 = insts3.numInstances() * PercentSplit / 100;
            var testSize3 = insts3.numInstances() - trainSize3;
            var train3 = new Instances(insts3, 0, trainSize3);

            _clTree.buildClassifier(train3);

            _clTree.toString();

            var numCorrect3 = 0;
            numCorrect3 = TestAlgorithm(trainSize3, insts3, numCorrect3, _clTree);
            var decisionTree = numCorrect3 / (double)testSize3 * 100.0;
            successPercent.Add(decisionTree);
        }

        private static void HandleKnn(Instances insts2, List<double> successPercent, Filter random,
            Filter nominalToBinary, Filter normalize)
        {
            //Nominal to Binary

            insts2 = NominalToBinary(nominalToBinary, insts2);

            //Normalization
            insts2 = Normalize(normalize, insts2);
            //randomize the order of the instances in the dataset.
            insts2 = Randomize(random, insts2);

            var trainSize2 = insts2.numInstances() * PercentSplit / 100;
            var testSize2 = insts2.numInstances() - trainSize2;
            var train2 = new Instances(insts2, 0, trainSize2);

            _clKnn.buildClassifier(train2);

            _clKnn.toString();

            var numCorrect2 = 0;
            numCorrect2 = TestAlgorithm(trainSize2, insts2, numCorrect2, _clKnn);
            var kNearestNeighbor = numCorrect2 / (double)testSize2 * 100.0;
            successPercent.Add(kNearestNeighbor);
        }

        private static void HandleNaiveBayes(Instances insts, List<double> successPercent, Filter random)
        {
            Filter nominalData = new Discretize();
            nominalData.setInputFormat(insts);
            insts = Filter.useFilter(insts, nominalData);


            //randomize the order of the instances in the dataset.
            insts = Randomize(random, insts);

            var trainSize = insts.numInstances() * PercentSplit / 100;
            var testSize = insts.numInstances() - trainSize;
            var train = new Instances(insts, 0, trainSize);

            _clNaive.buildClassifier(train);

            _clNaive.toString();

            var numCorrect = 0;
            numCorrect = TestAlgorithm(trainSize, insts, numCorrect, _clNaive);
            var naiveBayes = numCorrect / (double)testSize * 100.0;
            successPercent.Add(naiveBayes);
        }

        private static int TestAlgorithm(int trainSize, Instances insts, int numCorrect, Classifier classifier)
        {
            for (var i = trainSize; i < insts.numInstances(); i++)
            {
                var currentInst = insts.instance(i);
                var predictedClass = classifier.classifyInstance(currentInst);
                if (predictedClass == insts.instance(i).classValue())
                    numCorrect++;
            }

            return numCorrect;
        }


        private void button_Discover_Click(object sender, EventArgs e)
        {
            var newDirectory = "test.arff"; // The new file to be created
            // Clear the contents of the "test.arff" file
            File.WriteAllText(newDirectory, string.Empty);

            // Open the file in write mode to append new data
            using (var sw = new StreamWriter(newDirectory, true))
            {
                // Read data from the specified file until encountering "@data" or "@DATA"
                using (var sr = new StreamReader(_fileDirectory))
                {
                    string line;
                    var comp = "@data";
                    var comp2 = "@DATA";

                    while ((line = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                        if (line == comp || line == comp2)
                            break;
                    }
                }

                // Construct a new instance based on user input from the DataGridView
                var sNewInstance = "";
                for (var i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    sNewInstance += (string)dataGridView1.Rows[i].Cells[1].Value + ",";
                sNewInstance += "?";
                sw.WriteLine(sNewInstance);
            }


            switch (_classifierName)
            {
                case "Naive Bayes":
                {
                    HandleNaiveByesInput(newDirectory, _randomize);
                    break;
                }
                case "K Nearest Neighbor":
                {
                    HandleKnnInput(newDirectory, _randomize, _nominalToBinary, _normalize);
                    break;
                }
                case "Decision Tree":
                {
                    HandleDecisionTreeInput(newDirectory, _randomize, _normalize);
                    break;
                }
                case "Neural Network":
                {
                    HandleNnInput(newDirectory, _randomize, _nominalToBinary, _normalize);
                    break;
                }
                case "Support Vector Machine":
                {
                    HandleSvmInput(newDirectory, _randomize, _nominalToBinary, _normalize);
                    break;
                }
                default:
                {
                    label2.Text = @"Error!";
                    break;
                }
            }
        }

        private void HandleSvmInput(string newDirectory, Filter random, Filter nominalToBinary, Filter normalize)
        {
            var insts5 = new Instances(new FileReader(newDirectory));

            insts5.setClassIndex(insts5.numAttributes() - 1);


            //Nominal to Binary

            insts5 = NominalToBinary(nominalToBinary, insts5);

            //Normalization

            insts5 = Normalize(normalize, insts5);

            //randomize the order of the instances in the dataset.

            insts5 = Randomize(random, insts5);

            var predictedClass5 = _clSvm.classifyInstance(insts5.instance(0));
            label2.Text = insts5.classAttribute().value(Convert.ToInt32(predictedClass5));
        }

        private void HandleNnInput(string newDirectory, Filter random, Filter nominalToBinary, Filter normalize)
        {
            var insts4 = new Instances(new FileReader(newDirectory));
            insts4.setClassIndex(insts4.numAttributes() - 1);
            
            //Nominal to Binary

            insts4 = NominalToBinary(nominalToBinary, insts4);

            //Normalization
            insts4 = Normalize(normalize, insts4);

            //randomization
            insts4 = Randomize(random, insts4);

            var predictedClass4 = _clNn.classifyInstance(insts4.instance(0));
            label2.Text = insts4.classAttribute().value(Convert.ToInt32(predictedClass4));
        }

        private void HandleDecisionTreeInput(string newDirectory, Filter random, Filter normalize)
        {
            var insts3 = new Instances(new FileReader(newDirectory));

            insts3.setClassIndex(insts3.numAttributes() - 1);
            //Normalization
            insts3 = Normalize(normalize, insts3);

            //randomize the order of the instances in the dataset.

            insts3 = Randomize(random, insts3);

            var predictedClass3 = _clTree.classifyInstance(insts3.instance(0));
            label2.Text = insts3.classAttribute().value(Convert.ToInt32(predictedClass3));
        }

        private void HandleKnnInput(string newDirectory, Filter random, Filter nominalToBinary, Filter normalize)
        {
            var insts2 = new Instances(new FileReader(newDirectory));

            insts2.setClassIndex(insts2.numAttributes() - 1);
            //Nominal to Binary
            insts2 = NominalToBinary(nominalToBinary, insts2);
            //Normalization
            insts2 = Normalize(normalize, insts2);
            //randomize the order of the instances in the dataset.
            insts2 = Randomize(random, insts2);

            var predictedClass2 = _clKnn.classifyInstance(insts2.instance(0));
            label2.Text = insts2.classAttribute().value(Convert.ToInt32(predictedClass2));
        }

        private static Instances Randomize(Filter random, Instances insts)
        {
            //randomize the order of the instances in the dataset.

            random.setInputFormat(insts);
            insts = Filter.useFilter(insts, random);
            return insts;
        }

        private static Instances Normalize(Filter normalize, Instances insts)
        {
            //Normalization

            normalize.setInputFormat(insts);
            insts = Filter.useFilter(insts, normalize);
            return insts;
        }

        private static Instances NominalToBinary(Filter nominalToBinary, Instances insts)
        {
            //Nominal to Binary
            nominalToBinary.setInputFormat(insts);
            insts = Filter.useFilter(insts, nominalToBinary);
            return insts;
        }

        private void HandleNaiveByesInput(string newDirectory, Filter random)
        {
            var insts = new Instances(new FileReader(newDirectory));
            insts.setClassIndex(insts.numAttributes() - 1);

            Filter nominalData = new Discretize();
            nominalData.setInputFormat(insts);
            insts = Filter.useFilter(insts, nominalData);

            //randomize the order of the instances in the dataset.
            insts = Randomize(random, insts);

            var predictedClass = _clNaive.classifyInstance(insts.instance(0));
            label2.Text = insts.classAttribute().value(Convert.ToInt32(predictedClass));
        }
        

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}